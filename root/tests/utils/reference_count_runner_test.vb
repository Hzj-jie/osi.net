
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template
Imports osi.root.utils
Imports osi.root.utt

Public Class reference_count_runner_test
    Inherits case_wrapper

    Public Sub New()
        MyBase.New(chained(multithreading(repeat(New multi_threading_case(), 10240), 8),
                           New auto_stop_case(),
                           New event_case()))
    End Sub

    Private Class event_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Const size As Int32 = 1000
            Dim r As reference_count_runner(Of _true, _true) = Nothing
            r = New reference_count_runner(Of _true, _true)()
            Dim started As Int32 = 0
            Dim stopped As Int32 = 0
            Dim i As Int32 = 0
            AddHandler r.after_start, Sub()
                                          assertion.equal(started, i)
                                          started += 1
                                      End Sub
            AddHandler r.after_stop, Sub()
                                         assertion.equal(stopped, i)
                                         stopped += 1
                                     End Sub
            For i = 0 To size - 1
                assertion.is_true(r.bind())
                assertion.is_true(r.release())
            Next
            assertion.equal(started, size)
            assertion.equal(stopped, size)
            Return True
        End Function
    End Class

    Private Class auto_stop_case
        Inherits [case]

        Private Class RC
            Inherits reference_count_runner(Of _true, _true)

            Public Shared v As Boolean

            Protected Overrides Sub start_process()
                v = True
            End Sub

            Protected Overrides Sub stop_process()
                v = False
            End Sub
        End Class

        Public Overrides Function run() As Boolean
            Dim r As RC = Nothing
            r = New RC()
            assertion.is_true(r.bind())
            assertion.is_true(RC.v)
            r = Nothing
            assertion.is_true(garbage_collector.waitfor_collect_when(Function() As Boolean
                                                                         Return RC.v
                                                                     End Function))
            Return True
        End Function
    End Class

    Private Class multi_threading_case
        Inherits [case]

        Private ReadOnly r As RC

        Private Class RC
            Inherits reference_count_runner

            Private v As Int32

            Protected Overrides Sub start_process()
                assertion.more(binding_count(), uint32_0)
                assertion.is_false(started())
                assertion.is_false(stopped())
                assertion.is_true(starting())
                assertion.is_false(stopping())
                assertion.equal(v, 0)
                v += 1
                Me.mark_started()
                assertion.is_true(started())
                assertion.is_false(stopped())
                assertion.is_false(starting())
                assertion.is_false(stopping())
            End Sub

            Protected Overrides Sub stop_process()
                assertion.equal(binding_count(), uint32_0)
                assertion.is_false(stopped())
                assertion.is_false(started())
                assertion.is_false(starting())
                assertion.is_true(stopping())
                assertion.equal(v, 1)
                v -= 1
                Me.mark_stopped()
                assertion.is_false(started())
                assertion.is_true(stopped())
                assertion.is_false(starting())
                assertion.is_false(stopping())
            End Sub
        End Class

        Public Sub New()
            r = New RC()
        End Sub

        Public Overrides Function run() As Boolean
            r.bind()
            r.release()
            Return True
        End Function
    End Class
End Class
