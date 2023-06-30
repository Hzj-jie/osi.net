
Option Explicit On
Option Infer Off
Option Strict On

Public Module _trace_trace
    Public ReadOnly enable_trace As Boolean =
        env_bool(env_keys("trace")) OrElse
        env_bool(env_keys("enable", "trace"))
    Public ReadOnly enable_detail_trace As Boolean =
        env_bool(env_keys("detail", "trace")) OrElse
        env_bool(env_keys("enable", "detail", "trace"))
End Module
