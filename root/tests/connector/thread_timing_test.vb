
Imports System.Diagnostics
Imports System.Threading
Imports osi.root.connector
Imports osi.root.utt

Public Class thread_timing_test
    Inherits [case]

    Private Shared Sub assert_process_priority(ByVal ppc As ProcessPriorityClass)
        'for mono
        Try
            assertion.is_true(ppc = Process.GetCurrentProcess().PriorityClass())
        Catch
        End Try
    End Sub

    Private Shared Sub assert_thread_priority(ByVal tpc As ThreadPriority)
        assertion.is_true(tpc = Thread.CurrentThread().Priority())
    End Sub

    Public Overrides Function run() As Boolean
        Using New moderate()
            assert_process_priority(ProcessPriorityClass.Normal)
            assert_thread_priority(ThreadPriority.Normal)
            Using New realtime()
                assert_process_priority(ProcessPriorityClass.RealTime)
                assert_thread_priority(ThreadPriority.Highest)
            End Using
            assert_process_priority(ProcessPriorityClass.Normal)
            assert_thread_priority(ThreadPriority.Normal)

            Using New boost()
                assert_process_priority(ProcessPriorityClass.High)
                assert_thread_priority(ThreadPriority.Highest)
            End Using
            assert_process_priority(ProcessPriorityClass.Normal)
            assert_thread_priority(ThreadPriority.Normal)

            Using New boost()
                assert_process_priority(ProcessPriorityClass.High)
                assert_thread_priority(ThreadPriority.Highest)
                Using New moderate()
                    assert_process_priority(ProcessPriorityClass.Normal)
                    assert_thread_priority(ThreadPriority.Normal)
                End Using
                assert_process_priority(ProcessPriorityClass.High)
                assert_thread_priority(ThreadPriority.Highest)
            End Using

            Using New lazy()
                assert_process_priority(ProcessPriorityClass.Idle)
                assert_thread_priority(ThreadPriority.Lowest)
            End Using
            assert_process_priority(ProcessPriorityClass.Normal)
            assert_thread_priority(ThreadPriority.Normal)

            Using New thread_boost()
                assert_thread_priority(ThreadPriority.Highest)
            End Using
            assert_process_priority(ProcessPriorityClass.Normal)
            assert_thread_priority(ThreadPriority.Normal)

            Using New thread_boost()
                assert_thread_priority(ThreadPriority.Highest)
                Using New thread_moderate()
                    assert_thread_priority(ThreadPriority.Normal)
                End Using
                assert_thread_priority(ThreadPriority.Highest)
            End Using
            assert_process_priority(ProcessPriorityClass.Normal)
            assert_thread_priority(ThreadPriority.Normal)

            Using New thread_lazy()
                assert_thread_priority(ThreadPriority.Lowest)
            End Using
            assert_process_priority(ProcessPriorityClass.Normal)
            assert_thread_priority(ThreadPriority.Normal)
        End Using

        Return True
    End Function
End Class
