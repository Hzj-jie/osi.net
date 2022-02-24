
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.utt

Public NotInheritable Class callback_timeout_test
    Inherits case_wrapper

    Private Shared ReadOnly timeout_ms As Int64 = 1
    Private Shared ReadOnly test_size As Int64 = 1024
    Private Shared ReadOnly thread_count As UInt32 = CUInt(2 * Environment.ProcessorCount())

    Public Sub New()
        MyBase.New(multithreading(repeat(New callback_timeout_case(), test_size), thread_count))
    End Sub

    Private NotInheritable Class callback_timeout_case
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
            Dim this As Int32 = +counter
            Dim cb As callback_action = create_callback_action()
            assert(Not cb Is Nothing)
            Using New boost()
                Using expectation.timelimited_operation(timeout_ms,
                                                        (timeout_ms + 4 * timeslice_length_ms) * thread_count)
                    assertion.is_false(async_sync(cb, timeout_ms))
                End Using
            End Using
            assertion.is_true(timeslice_sleep_wait_until(Function() cb.finished(), minutes_to_milliseconds(1)))

            assertion.is_false(cb.begin_result().unknown_())
            assertion.is_true(cb.begin_result().true_())

            assertion.is_false(cb.end_result().unknown_())
            assertion.is_true(cb.end_result().true_())

            assertion.is_true(cb.check_result().false_())
            assertion.is_false(cb.check_result().true_())
            assertion.is_false(cb.check_result().unknown_())

            assertion.more(+counter, this + 1)
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            assertion.equal(+counter, (+run_times) << 1)
            Return MyBase.finish()
        End Function
    End Class
End Class
