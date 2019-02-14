
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.delegates

Partial Public Class bytes_serializer(Of T)
    Private NotInheritable Class bytes_serializer_object
        Inherits bytes_serializer(Of Object)

        Private ReadOnly s As bytes_serializer(Of T)

        Public Sub New(ByVal s As bytes_serializer(Of T))
            assert(Not s Is Nothing)
            Me.s = s
        End Sub

        Protected Overrides Function append_to() As Func(Of Object, MemoryStream, Boolean)
            Return Function(ByVal i As Object, ByVal o As MemoryStream) As Boolean
                       Return s.append_to(direct_cast(Of T)(i), o)
                   End Function
        End Function

        Protected Overrides Function write_to() As Func(Of Object, MemoryStream, Boolean)
            Return Function(ByVal i As Object, ByVal o As MemoryStream) As Boolean
                       Return s.write_to(direct_cast(Of T)(i), o)
                   End Function
        End Function

        Protected Overrides Function consume_from() As _do_val_ref(Of MemoryStream, Object, Boolean)
            Return Function(ByVal i As MemoryStream, ByRef o As Object) As Boolean
                       Dim x As T = Nothing
                       If Not s.consume_from(i, x) Then
                           Return False
                       End If
                       o = x
                       Return True
                   End Function
        End Function

        Protected Overrides Function read_from() As _do_val_ref(Of MemoryStream, Object, Boolean)
            Return Function(ByVal i As MemoryStream, ByRef o As Object) As Boolean
                       Dim x As T = Nothing
                       If Not s.read_from(i, x) Then
                           Return False
                       End If
                       o = x
                       Return True
                   End Function
        End Function
    End Class
End Class