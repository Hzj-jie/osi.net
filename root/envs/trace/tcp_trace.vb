
Public Module _tcp_trace
    Public ReadOnly tcp_trace As Boolean = False
    Public ReadOnly tcp_detail_trace As Boolean = False
    Public ReadOnly disable_tcp_socket_state As Boolean = False

    Sub New()
        tcp_trace = network_trace OrElse
                    env_bool(env_keys("tcp", "trace"))
        tcp_detail_trace = network_detail_trace OrElse
                           env_bool(env_keys("tcp", "detail", "trace"))
        disable_tcp_socket_state = env_bool(env_keys("disable", "tcp", "socket", "state"))
    End Sub
End Module
