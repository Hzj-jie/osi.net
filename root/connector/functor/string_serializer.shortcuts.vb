
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.delegates

Partial Public NotInheritable Class string_serializer
    Public Shared Function forward_to_str(Of T)(ByVal i As T,
                                                ByRef o As String,
                                                ByVal v As Func(Of T, StringWriter, Boolean)) As Boolean
        assert(Not v Is Nothing)
        Using sw As StringWriter = New StringWriter()
            If Not v(i, sw) Then
                Return False
            End If
            o = Convert.ToString(sw)
            Return True
        End Using
    End Function

    Public Shared Function forward_from_str(Of T)(ByVal i As String,
                                                  ByRef o As T,
                                                  ByVal v As _do_val_ref(Of StringReader, T, Boolean)) As Boolean
        If i Is Nothing Then
            Return False
        End If
        Using sr As StringReader = New StringReader(i)
            Return v(sr, o) AndAlso sr.Peek() = npos
        End Using
    End Function
End Class

Partial Public Class string_serializer(Of T, PROTECTOR)
    Public Shared Sub register(ByVal to_str As Action(Of T, StringWriter),
                               ByVal from_str As _do_val_ref(Of StringReader, T, Boolean))
        register(to_str)
        register(from_str)
    End Sub

    Public Function to_str(ByVal i As T, ByRef o As String) As Boolean
        Return string_serializer.forward_to_str(i, o, AddressOf to_str)
    End Function

    Public Function to_str(ByVal i As T) As String
        Dim o As String = Nothing
        assert(to_str(i, o))
        Return o
    End Function

    Public Function to_str_or_default(ByVal i As T, ByVal [default] As String) As String
        Dim o As String = Nothing
        If to_str(i, o) Then
            Return o
        End If
        Return [default]
    End Function

    Public Function to_str_or_null(ByVal i As T) As String
        Return to_str_or_default(i, Nothing)
    End Function

    Public Function from_str(ByVal i As String, ByRef o As T) As Boolean
        Return string_serializer.forward_from_str(i, o, AddressOf from_str)
    End Function

    Public Function from_str(ByVal i As String) As T
        Dim o As T = Nothing
        assert(from_str(i, o))
        Return o
    End Function

    Public Function from_str_or_default(ByVal i As String, ByVal [default] As T) As T
        Dim r As T = Nothing
        If from_str(i, r) Then
            Return r
        End If
        Return [default]
    End Function

    Public Function from_str_or_null(ByVal i As String) As T
        Return from_str_or_default(i, Nothing)
    End Function
End Class

Public Module _string_serializer
    <Extension()> Public Function [to](Of T)(ByVal s As String, Optional ByVal [default] As T = Nothing) As T
        Return string_serializer.from_str_or_default(s, [default])
    End Function

    <Extension()> Public Function [from](Of T)(ByRef s As String, ByVal v As T) As String
        s = string_serializer.to_str(v)
        Return s
    End Function
End Module