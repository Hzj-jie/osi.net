
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class invoker_unmatch_signature_test
    Private Shared Sub f1(ByVal i As Int32)
    End Sub

    Private Function f2() As UInt32
        Return 0
    End Function

    <test>
    Private Shared Sub run()
        Dim f1 As Action(Of Int32) = Nothing
        Dim f2 As Func(Of UInt32) = Nothing

        Dim i1 As invoker(Of Action(Of Int32)) = Nothing
        i1 = New invoker(Of Action(Of Int32))(GetType(invoker_unmatch_signature_test),
                                              binding_flags.instance_private_method,
                                              "f2")
        assert_true(i1.valid())
        assert_false(i1.pre_binding())
        assert_false(i1.post_binding())
        assert_false(i1.pre_bind(f1))
        assert_false(i1.post_bind(New Object(), f1))

        Dim i2 As invoker(Of Func(Of UInt32)) = Nothing
        i2 = New invoker(Of Func(Of UInt32))(GetType(invoker_unmatch_signature_test),
                                             binding_flags.static_private_method,
                                             "f1")
        assert_true(i2.valid())
        assert_false(i2.pre_binding())
        assert_false(i2.post_binding())
        assert_false(i2.pre_bind(f2))
        assert_false(i2.post_bind(New Object(), f2))
    End Sub

    Private Sub New()
    End Sub
End Class
