
Imports osi.root.delegates
Imports osi.root.envs
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.constants

Partial Public Class threadpool
    Public Sub queue_job(ByVal work As Action) Implements ithreadpool.queue_job
        If stopping() Then
            If threadpool_trace Then
                raise_error("should not queue_job after stop, ", callstack())
            End If
            Return
        End If

        push_queue()
        queue_job(New work_info(work))
    End Sub

    Public Function [stop](Optional ByVal stop_wait_seconds As Int64 =
                                              default_stop_threadpool_wait_seconds) As Boolean _
                          Implements ithreadpool.stop
        If Not stoppable() Then
            Return True
        End If
        If set_stopping() Then
            'if managed_threads is nothing, means do not support stop or do not need to stop
            If Not isemptyarray(managed_threads()) Then
                Dim s As Action = Nothing
                s = Sub()
                        stop_threads(managed_threads(), stop_wait_seconds)
                    End Sub
                For i As Int64 = 0 To managed_threads().Length() - 1
                    If object_compare(managed_threads()(i), current_thread()) = 0 Then
                        queue_in_managed_threadpool(s)
                        Return True
                    End If
                Next
                s()
            End If
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Operator +(ByVal this As threadpool, ByVal that As Action) As threadpool
        assert(Not this Is Nothing)
        this.queue_job(that)
        Return this
    End Operator
End Class
