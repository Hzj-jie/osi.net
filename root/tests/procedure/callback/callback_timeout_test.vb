
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utils
Imports osi.root.envs

Public Class callback_timeout_test
    Inherits case_wrapper

    Private Shared ReadOnly timeout_ms As Int64
    Private Shared ReadOnly test_size As Int64
    Private Shared ReadOnly thread_count As Int64

    Shared Sub New()
        test_size = 1024 * If(isdebugbuild(), 1, 8)
        thread_count = 8 * If(isdebugbuild(), 1, 4)
        timeout_ms = 1
    End Sub

    Public Sub New()
        MyBase.New(multithreading(repeat(New callback_timeout_case(), test_size), thread_count))
    End Sub

    Private Class callback_timeout_case
        Inherits [case]

        Private ReadOnly counter As atomic_int
        Private ReadOnly run_times As atomic_int

        Public Sub New()
            counter = New atomic_int()
            run_times = New atomic_int()
        End Sub

        Private Function create_callback_action() As callback_action
            Return New callback_action(Function() As Boolean
                                           counter.increment()
                                           Return True
                                       End Function,
                                       Function() As Boolean
                                           Return callback_action.check_stay
                                       End Function,
                                       Function() As Boolean
                                           counter.increment()
                                           Return True
                                       End Function)
        End Function

        Public Overrides Function run() As Boolean
            run_times.increment()
            Dim this As Int32 = 0
            this = +counter
            Dim cb As callback_action = Nothing
            cb = create_callback_action()
            assert(Not cb Is Nothing)
            Using New auto_assert_timelimited_operation(timeout_ms,
                                                        (timeout_ms + four_timeslice_length_ms) * thread_count)
                assert_false(async_sync(cb, timeout_ms))
            End Using
            assert_true(timeslice_sleep_wait_until(Function() cb.finished(), minutes_to_milliseconds(1)))

            assert_false(cb.begin_result().unknown_())
            assert_true(cb.begin_result().true_())

            assert_false(cb.end_result().unknown_())
            assert_true(cb.end_result().true_())

            assert_true(cb.check_result().false_())
            assert_false(cb.check_result().true_())
            assert_false(cb.check_result().unknown_())

            assert_more(+counter, this + 1)
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            assert_equal(+counter, (+run_times) << 1)
            Return MyBase.finish()
        End Function
    End Class
End Class
