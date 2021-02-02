
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.delegates

Public NotInheritable Class string_serializer_object(Of T)
    Public Shared Function [of](Of PROTECTOR)(ByVal i As string_serializer(Of T, PROTECTOR)) _
                                             As string_serializer_object(Of T, PROTECTOR)
        Return New string_serializer_object(Of T, PROTECTOR)(i)
    End Function

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class string_serializer_object(Of T, PROTECTOR)
    Inherits string_serializer(Of Object)

    Private ReadOnly s As string_serializer(Of T, PROTECTOR)

    Public Sub New(ByVal s As string_serializer(Of T, PROTECTOR))
        assert(Not s Is Nothing)
        Me.s = s
    End Sub

    Protected Overrides Function to_str() As Func(Of Object, StringWriter, Boolean)
        Return Function(ByVal i As Object, ByVal o As StringWriter) As Boolean
                   Return s.to_str(direct_cast(Of T)(i), o)
               End Function
    End Function

    Protected Overrides Function from_str() As _do_val_ref(Of StringReader, Object, Boolean)
        Return Function(ByVal i As StringReader, ByRef o As Object) As Boolean
                   Dim x As T = Nothing
                   If s.from_str(i, x) Then
                       o = x
                       Return True
                   End If
                   Return False
               End Function
    End Function
End Class
