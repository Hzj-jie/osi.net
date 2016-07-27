
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utt
Imports osi.root.utils

Public Class duallock_normal_write_perf_test
    Inherits duallock_perf_test

    Public Sub New()
        MyBase.New(3)
    End Sub
End Class

Public Class duallock_less_write_perf_test
    Inherits duallock_perf_test

    Public Sub New()
        MyBase.New(6)
    End Sub
End Class

Public Class duallock_heavy_write_perf_test
    Inherits duallock_perf_test

    Public Sub New()
        MyBase.New(1)
    End Sub
End Class

Public Class duallock_seldom_write_perf_test
    Inherits duallock_perf_test

    Public Sub New()
        MyBase.New(10)
    End Sub
End Class

Public MustInherit Class duallock_perf_test
    Inherits performance_case_wrapper

    Protected Sub New(ByVal bool_true_times As Int32)
        'should be consistent with islimlock_test
        MyBase.New(multithreading(repeat(New duallock_perf_case(bool_true_times),
                                         524288 * If(isdebugbuild(), 1, 8)),
                                  4 * If(isdebugbuild(), 1, 4)))
    End Sub

    Private Class duallock_perf_case
        Inherits [case]

        Private ReadOnly bool_true_times As Int32
        Private dl As duallock
        Private x As Int32

        Public Sub New(ByVal bool_true_times As Int32)
            assert(bool_true_times > 0)
            Me.bool_true_times = bool_true_times
        End Sub

        Public Overrides Function run() As Boolean
            If rnd_bool_trues(bool_true_times) Then
                dl.writer_wait()
                x += 1
                dl.writer_release()
            Else
                dl.reader_wait()
                Dim y As Int32 = 0
                y = x
                dl.reader_release()
            End If
            Return True
        End Function
    End Class
End Class
