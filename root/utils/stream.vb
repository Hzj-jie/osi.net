
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.IO
Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.root.formation

Public Module _stream
#If 0 Then
    Private ReadOnly empty_buff() As Byte

    Sub New()
        '-1 means nothing?
        ReDim empty_buff(0)
        assert(npos < 0)
    End Sub
#End If

    'not only whether the stream can write, but also try to write an empty buff
    <Extension()> Public Function can_read(ByVal i As Stream) As Boolean
        Return i IsNot Nothing AndAlso i.CanRead()
#If 0 Then
        If i Is Nothing OrElse Not i.CanRead() Then
            Return False
        Else
            Try
                i.read(empty_buff, 0, 0)
                Return True
            Catch
                Return False
            End Try
        End If
#End If
    End Function

    <Extension()> Public Function can_write(ByVal i As Stream) As Boolean
        Return i IsNot Nothing AndAlso i.CanWrite()
#If 0 Then
        If i Is Nothing OrElse Not i.CanWrite() Then
            Return False
        Else
            Try
                i.write(empty_buff, 0, 0)
                Return True
            Catch
                Return False
            End Try
        End If
#End If
    End Function

    <Extension()> Public Function data_available(ByVal i As NetworkStream) As ternary
        If i Is Nothing Then
            Return ternary.unknown
        End If
        Try
            If i.DataAvailable() Then
                Return ternary.true
            End If
            Return ternary.false
        Catch
            Return ternary.unknown
        End Try
    End Function

    <Extension()> Public Function keep_position(ByVal i As Stream) As IDisposable
        assert(i IsNot Nothing)
        Dim p As Int64 = 0
        p = i.Position()
        Return defer.to(Sub()
                            i.Position() = p
                        End Sub)
    End Function
End Module
