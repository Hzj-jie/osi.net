
Imports osi.root.envs
Imports osi.root.utils
Imports osi.service.argument

Public Class argument
    Private Const _server As String = "server"
    Private Const _start_app As String = "start-app"

    Public Shared ReadOnly server As Boolean
    Public Shared ReadOnly is_outgoing As String
    Public Shared ReadOnly host As String
    Public Shared ReadOnly port As String
    Public Shared ReadOnly token As String
    Public Shared ReadOnly max_connecting As String
    Public Shared ReadOnly max_connected As String
    Public Shared ReadOnly no_delay As String
    Public Shared ReadOnly connecting_timeout_ms As String
    Public Shared ReadOnly response_timeout_ms As String
    Public Shared ReadOnly send_rate_sec As String
    Public Shared ReadOnly receive_rate_sec As String
    Public Shared ReadOnly max_lifetime_ms As String
    Public Shared ReadOnly ipv4 As String
    Public Shared ReadOnly enable_keepalive As String
    Public Shared ReadOnly first_keepalive_ms As String
    Public Shared ReadOnly keepalive_interval_ms As String
    Public Shared ReadOnly start_app As String

    Shared Sub New()
        var.default.bind(_server,
                         _start_app)
        server = var.default.switch(_server)
        If Not var.default.value(_start_app, start_app) AndAlso
           Not env_value("ComSpec", start_app) Then
            start_app = "cmd.exe"
        End If
#If 0 Then
        assert(var.default.parse_tcp_parameters(is_outgoing,
                                                host,
                                                port,
                                                Nothing,
                                                token,
                                                max_connecting,
                                                max_connected,
                                                no_delay,
                                                connecting_timeout_ms,
                                                response_timeout_ms,
                                                send_rate_sec,
                                                receive_rate_sec,
                                                max_lifetime_ms,
                                                ipv4,
                                                enable_keepalive,
                                                first_keepalive_ms,
                                                keepalive_interval_ms))
#End If
        If Not server Then
            max_connecting = Convert.ToString(1)
            max_connected = Convert.ToString(1)
        End If
    End Sub
End Class
