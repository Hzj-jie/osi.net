
Public Module _network_trace
    Public ReadOnly network_trace As Boolean = False
    Public ReadOnly network_detail_trace As Boolean = False

    Sub New()
        network_trace = env_bool(env_keys("network", "trace")) OrElse
                        env_bool(env_keys("net", "trace"))
        network_detail_trace = env_bool(env_keys("network", "detail", "trace")) OrElse
                               env_bool(env_keys("net", "detail", "trace"))
    End Sub
End Module
