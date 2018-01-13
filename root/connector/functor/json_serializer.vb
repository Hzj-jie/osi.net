
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.delegates

Public NotInheritable Class json_serializer
    Public Shared Sub register(Of T)(ByVal to_str As Action(Of T, StringWriter))
        json_serializer(Of T).register(to_str)
    End Sub

    Public Shared Sub register(Of T)(ByVal from_str As _do_val_ref(Of StringReader, T, Boolean))
        json_serializer(Of T).register(from_str)
    End Sub

    Public Shared Function from_str(Of T)(ByVal i As StringReader, ByRef o As T) As Boolean
        Return json_serializer(Of T).default.from_str(i, o)
    End Function

    Public Shared Function from_str(Of T)(ByVal i As String, ByRef o As T) As Boolean
        Return json_serializer(Of T).default.from_str(i, o)
    End Function

    Public Shared Function to_str(Of T)(ByVal i As T, ByVal o As StringWriter) As Boolean
        Return json_serializer(Of T).default.to_str(i, o)
    End Function

    Public Shared Function to_str(Of T)(ByVal i As T, ByRef o As String) As Boolean
        Return json_serializer(Of T).default.to_str(i, o)
    End Function

    Public Shared Function to_str(Of T)(ByVal i As T) As String
        Return json_serializer(Of T).default.to_str(i)
    End Function

    Private Sub New()
    End Sub
End Class

' From or to json strings, with type information. This serializer works with both simple types and complex structures.
Public Class json_serializer(Of T)
    Inherits string_serializer(Of T, json_serializer(Of T))

    Public Shared Shadows ReadOnly [default] As json_serializer(Of T)

    Shared Sub New()
        [default] = New json_serializer(Of T)()
    End Sub

    Public Shared Shadows Operator +(ByVal this As json_serializer(Of T)) As json_serializer(Of T)
        If this Is Nothing Then
            Return [default]
        End If
        Return this
    End Operator

    Protected Sub New()
    End Sub
End Class
