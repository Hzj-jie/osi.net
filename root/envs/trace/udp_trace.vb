
Option Explicit On
Option Infer Off
Option Strict On

Public Module _udp_trace
    Public ReadOnly udp_trace As Boolean =
        network_trace OrElse
        env_bool(env_keys("udp", "trace"))
    Public ReadOnly udp_detail_trace As Boolean =
        network_detail_trace OrElse
        env_bool(env_keys("udp", "detail", "trace"))
End Module
