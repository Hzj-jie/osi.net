
Imports osi.root.delegates

Public Interface ithreadpool
    Sub queue_job(ByVal work As Action)
    Property thread_count() As UInt32
    Function stopping() As Boolean
    Sub [stop](Optional ByVal stop_wait_seconds As Int64 = constants.default_stop_threadpool_wait_seconds)
    Function idle() As Boolean
End Interface

Public Module _ithreadpool
    Public Function in_iqless_threadpool_thread() As Boolean
        Return slimqless2_threadpool.in_iqless_threadpool_thread() OrElse
               qless_threadpool.in_iqless_threadpool_thread()
    End Function
End Module