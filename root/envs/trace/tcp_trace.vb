
Option Explicit On
Option Infer Off
Option Strict On

Public Module _tcp_trace
    Public ReadOnly tcp_trace As Boolean =
        network_trace OrElse
        env_bool(env_keys("tcp", "trace"))
    Public ReadOnly tcp_detail_trace As Boolean =
        network_detail_trace OrElse
        env_bool(env_keys("tcp", "detail", "trace"))
    Public ReadOnly disable_tcp_socket_state As Boolean = env_bool(env_keys("disable", "tcp", "socket", "state"))
End Module
