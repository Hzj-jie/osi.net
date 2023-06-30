
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
                assertion.equal(c, 0)
                c = i
                assertion.equal(c, i)
                assertion.is_false(f.free())
                assertion.equal(c, i)
                assertion.is_true(f.not_free())
                assertion.equal(c, i)
                For j As Int32 = min To max - 1
                    assertion.equal(c, i)
                    assert(j <> 0)
                    assertion.equal(c, i)
                    assertion.is_false(f.mark_as(j))
                Next
                assertion.equal(c, i)
                c = 0
                assertion.equal(c, 0)
                f.release()
            End If
            Return True
        End Function
    End Class
End Class
