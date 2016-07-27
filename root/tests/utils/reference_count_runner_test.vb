
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.template

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
                                          assert_equal(started, i)
                                          started += 1
                                      End Sub
            AddHandler r.after_stop, Sub()
                                         assert_equal(stopped, i)
                                         stopped += 1
                                     End Sub
            For i = 0 To size - 1
                assert_true(r.bind())
                assert_true(r.release())
            Next
            assert_equal(started, size)
            assert_equal(stopped, size)
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
            assert_true(r.bind())
            assert_true(RC.v)
            r = Nothing
            assert_true(waitfor_gc_collect_when(Function() As Boolean
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
                assert_more(binding_count(), 0)
                assert_false(started())
                assert_true(stopped())
                assert_true(starting())
                assert_false(stopping())
                assert_equal(v, 0)
                v += 1
                Me.mark_started()
                assert_true(started())
            End Sub

            Protected Overrides Sub stop_process()
                assert_equal(binding_count(), 0)
                assert_false(stopped())
                assert_true(started())
                assert_false(starting())
                assert_true(stopping())
                assert_equal(v, 1)
                v -= 1
                Me.mark_stopped()
                assert_true(stopped())
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
