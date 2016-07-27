
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.root.lock
Imports osi.root.constants
Imports osi.root.utils
Imports osi.root.envs
Imports osi.root.connector

Public Class event_comb_timeout_test
    Inherits case_wrapper

    Private Shared ReadOnly timeout_ms As Int64
    Private Shared ReadOnly test_size As Int64
    Private Shared ReadOnly thread_count As Int64

    Shared Sub New()
        test_size = 128 * If(isdebugbuild(), 1, 2)
        thread_count = 2
        timeout_ms = 1
    End Sub

    Public Sub New()
        MyBase.New(multithreading(repeat(New event_comb_timeout_case(), test_size), thread_count))
    End Sub

    Public Overrides Function run() As Boolean
        Dim r As Boolean = False
        r = MyBase.run()
        event_comb_timeout_case.wait_to_finish()
        Return r
    End Function

    Private Class event_comb_timeout_case
        Inherits [case]

        Private ReadOnly counter As atomic_int
        Private ReadOnly resumed As atomic_int
        Private ReadOnly run_times As atomic_int

        Public Sub New()
            counter = New atomic_int()
            resumed = New atomic_int()
            run_times = New atomic_int()
        End Sub

        Public Shared Sub wait_to_finish()
            'make sure the callbacks in stop_watch have been expired
            sleep(acceptable_latency_ms() << 1)
        End Sub

        Private Function create_event_comb() As event_comb
            Return New event_comb(Function() As Boolean
                                      counter.increment()
                                      Return goto_next()
                                  End Function,
                                  Function() As Boolean
                                      Return waitfor(acceptable_latency_ms() << 1) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      counter.increment()
                                      Return goto_end()
                                  End Function)
        End Function

        Private Shared Function acceptable_latency_ms() As Int64
            Return ((timeout_ms + sixteen_timeslice_length_ms) * thread_count) << 2
        End Function

        Private Function async_sync_test() As Boolean
            Dim this As Int32 = 0
            this = +counter
            Dim ec As event_comb = Nothing
            ec = create_event_comb()
            assert(Not ec Is Nothing)
            Using New auto_assert_timelimited_operation(timeout_ms, acceptable_latency_ms())
                assert_false(async_sync(ec, timeout_ms))
            End Using
            If assert_true(timeslice_sleep_wait_until(Function() ec.end(), minute_to_milliseconds(1))) Then
                assert_false(ec.end_result())
            End If
            'some event_combs may be timeouted before it's really started
            assert_more_or_equal(+counter, this)
            Return True
        End Function

        Private Function waitfor_test()
            Dim this As Int32 = 0
            this = +counter
            Dim ec As event_comb = Nothing
            ec = New event_comb(Function() As Boolean
                                    Return waitfor(create_event_comb(), timeout_ms) AndAlso
                                           goto_next()
                                End Function,
                                Function() As Boolean
                                    resumed.increment()
                                    Return goto_end()
                                End Function)
            assert_true(async_sync(ec, acceptable_latency_ms()))
            If assert_true(timeslice_sleep_wait_until(Function() ec.end(), minute_to_milliseconds(1))) Then
                assert_true(ec.end_result())
            End If
            'some event_combs may be timeouted before it's really started
            assert_more_or_equal(+counter, this)
            Return True
        End Function

        Public Overrides Function run() As Boolean
            run_times.increment()
            Return async_sync_test() AndAlso
                   waitfor_test()
        End Function

        Public Overrides Function finish() As Boolean
            assert_less_or_equal(+counter, (+run_times) * 2)
            assert_equal(+resumed, +run_times)
            Return MyBase.finish()
        End Function
    End Class
End Class
