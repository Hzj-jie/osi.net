
Imports System.DateTime
Imports System.Threading
Imports osi.root.envs
Imports osi.root.formation
Imports osi.root.delegates
Imports osi.root.utils
Imports osi.root.lock
Imports osi.root.template
Imports osi.root.connector

Namespace async_result_destructor
    Public Module _async_result_destructor
        Private ReadOnly ASYNC_OPERATION_FORCE_FINISH_TIMEMS As Int64
        Private ReadOnly are As AutoResetEvent = Nothing
        Private ReadOnly q As slimqless2(Of Action) = Nothing

        Sub New()
            ASYNC_OPERATION_FORCE_FINISH_TIMEMS =
                counter.register_average_and_last_average("ASYNC_OPERATION_FORCE_FINISH_TIMEMS")
            are = New AutoResetEvent(False)
            q = New slimqless2(Of Action)()
            Dim ready_to_abort As singleentry
            ready_to_abort.mark_in_use()
            Dim th As Thread = Nothing
            th = New Thread(Sub()
                                Dim v As Action = Nothing
                                While q.pop(v) OrElse are.WaitOne()
                                    If Not v Is Nothing Then
                                        Dim n As Int64 = 0
                                        n = Now().milliseconds()
                                        Try
                                            ready_to_abort.mark_not_in_use()
                                            v()
                                        Catch ex As ThreadAbortException
                                            If application_lifetime.running() Then
                                                Thread.ResetAbort()
                                            End If
                                        Catch
                                        Finally
                                            counter.increase(ASYNC_OPERATION_FORCE_FINISH_TIMEMS,
                                                             Now().milliseconds() - n)
                                        End Try
                                    End If
                                End While
                            End Sub)
            th.IsBackground() = True
            th.Name() = "ASYNC_RESULT_DESTRUCTOR_THREAD"
            th.Start()
            application_lifetime_binder.instance.insert(
                New disposer(Of Thread)(th,
                                        disposer:=Sub(x) x.Abort()))

            stopwatch.repeat(timeslice_length_ms,
                             Sub()
                                 If ready_to_abort.mark_in_use() Then
                                     th.Abort()
                                 End If
                             End Sub)
        End Sub

        Public Sub queue(ByVal state As async_state_t, ByVal ar As IAsyncResult)
            assert(Not state Is Nothing AndAlso Not ar Is Nothing)
            queue(Sub()
                      state.mark_finish(ar, False)
                  End Sub)
        End Sub

        Public Sub queue(ByVal e As Action)
            assert(Not e Is Nothing)
            q.emplace(e)
            are.Set()
        End Sub
    End Module
End Namespace
