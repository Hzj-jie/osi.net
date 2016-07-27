
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utt

Public Class singleentry_test
    Inherits multithreading_case_wrapper

    Public Sub New()
        MyBase.New(repeat(New singleentry_case(),
                          524288 * If(isdebugbuild(), 1, 8)),
                   4 * If(isdebugbuild(), 1, 4))
    End Sub

    Private Class singleentry_case
        Inherits [case]

        Private s As singleentry
        Private in_use As Boolean

        Public Overrides Function run() As Boolean
            If s.mark_in_use() Then
                assert_false(in_use)
                in_use = True
                assert_true(in_use)
                assert_true(s.in_use())
                assert_true(in_use)
                assert_false(s.not_in_use())
                assert_true(in_use)
                assert_false(s.mark_in_use())
                assert_true(in_use)
                in_use = False
                assert_false(in_use)
                s.release()
            End If
            Return True
        End Function
    End Class
End Class
