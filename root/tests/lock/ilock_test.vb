
Imports osi.root.lock
Imports osi.root.utt

Public Class ilock_test(Of T As {ilock, Structure})
    Inherits islimlock_test(Of T)

    Protected Sub New(Optional ByVal small_size As Boolean = False)
        MyBase.New(New ilock_case(), small_size)
    End Sub

    Protected Class ilock_case
        Inherits islimlock_case

        Protected Overrides Sub before_wait(ByRef l As T)
            MyBase.before_wait(l)
            assert_false(l.held_in_thread())
        End Sub

        Protected Overrides Sub after_wait(ByRef l As T)
            MyBase.after_wait(l)
            assert_true(l.held())
            assert_true(l.held_in_thread())
        End Sub

        Protected Overrides Sub before_release(ByRef l As T)
            MyBase.before_release(l)
            assert_true(l.held())
            assert_true(l.held_in_thread())
        End Sub

        Protected Overrides Sub after_release(ByRef l As T)
            MyBase.after_release(l)
            assert_false(l.held_in_thread())
        End Sub
    End Class
End Class
