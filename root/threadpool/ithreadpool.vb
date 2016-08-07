
Imports osi.root.delegates
Imports osi.root.constants

Public Interface ithreadpool
    Sub queue_job(ByVal work As Action)
    Property thread_count() As UInt32
    Function stopping() As Boolean
    Sub [stop](Optional ByVal stop_wait_seconds As Int64 = constants.default_stop_threadpool_wait_seconds)
    Function idle() As Boolean
    ' Executes the next job in current thread if there is one.
    Function execute_job() As Boolean
    ' Waits for next jobs for at most ms milliseconds. If ms < 0, waits forever.
    Function wait_job(Optional ByVal ms As Int64 = npos) As Boolean
End Interface

Public Module _ithreadpool
    Public Function in_iqless_threadpool_thread() As Boolean
        Return slimqless2_threadpool.in_managed_thread() OrElse
               qless_threadpool.in_managed_thread() OrElse
               slimheapless_threadpool.in_managed_thread() OrElse
               fast_threadpool.in_managed_thread()
    End Function
End Module