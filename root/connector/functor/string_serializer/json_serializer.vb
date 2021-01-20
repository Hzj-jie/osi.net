
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
        Return json_serializer(Of T).r.from_str(i, o)
    End Function

    Public Shared Function from_str(Of T)(ByVal i As String, ByRef o As T) As Boolean
        Return json_serializer(Of T).r.from_str(i, o)
    End Function

    Public Shared Function to_str(Of T)(ByVal i As T, ByVal o As StringWriter) As Boolean
        Return json_serializer(Of T).r.to_str(i, o)
    End Function

    Public Shared Function to_str(Of T)(ByVal i As T) As String
        Return json_serializer(Of T).r.to_str(i)
    End Function

    Private Sub New()
    End Sub
End Class

' From or to json strings, with type information. This serializer works with both simple types and complex structures.
Partial Public NotInheritable Class json_serializer(Of T)
    Inherits string_serializer(Of T, json_serializer(Of T))

    Public Shared ReadOnly r As json_serializer(Of T) = New json_serializer(Of T)()

    Protected Overrides Function to_str() As Func(Of T, StringWriter, Boolean)
        If type_info(Of T).can_cast_to_array_type Then
            Return Function(ByVal i As T, ByVal o As StringWriter) As Boolean
                       Dim a As Array = Nothing
                       a = direct_cast(Of Array)(i)
                       If a Is Nothing Then
                           o.Write("[]")
                       End If
                       o.Write("[")
                       For j As Int32 = 0 To a.GetLength(0) - 1
                           If j > 0 Then
                               o.Write(",")
                           End If
                           o.Write(type_json_serializer.r.to_str_or_null(a.GetValue(j)))
                       Next
                       o.Write("]")
                       Return True
                   End Function
        End If
        Return MyBase.to_str()
    End Function

    ' TODO: Implementation.
    Protected Overrides Function from_str() As _do_val_ref(Of StringReader, T, Boolean)
        Return MyBase.from_str()
    End Function

    Protected Sub New()
    End Sub
End Class
