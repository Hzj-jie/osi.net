
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.constants
Imports osi.root.delegates

Partial Public Class string_serializer(Of T, PROTECTOR)
    Public Shared Sub register(ByVal to_str As Action(Of T, StringWriter),
                               ByVal from_str As _do_val_ref(Of StringReader, T, Boolean))
        register(to_str)
        register(from_str)
    End Sub

    Public Function to_str(ByVal i As T, ByRef o As String) As Boolean
        Using sw As StringWriter = New StringWriter()
            If to_str(i, sw) Then
                o = Convert.ToString(sw)
                Return True
            Else
                Return False
            End If
        End Using
    End Function

    Public Function to_str(ByVal i As T) As String
        Dim r As String = Nothing
        assert(to_str(i, r))
        Return r
    End Function

    Public Function from_str(ByVal i As String, ByRef o As T) As Boolean
        Using sr As StringReader = New StringReader(i)
            Return from_str(sr, o) AndAlso sr.Peek() = npos
        End Using
    End Function
End Class
