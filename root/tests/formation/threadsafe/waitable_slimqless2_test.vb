﻿
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.utt

Public Class waitable_slimqless2_test
    Inherits multithreading_case_wrapper

    Public Sub New()
        MyBase.New(New waitable_slimqless2_case(), waitable_slimqless2_case.push_thread << 1)
    End Sub

    Private Class waitable_slimqless2_case
        Inherits [case]

        Public Const push_thread As Int32 = 2
        Private Const test_size As Int32 = 1000000
        Private ReadOnly q As waitable_slimqless2(Of Int32)
        Private ReadOnly passed As atomic_int
        Private ReadOnly finished As atomic_int

        Public Sub New()
            q = New waitable_slimqless2(Of Int32)()
            passed = New atomic_int()
            finished = New atomic_int()
        End Sub

        Public Overrides Function prepare() As Boolean
            If MyBase.prepare() Then
                q.clear()
                passed.set(0)
                finished.set(0)
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Function run() As Boolean
            If multithreading_case_wrapper.thread_id() < push_thread Then
                For i As Int32 = 0 To test_size - 1
                    Dim c As Int32 = 0
                    c = +passed
                    q.emplace(i)
                    assert_true(lazy_sleep_wait_until(Function() As Boolean
                                                          Return +passed > c
                                                      End Function,
                                                      seconds_to_milliseconds(1)))
                Next
                finished.increment()
                Return True
            Else
                While +finished < push_thread
                    assert_true(q.wait(seconds_to_milliseconds(1)) OrElse +finished = push_thread)
                    passed.increment()
                    assert_true(q.pop(Nothing))
                End While
                Return True
            End If
        End Function

        Public Overrides Function finish() As Boolean
            Dim c As Int32 = 0
            While q.pop(Nothing)
                c += 1
            End While
            ' Expect at least there is once, two concurrent emplace operations release two threads.
            assert_less(c, test_size >> 1)
            Return MyBase.finish()
        End Function
    End Class
End Class
