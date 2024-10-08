
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.event
Imports osi.root.lock
Imports osi.root.utils
Imports osi.root.connector
Imports osi.root.utt

Public Class signal_event_test
    Inherits multithreading_case_wrapper

    Private Const thread_count As Int32 = 64
    Private Const repeat_size As Int32 = 1024

    Public Sub New()
        MyBase.New(repeat(New signal_event_case(), repeat_size), thread_count)
    End Sub

    Private Class signal_event_case
        Inherits [case]

        Private Class signal_event_test_action
            Implements iaction

            Private ReadOnly called As atomic_int

            Public Sub New()
                Me.called = New atomic_int()
            End Sub

            Public Function called_times() As Int32
                Return +called
            End Function

            Public Sub run() Implements iaction.run
                assertion.equal(called.increment(), 1)
            End Sub

            Public Function valid() As Boolean Implements iaction.valid
                Return True
            End Function
        End Class

        Private ReadOnly e As vector(Of signal_event_test_action)
        Private ReadOnly se As signal_event

        Public Sub New()
            e = New vector(Of signal_event_test_action)()
            se = New signal_event()
        End Sub

        Private Shared Sub random_sleep()
            sleep(rnd_int(0, 2))
        End Sub

        Private Function caller() As Boolean
            If rnd_bool() Then
                se.mark()
            Else
                se.unmark()
            End If
            random_sleep()
            Return True
        End Function

        Private Function callee() As Boolean
            Dim a As signal_event_test_action = Nothing
            a = New signal_event_test_action()
            SyncLock e
                e.emplace_back(a)
            End SyncLock
            assertion.is_true(se.attach(a))
            random_sleep()
            Return True
        End Function

        Public Overrides Function run() As Boolean
            assert(thread_id() >= 0 AndAlso thread_id() < thread_count)
            If thread_id() < (thread_count >> 2) Then
                Return caller()
            Else
                Return callee()
            End If
        End Function

        Public Overrides Function prepare() As Boolean
            e.clear()
            se.clear()
            Return MyBase.prepare()
        End Function

        Public Overrides Function finish() As Boolean
            se.mark()
            assertion.is_true(se.marked())
            assertion.more(e.size(), uint32_0)
            assertion.equal(se.attached_count(), 0)
            assertion.is_false(se.attached())
            For i As Int32 = 0 To e.size() - 1
                assert(Not e(i) Is Nothing)
                assertion.equal(e(i).called_times(), 1)
            Next
            e.clear()
            se.clear()
            Return MyBase.finish()
        End Function
    End Class
End Class
