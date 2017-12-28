
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.delegates

Public Class bytes_serializer(Of T)
    Public Shared ReadOnly [default] As bytes_serializer(Of T)

    Shared Sub New()
        [default] = New bytes_serializer(Of T)()
    End Sub

    Public Shared Sub register(ByVal sizeof As Func(Of T, UInt64))
        binder(Of Func(Of T, UInt64), sizeof_binder_protector).set_global(sizeof)
    End Sub

    Public Shared Sub register(ByVal to_bytes As _do_val_ref(Of T, Byte(), Boolean))
        binder(Of _do_val_ref(Of T, Byte(), Boolean), bytes_conversion_binder_protector).set_global(to_bytes)
    End Sub

    Public Shared Sub register(ByVal from_bytes As _do_val_ref(Of Byte(), T, Boolean))
        binder(Of _do_val_ref(Of Byte(), T, Boolean), bytes_conversion_binder_protector).set_global(from_bytes)
    End Sub

    Public Overridable Function sizeof(ByVal i As T) As UInt64
        assert(binder(Of Func(Of T, UInt64), sizeof_binder_protector).has_global())
        Return binder(Of Func(Of T, UInt64), sizeof_binder_protector).global()(i)
    End Function

    Public Overridable Function to_bytes(ByVal i As T, ByRef o() As Byte) As Boolean
        assert(binder(Of _do_val_ref(Of T, Byte(), Boolean), bytes_conversion_binder_protector).has_global())
        Return binder(Of _do_val_ref(Of T, Byte(), Boolean), bytes_conversion_binder_protector).global()(i, o)
    End Function

    Public Overridable Function from_bytes(ByVal i() As Byte, ByRef o As T) As Boolean
        assert(binder(Of _do_val_ref(Of Byte(), T, Boolean), bytes_conversion_binder_protector).has_global())
        Return binder(Of _do_val_ref(Of Byte(), T, Boolean), bytes_conversion_binder_protector).global()(i, o)
    End Function

    Public Shared Operator +(ByVal this As bytes_serializer(Of T)) As bytes_serializer(Of T)
        If this Is Nothing Then
            Return [default]
        Else
            Return this
        End If
    End Operator

    Protected Sub New()
    End Sub
End Class
