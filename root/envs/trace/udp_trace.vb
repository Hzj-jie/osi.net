
Public Module _udp_trace
    Public ReadOnly udp_trace As Boolean = False
    Public ReadOnly udp_detail_trace As Boolean = False

    Sub New()
        udp_trace = network_trace OrElse
                    env_bool(env_keys("udp", "trace"))
        udp_detail_trace = network_detail_trace OrElse
                           env_bool(env_keys("udp", "detail", "trace"))
    End Sub
End Module
