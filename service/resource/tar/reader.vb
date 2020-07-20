
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utils

Partial Public NotInheritable Class tar
    Public NotInheritable Class reader
        Private ReadOnly v As vector(Of String)
        Private ReadOnly f As Func(Of String, MemoryStream, Boolean)
        Private ReadOnly m As MemoryStream
        Private i As UInt32

        Public Sub New(ByVal v As vector(Of String))
            Me.New(v,
                   Function(ByVal f As String, ByVal o As MemoryStream) As Boolean
                       assert(Not o Is Nothing)
                       Return o.read_from_file(f)
                   End Function)
        End Sub

        Private Sub New(ByVal v As vector(Of String), ByVal read As Func(Of String, MemoryStream, Boolean))
            assert(Not v Is Nothing)
            assert(Not read Is Nothing)
            Me.v = v
            Me.f = read
            Me.m = New MemoryStream()
        End Sub

        Public Shared Function of_testing(ByVal v As vector(Of MemoryStream)) As reader
            Return New reader(streams.range(0, v.size()).
                                      map(AddressOf Convert.ToString).
                                      collect(Of vector(Of String))(),
                              Function(ByVal index_str As String, ByVal o As MemoryStream) As Boolean
                                  Dim index As UInt32 = 0
                                  index = assert_which.of(index_str).can_cast_to_uint32()
                                  v(assert_which.of(index).less_than(v.size())).CopyTo(o)
                                  Return True
                              End Function)
        End Function

        Public Shared Function unzip(ByVal v As vector(Of String)) As reader
            Return New reader(v,
                              Function(ByVal f As String, ByVal o As MemoryStream) As Boolean
                                  assert(Not o Is Nothing)
                                  Return o.unzip_from_file(f)
                              End Function)
        End Function

        Public Function [next](ByRef name As String, ByRef o As MemoryStream) As Boolean
            While m.eos() AndAlso i < v.size()
                m.clear()
                Using m.keep_position()
                    If Not f(v(i), m) Then
                        Return False
                    End If
                End Using
                i += uint32_1
            End While
            If m.eos() Then
                Return False
            End If
            Return bytes_serializer.consume_from(m, name) AndAlso
                   bytes_serializer.consume_from(m, o)
        End Function

        Public Sub foreach(ByVal f As Action(Of String, MemoryStream))
            assert(Not f Is Nothing)
            Dim n As String = Nothing
            Dim m As MemoryStream = Nothing
            While [next](n, m)
                Try
                    f(n, m)
                Catch ex As break_lambda
                    Return
                End Try
            End While
        End Sub

        Public Function dump() As vector(Of tuple(Of String, MemoryStream))
            Dim v As vector(Of tuple(Of String, MemoryStream)) = Nothing
            v = New vector(Of tuple(Of String, MemoryStream))()
            Dim s As String = Nothing
            Dim m As MemoryStream = Nothing
            While [next](s, m)
                v.emplace_back(tuple.emplace_of(s, m))
            End While
            Return v
        End Function

        Public Function index() As vector(Of String)
            Dim v As vector(Of String) = Nothing
            v = New vector(Of String)()
            Dim s As String = Nothing
            Dim m As MemoryStream = Nothing
            While [next](s, m)
                v.emplace_back(s)
            End While
            Return v
        End Function
    End Class
End Class
