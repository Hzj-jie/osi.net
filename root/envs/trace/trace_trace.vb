
Public Module _trace_trace
    Public ReadOnly enable_trace As Boolean = False
    Public ReadOnly enable_detail_trace As Boolean = False

    Sub New()
        enable_trace = env_bool(env_keys("trace")) OrElse
                       env_bool(env_keys("enable", "trace"))
        enable_detail_trace = env_bool(env_keys("detail", "trace")) OrElse
                              env_bool(env_keys("enable", "detail", "trace"))
    End Sub
End Module
