
Option Explicit On
Option Infer Off
Option Strict On

Imports System.DateTime
Imports System.Threading
Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.utils

Public NotInheritable Class async_result_destructor
    Private Shared ReadOnly ASYNC_OPERATION_FORCE_FINISH_TIMEMS As Int64 =
        counter.register_average_and_last_average("ASYNC_OPERATION_FORCE_FINISH_TIMEMS")
    Private Shared ReadOnly are As AutoResetEvent = New AutoResetEvent(False)
    Private Shared ReadOnly q As slimqless2(Of Action) = New slimqless2(Of Action)()

    Shared Sub New()
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
                        New disposer(Of Thread)(th, disposer:=Sub(x) x.Abort()))

        stopwatch.repeat(timeslice_length_ms,
                                     Sub()
                                         If ready_to_abort.mark_in_use() Then
                                             th.Abort()
                                         End If
                                     End Sub)
    End Sub

    Public Shared Sub queue(ByVal state As async_state_t, ByVal ar As IAsyncResult)
        assert(Not state Is Nothing AndAlso Not ar Is Nothing)
        queue(Sub()
                  state.mark_finish(ar, False)
              End Sub)
    End Sub

    Public Shared Sub queue(ByVal e As Action)
        assert(Not e Is Nothing)
        q.emplace(e)
        are.Set()
    End Sub

    Private Sub New()
    End Sub
End Class
