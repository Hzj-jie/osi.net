
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.event
Imports osi.root.formation
Imports osi.root.utt

Public NotInheritable Class count_down_event_test
    Inherits multithreading_case_wrapper

    Private Const thread_count As Int32 = 16

    Public Sub New()
        MyBase.New(New count_down_event_case(), thread_count)
    End Sub

    Private NotInheritable Class count_down_event_case
        Inherits [case]

        Private ReadOnly e As count_down_event
        Private ReadOnly w As disposer(Of ManualResetEvent)

        Public Sub New()
            e = New count_down_event(128)
            w = make_disposer(New ManualResetEvent(False))
        End Sub

        Public Overrides Function run() As Boolean
            If multithreading_case_wrapper.thread_id() = 0 Then
                For i As Int32 = 0 To 100
                    assert(w.get().force_set())
                    e.wait()
                    assert(w.get().force_reset())
                    e.reset()
                Next
            Else
                While multithreading_case_wrapper.running_thread_count() = thread_count
                    While w.get().wait(seconds_to_milliseconds(5))
                        e.set()
                    End While
                End While
            End If
            Return True
        End Function
    End Class
End Class
