
Public Module _threadpool_trace
    Public ReadOnly threadpool_trace As Boolean = False

    Sub New()
        threadpool_trace = perf_trace OrElse
                           env_bool(env_keys("threadpool", "trace")) OrElse
                           env_bool(env_keys("tp", "trace"))
    End Sub
End Module
