
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class invoker_value_type_test
    Private Structure test_struct
        Private ReadOnly i As Int32

        Public Sub New(ByVal i As Int32)
            Me.i = i
        End Sub

        Public Function exec() As Int32
            Return i
        End Function
    End Structure

    <test>
    Private Shared Sub post_bind_struct()
        Dim i As invoker(Of Func(Of Int32)) = Nothing
        assertion.is_true(invoker.of(Of Func(Of Int32)).
                        with_type(Of test_struct).
                        with_name("exec").
                        for_instance_public_methods().
                        build(i))
        assertion.is_true(i.post_binding())
        For j As Int32 = 0 To 10
            Dim f As Func(Of Int32) = Nothing
            assertion.is_true(i.post_bind(New test_struct(j), f))
            assertion.equal(f(), j)
        Next
    End Sub

    <test>
    Private Shared Sub post_bind_primitive_type()
        Dim i As invoker(Of Func(Of Int32)) = Nothing
        assertion.is_true(invoker.of(Of Func(Of Int32)).
                        with_type(Of Int32).
                        with_name("GetHashCode").
                        for_instance_public_methods().
                        build(i))
        assertion.is_true(i.post_binding())
        For j As Int32 = 0 To 10
            Dim f As Func(Of Int32) = Nothing
            assertion.is_true(i.post_bind(j, f))
            assertion.equal(f(), j.GetHashCode())
        Next
    End Sub

    Private Sub New()
    End Sub
End Class
