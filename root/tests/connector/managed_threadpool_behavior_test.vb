
Imports System.Threading
Imports osi.root.connector
Imports osi.root.event
Imports osi.root.utt

Public Class managed_threadpool_behavior_test
    Inherits [case]

    Private Shared Function queue_in_background_io_thread_case() As Boolean
        Const size As Int32 = 1024
        Dim finished As count_down_event = Nothing
        finished = New count_down_event(size)
        For i As Int32 = 0 To size - 1
            queue_in_managed_threadpool(Sub()
                                            assert_true(Thread.CurrentThread().IsThreadPoolThread())
                                            assert_true(Thread.CurrentThread().IsBackground())
                                            finished.set()
                                        End Sub)
        Next
        finished.wait()
        Return True
    End Function

    Private Shared Function new_in_worker_thread_case() As Boolean
        Dim finished As AutoResetEvent = Nothing
        finished = New AutoResetEvent(False)
        For i As Int32 = 0 To 1024 - 1
            assert(start_thread(Nothing,
                                Sub()
                                    assert_false(Thread.CurrentThread().IsBackground())
                                    assert(finished.force_set())
                                End Sub))
            assert(finished.wait())
        Next
        finished.Close()
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return queue_in_background_io_thread_case() AndAlso
               new_in_worker_thread_case()
    End Function
End Class
