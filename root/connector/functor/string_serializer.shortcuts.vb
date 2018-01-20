
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.delegates

Partial Public Class string_serializer(Of T, PROTECTOR)
    Public Shared Sub register(ByVal to_str As Action(Of T, StringWriter),
                               ByVal from_str As _do_val_ref(Of StringReader, T, Boolean))
        register(to_str)
        register(from_str)
    End Sub

    Public Function to_str(ByVal i As T) As String
        Using sw As StringWriter = New StringWriter()
            assert(to_str(i, sw))
            Return Convert.ToString(sw)
        End Using
    End Function

    Public Function from_str(ByVal i As String, ByRef o As T) As Boolean
        If i Is Nothing Then
            Return False
        End If
        Using sr As StringReader = New StringReader(i)
            Return from_str(sr, o) AndAlso sr.Peek() = npos
        End Using
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