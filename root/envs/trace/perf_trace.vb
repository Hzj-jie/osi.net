
Public Module _perf_trace
    Public ReadOnly perf_trace As Boolean = False

    Sub New()
        perf_trace = env_bool(env_keys("perf", "trace"))
    End Sub
End Module
