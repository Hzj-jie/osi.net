
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.event
Imports osi.root.lock
Imports osi.root.utt

Public Class count_reset_event_basic_test
    Inherits multithreading_case_wrapper

    Public Sub New()
        MyBase.New(repeat(New executor(), 1024 * 1024), 8)
    End Sub

    Private Class executor
        Inherits [case]

        Private ReadOnly e As count_reset_event
        Private ReadOnly set_times As atomic_int
        Private ReadOnly release_times As atomic_int

        Public Sub New()
            e = New count_reset_event()
            set_times = New atomic_int()
            release_times = New atomic_int()
        End Sub

        Public Overrides Function prepare() As Boolean
            If MyBase.prepare() Then
                set_times.set(0)
                release_times.set(0)
                e.reset()
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Function run() As Boolean
            If rnd_bool() OrElse multithreading_case_wrapper.thread_id() = 0 Then
                set_times.increment()
                e.set()
            Else
                e.wait()
                release_times.increment()
            End If
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            While e.wait(int64_0)
                release_times.increment()
            End While
            assert_equal(+release_times, +set_times)
            Return MyBase.finish()
        End Function
    End Class
End Class

Public Class count_reset_event_concurrently_release_test
    Inherits multithreading_case_wrapper

    Private Const test_size As Int64 = 1024

    Public Sub New()
        MyBase.New(repeat(New executor(), test_size), 8)
    End Sub

    Private Class executor
        Inherits [case]

        <ThreadStatic> Private Shared round As Int64
        Private ReadOnly e As count_reset_event
        Private ReadOnly pending As atomic_int
        Private this_round As Int64

        Public Sub New()
            e = New count_reset_event()
            pending = New atomic_int()
        End Sub

        Public Overrides Function run() As Boolean
            round += int64_1
            If multithreading_case_wrapper.thread_id() = 0 Then
                atomic.eva(this_round, round)
                lazy_wait_when(Function() +pending < multithreading_case_wrapper.running_thread_count() - 1)
                For i As Int32 = 0 To CInt(multithreading_case_wrapper.running_thread_count()) - 2
                    e.set()
                Next
                assert_true(lazy_sleep_wait_when(Function() +pending > 0, seconds_to_milliseconds(1)))
            Else
                lazy_wait_when(Function() atomic.read(this_round) < round)
                pending.increment()
                e.wait()
                pending.decrement()
            End If
            Return True
        End Function
    End Class
End Class

Public Class count_reset_event_timed_wait_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        Dim clock As mock_tick_clock = Nothing
        clock = New mock_tick_clock(milliseconds_to_ticks(uint64_1))
        Using thread_static_mock_tick_clock.scoped_register(clock)
            Using e As count_reset_event = New count_reset_event()
                assert_false(e.wait(1))
                e.set()
                assert_true(e.wait(1))
            End Using
        End Using
        Return True
    End Function
End Class
