
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utt
Imports osi.root.utils

Public Class forks_test
    Inherits multithreading_case_wrapper

    Private Shared ReadOnly repeat_count As Int32

    Shared Sub New()
        repeat_count = 524288 * If(isdebugbuild(), 1, 8)
    End Sub

    Public Sub New()
        MyBase.New(repeat(New forks_case(), repeat_count), 4 * If(isdebugbuild(), 1, 4))
    End Sub

    Private Class forks_case
        Inherits [case]

        Private f As forks
        Private c As Int32

        Public Overrides Function run() As Boolean
            Const min As Int32 = 1
            Const max As Int32 = 17
            Dim i As Int32 = 0
            i = rnd_int(min, max)
            assert(i > 0)
            If f.mark_as(i) Then
                assert_equal(c, 0)
                c = i
                assert_equal(c, i)
                assert_false(f.free())
                assert_equal(c, i)
                assert_true(f.not_free())
                assert_equal(c, i)
                For j As Int32 = min To max - 1
                    assert_equal(c, i)
                    assert(j <> 0)
                    assert_equal(c, i)
                    assert_false(f.mark_as(j))
                Next
                assert_equal(c, i)
                c = 0
                assert_equal(c, 0)
                f.release()
            End If
            Return True
        End Function
    End Class
End Class
