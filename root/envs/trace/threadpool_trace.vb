
Option Explicit On
Option Infer Off
Option Strict On

Public Module _threadpool_trace
    Public ReadOnly threadpool_trace As Boolean =
        perf_trace OrElse
        env_bool(env_keys("threadpool", "trace")) OrElse
        env_bool(env_keys("tp", "trace"))
End Module
