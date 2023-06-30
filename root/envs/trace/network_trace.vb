
Option Explicit On
Option Infer Off
Option Strict On

Public Module _network_trace
    Public ReadOnly network_trace As Boolean =
        env_bool(env_keys("network", "trace")) OrElse
        env_bool(env_keys("net", "trace"))
    Public ReadOnly network_detail_trace As Boolean =
        env_bool(env_keys("network", "detail", "trace")) OrElse
        env_bool(env_keys("net", "detail", "trace"))
End Module
