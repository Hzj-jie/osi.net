
Option Explicit On
Option Infer Off
Option Strict On

Public Module _perf_trace
    Public ReadOnly perf_trace As Boolean = env_bool(env_keys("perf", "trace"))
End Module
