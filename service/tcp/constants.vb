
Imports osi.root.constants
Imports osi.root.envs

Namespace constants
    Public Module _constants
        Public Const bypass_tokener As String = "bypass"
        Public Const token1 As String = "1"
        Public ReadOnly use_socket As Boolean = True

        Sub New()
            use_socket = Not (env_bool(env_keys("donot", "use", "socket")) OrElse
                              env_bool(env_keys("do", "not", "use", "socket")))
        End Sub
    End Module

    Namespace default_value
        Namespace incoming
            Public Module _incoming
                Public Const max_connecting As UInt32 = max_uint32
                Public Const max_connected As UInt32 = max_uint32
                Public Const max_lifetime_ms As Int64 = 24 * 60 * 60 * second_milli
            End Module
        End Namespace

        Namespace outgoing
            Public Module _outgoing
                Public Const connecting_timeout_ms As Int64 = 10 * second_milli
                Public Const max_connecting As UInt32 = 2
                Public Const max_connected As UInt32 = 256
                Public Const max_lifetime_ms As Int64 = npos
            End Module
        End Namespace

        Public Module _default_value
            Public Const buff_size As Int32 = 4096
            Public Const response_timeout_ms As Int64 = 30 * second_milli
            Public Const send_rate_sec As UInt32 = 1
            Public Const receive_rate_sec As UInt32 = 1
            Public Const no_delay As Boolean = False
            Public Const ipv4 As Boolean = True
            Public ReadOnly connector_concurrently_connecting As Int32

            Sub New()
                If Not (env_value(env_keys("tcp", "connector", "concurrency"),
                                  connector_concurrently_connecting) OrElse
                        env_value(env_keys("tcp", "connector", "concurrently", "connecting"),
                                  connector_concurrently_connecting)) OrElse
                   connector_concurrently_connecting <= 0 Then
                    connector_concurrently_connecting = 8
                End If
            End Sub
        End Module
    End Namespace

    Namespace interval_ms
        Public Module _interval_ms
            Public ReadOnly connection_check_interval As Int64
            Public ReadOnly connector_fail As Int64
            Public ReadOnly connector_check As Int64
            Public ReadOnly accepter_over_max_connecting As Int64

            Sub New()
                If Not (env_value(env_keys("tcp", "connection", "check", "interval", "ms"),
                                  connection_check_interval) OrElse
                        env_value(env_keys("tcp", "connection", "check"),
                                  connection_check_interval)) OrElse
                   connection_check_interval <= 0 Then
                    connection_check_interval = 15 * second_milli
                End If
                If Not (env_value(env_keys("tcp", "connector", "fail", "interval", "ms"),
                                  connector_fail) OrElse
                        env_value(env_keys("tcp", "connector", "fail"),
                                  connector_fail)) OrElse
                   connector_fail <= 0 Then
                    connector_fail = 1 * second_milli
                End If
                If Not (env_value(env_keys("tcp", "connector", "check", "interval", "ms"),
                                  connector_check) OrElse
                        env_value(env_keys("tcp", "connector", "check"),
                                  connector_check)) OrElse
                   connector_check <= 0 Then
                    connector_check = 100
                End If
                If Not (env_value(env_keys("tcp", "accepter", "over", "max", "connecting", "interval", "ms"),
                                  accepter_over_max_connecting) OrElse
                        env_value(env_keys("tcp", "accepter", "over", "max", "connecting"),
                                  accepter_over_max_connecting)) OrElse
                   accepter_over_max_connecting <= 0 Then
                    accepter_over_max_connecting = 100
                End If
            End Sub
        End Module
    End Namespace
End Namespace
