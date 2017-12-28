
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class type_info_test
    Private Class finalizer_class
        Public Shared ReadOnly finalizer_called As atomic_int

        Shared Sub New()
            finalizer_called = New atomic_int()
        End Sub

        Protected Overrides Sub Finalize()
            finalizer_called.increment()
            MyBase.Finalize()
        End Sub
    End Class

    Private Class no_finalizer_class
    End Class

    <test>
    Private Shared Sub finalizer_case()
        assert_true(type_info(Of finalizer_class).has_finalizer())
        assert_false(type_info(Of no_finalizer_class).has_finalizer())
        finalizer_class.finalizer_called.set(0)
        Dim f As finalizer_class = Nothing
        f = New finalizer_class()
        assert_not_nothing(type_info(Of finalizer_class).finalizer())
        type_info(Of finalizer_class).finalizer()(f)
        assert_equal(+finalizer_class.finalizer_called, 1)
        type_info(Of finalizer_class).finalizer()(f)
        assert_equal(+finalizer_class.finalizer_called, 2)
    End Sub
End Class
