
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.utils
Imports osi.root.utt

Public Class suspend_all_process_threads_test
    Inherits isolate_case_wrapper

    Public Sub New()
        MyBase.New(New suspend_all_process_threads_case(), minute_to_milliseconds(1))
    End Sub

    Public Class suspend_all_process_threads_case
        Inherits commandline_specific_case_wrapper

        Public Sub New()
            MyBase.New(New c())
        End Sub

        Private Class c
            Inherits [case]

            Public Overrides Function reserved_processors() As Int16
                Return Environment.ProcessorCount()
            End Function

            Private Sub exec(ByVal p As pointer(Of UInt32), ByVal stopping As pointer(Of Boolean))
                assert(Not p Is Nothing)
                assert(Not stopping Is Nothing)
                While Not +stopping
                    eva(p, (+p) + 1)
                    sleep(100)
                End While
            End Sub

            Public Overrides Function run() As Boolean
                Dim p As pointer(Of UInt32) = Nothing
                Dim stopping As pointer(Of Boolean) = Nothing
                p = New pointer(Of UInt32)()
                stopping = New pointer(Of Boolean)()
                queue_in_managed_threadpool(Sub()
                                                exec(p, stopping)
                                            End Sub)
                assert_true(timeslice_sleep_wait_when(Function()
                                                          Return (+p) = 0
                                                      End Function,
                                                      seconds_to_milliseconds(1)))
                assert_true(suspend_all_current_process_threads())
                Dim c As UInt32 = 0
                c = (+p)
                assert_false(timeslice_sleep_wait_when(Function()
                                                           Return (+p) = c
                                                       End Function,
                                                       seconds_to_milliseconds(10)))
                assert_true(resume_all_current_process_threads())
                c = (+p)
                assert_true(timeslice_sleep_wait_when(Function()
                                                          Return (+p) = c
                                                      End Function,
                                                      seconds_to_milliseconds(10)))
                stopping.set(True)
                Return True
            End Function
        End Class
    End Class
End Class
