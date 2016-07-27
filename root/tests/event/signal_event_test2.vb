
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.utils
Imports osi.root.event
Imports osi.root.connector
Imports osi.root.utt

Public Class signal_event_test2
    Inherits multithreading_case_wrapper

    Private Const thread_count As Int32 = 2
    Private Const repeat_size As Int32 = 1024

    Public Sub New()
        MyBase.New(repeat(New signal_event_case(), repeat_size), thread_count)
    End Sub

    Private Class signal_event_case
        Inherits [case]

        Private Class signal_event_test_action
            Implements iaction

            Private ReadOnly before As Action
            Private ReadOnly called As atomic_int

            Public Sub New(ByVal before As action)
                assert(Not before Is Nothing)
                Me.before = before
                Me.called = New atomic_int()
            End Sub

            Public Function called_times() As Int32
                Return +called
            End Function

            Public Sub run() Implements iaction.run
                before()
                assert_equal(called.increment(), 1)
            End Sub

            Public Function valid() As Boolean Implements iaction.valid
                Return True
            End Function
        End Class

        Private ReadOnly e As vector(Of signal_event_test_action)
        Private ReadOnly se As signal_event
        Private v As singleentry

        Public Sub New()
            e = New vector(Of signal_event_test_action)()
            se = New signal_event()
        End Sub

        Private Shared Sub random_sleep()
            sleep(rnd_int(0, 2))
        End Sub

        Private Function caller() As Boolean
            If v.mark_in_use() Then
                'all the callbacks are finished
                assert_false(se.marked())
                se.mark()
            Else
                'callbacks are involving
                'assert_true(se.marked())   ' the se.unmark() may be called right before the v.release()
            End If
            random_sleep()
            Return True
        End Function

        Private Function callee() As Boolean
            Dim a As signal_event_test_action = Nothing
            a = New signal_event_test_action(Sub()
                                                 assert_true(se.marked())
                                                 assert_true(v.in_use())
                                                 If Not se.attached() Then
                                                     'the last in the event queue
                                                     se.unmark()
                                                     If assert_true(v.in_use()) Then
                                                         'avoid assert
                                                         v.release()
                                                     End If
                                                 End If
                                             End Sub)
            SyncLock e
                e.emplace_back(a)
            End SyncLock
            assert_true(se.attach(a))
            random_sleep()
            Return True
        End Function

        Public Overrides Function run() As Boolean
            assert(thread_id() = 0 OrElse thread_id() = 1)
            If thread_id() = 0 Then
                Return caller()
            Else
                Return callee()
            End If
        End Function

        Public Overrides Function prepare() As Boolean
            e.clear()
            se.clear()
            v.mark_not_in_use()
            Return MyBase.prepare()
        End Function

        Public Overrides Function finish() As Boolean
            assert_true(caller())
            assert_false(se.attached())
            assert_true(callee() And caller() And caller())
            assert_false(se.attached())
            assert_true(v.in_use())
            assert_true(se.marked())
            assert_more(e.size(), uint32_0)
            assert_equal(se.attached_count(), 0)
            assert_false(se.attached())
            For i As Int32 = 0 To e.size() - 1
                assert(Not e(i) Is Nothing)
                assert_equal(e(i).called_times(), 1)
            Next
            Return MyBase.finish()
        End Function
    End Class
End Class
