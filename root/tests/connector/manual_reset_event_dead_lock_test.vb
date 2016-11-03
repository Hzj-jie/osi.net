﻿
Imports System.Threading
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utt

Public Class manual_reset_event_dead_lock_test
    Inherits commandline_specific_case_wrapper

    Public Sub New()
        MyBase.New(New manual_reset_event_dead_lock_case())
    End Sub

    Private Class manual_reset_event_dead_lock_case
        Inherits multithreading_case_wrapper

        Private Const thread_count As Int32 = 8

        Public Sub New()
            MyBase.New(New executor(), thread_count)
        End Sub

        Private Class executor
            Inherits [case]

            Private ReadOnly stage1 As ManualResetEvent
            Private ReadOnly stage2 As ManualResetEvent
            Private ReadOnly passed1 As atomic_int
            Private ReadOnly passed2 As atomic_int

            Public Sub New()
                stage1 = New ManualResetEvent(False)
                stage2 = New ManualResetEvent(False)
                passed1 = New atomic_int()
                passed2 = New atomic_int()
            End Sub

            Public Overrides Function preserved_processors() As Int16
                Return 1
            End Function

            Protected Overrides Sub Finalize()
                stage1.Close()
                stage2.Close()
                MyBase.Finalize()
            End Sub

            Public Overrides Function run() As Boolean
                If multithreading_case_wrapper.thread_id() = 0 Then
                    For i As Int32 = min_int32 To max_int32 - 1
                        passed1.set(0)
                        assert(stage1.force_set())
                        assert_true(timeslice_sleep_wait_until(Function() +passed1 = thread_count - 1,
                                                               seconds_to_milliseconds(10)))
                        assert(stage1.force_reset())
                        assert_equal(+passed1, thread_count - 1)
                        passed2.set(0)
                        assert(stage2.force_set())
                        assert_equal(+passed1, thread_count - 1)
                        assert_true(timeslice_sleep_wait_until(Function() +passed2 = thread_count - 1,
                                                               seconds_to_milliseconds(10)))
                        assert(stage2.force_reset())
                        assert_equal(+passed1, thread_count - 1)
                        assert_equal(+passed2, thread_count - 1)
                    Next
                    Return True
                Else
                    While multithreading_case_wrapper.running_thread_count() = thread_count
                        assert(stage1.wait())
                        passed1.increment()
                        assert(stage2.wait())
                        passed2.increment()
                    End While
                    Return True
                End If
            End Function
        End Class
    End Class
    End Class
