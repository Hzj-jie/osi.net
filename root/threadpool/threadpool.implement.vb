
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

    Public Sub [stop](Optional ByVal stop_wait_seconds As Int64 = default_stop_threadpool_wait_seconds) _
                     Implements ithreadpool.stop
        'if managed_threads is nothing, means do not support stop or do not need to stop
        If Not isemptyarray(managed_threads()) AndAlso set_stopping() Then
            Dim s As Action = Sub() stop_threads(managed_threads(), stop_wait_seconds)
            For i As Int64 = 0 To managed_threads().Length() - 1
                If object_compare(managed_threads()(i), current_thread()) = 0 Then
                    queue_in_managed_threadpool(s)
                    Return
                End If
            Next
            s()
        End If
    End Sub
End Class
