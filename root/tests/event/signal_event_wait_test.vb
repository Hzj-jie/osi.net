
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.event
Imports osi.root.lock
Imports osi.root.utt

Public NotInheritable Class signal_event_wait_test
    Inherits repeat_case_wrapper

    Private Const thread_count As UInt32 = 8

    Public Sub New()
        MyBase.New(multithreading(New signal_event_wait_case(), thread_count), 128)
    End Sub

    Public Overrides Function reserved_processors() As Int16
        Return 1
    End Function

    Private NotInheritable Class signal_event_wait_case
        Inherits [case]

        Private ReadOnly e As signal_event
        Private ReadOnly i As atomic_int

        Public Sub New()
            e = New signal_event(False)
            i = New atomic_int()
        End Sub

        Private Function sender() As Boolean
            If rnd_bool() Then
                sleep(rnd_int(0, 100))
            End If
            e.mark()
            assertion.is_true(timeslice_sleep_wait_when(Function() (+i) < thread_count - 1, seconds_to_milliseconds(1)))
            Return True
        End Function

        Private Function receiver() As Boolean
            e.wait(10)
            assertion.is_true(e.wait())
            i.increment()
            Return True
        End Function

        Public Overrides Function run() As Boolean
            If multithreading_case_wrapper.thread_id() = 0 Then
                Return sender()
            End If
            Return receiver()
        End Function
    End Class
End Class
