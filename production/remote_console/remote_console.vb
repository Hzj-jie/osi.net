
Imports osi.root.connector
Imports osi.service.tcp

Public Module remote_console
    Public Sub main()
        Dim pp As powerpoint = Nothing
        pp = powerpoint.create(argument.token,
                               argument.host,
                               argument.port,
                               argument.ipv4,
                               argument.connecting_timeout_ms,
                               argument.send_rate_sec,
                               argument.response_timeout_ms,
                               argument.receive_rate_sec,
                               argument.max_connecting,
                               argument.max_connected,
                               argument.no_delay,
                               argument.max_lifetime_ms,
                               argument.is_outgoing,
                               argument.enable_keepalive,
                               argument.first_keepalive_ms,
                               argument.keepalive_interval_ms)
        If argument.server Then
            server.run(pp)
        Else
            client.run(pp)
        End If
        gc_trigger()
    End Sub
End Module
