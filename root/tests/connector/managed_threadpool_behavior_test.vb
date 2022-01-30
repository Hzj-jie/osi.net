
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.event
Imports osi.root.utt

Public NotInheritable Class managed_threadpool_behavior_test
    Inherits [case]

    Private Shared Function queue_in_background_io_thread_case() As Boolean
        Const size As Int32 = 1024
        Dim finished As New count_down_event(size)
        For i As Int32 = 0 To size - 1
            managed_thread_pool.push(Sub()
                                         assertion.is_true(Thread.CurrentThread().IsThreadPoolThread())
                                         assertion.is_true(Thread.CurrentThread().IsBackground())
                                         finished.set()
                                     End Sub)
        Next
        finished.wait()
        Return True
    End Function

    Private Shared Function new_in_worker_thread_case() As Boolean
        Dim finished As New AutoResetEvent(False)
        For i As Int32 = 0 To 1024 - 1
            Dim t As New Thread(Sub()
                                    assertion.is_false(Thread.CurrentThread().IsBackground())
                                    assert(finished.force_set())
                                End Sub)
            t.Start()
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
