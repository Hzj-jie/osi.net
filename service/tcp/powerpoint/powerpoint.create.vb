
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.argument
Imports osi.service.tcp.constants

Partial Public Class powerpoint
    Public Shared Function create(ByVal v As var, ByRef o As powerpoint) As Boolean
        Dim c As creator = Nothing
        Return creator.from_var(v, c) AndAlso
               assert(c.create(o))
    End Function

    Public Shared Function create(ByVal v As var) As powerpoint
        Dim o As powerpoint = Nothing
        assert(create(v, o))
        Return o
    End Function

    Public Class creator
        Private token As String
        Private host_or_ip As String
        Private port As UInt16
        Private ipv4 As Boolean
        Private connecting_timeout_ms As Int64
        Private send_rate_sec As UInt32
        Private response_timeout_ms As Int64
        Private receive_rate_sec As UInt32
        Private max_connecting As UInt32
        Private max_connected As UInt32
        Private no_delay As Boolean
        Private max_lifetime_ms As Int64
        Private is_outgoing As Boolean
        Private enable_keepalive As Boolean
        Private first_keepalive_ms As UInt32
        Private keepalive_interval_ms As UInt32
        Private tokener As String
        Private delay_connect As Boolean

        Shared Sub New()
            assert(IPEndPoint.MinPort >= 0 AndAlso IPEndPoint.MaxPort <= max_uint16)
        End Sub

        Public Sub New()
            Me.without_token().
               without_host_or_ip().
               without_port().
               without_ipv4().
               without_connecting_timeout_ms().
               without_send_rate_sec().
               without_response_timeout_ms().
               without_receive_rate_sec().
               without_max_connecting().
               without_max_connected().
               without_no_delay().
               without_max_lifetime_ms().
               without_is_outgoing().
               without_enable_keepalive().
               without_first_keepalive_ms().
               without_keepalive_interval_ms().
               without_tokener().
               without_delay_connect()
        End Sub

        Public Shared Function [New]() As creator
            Return New creator()
        End Function

        Public Shared Function from_var(ByVal v As var, ByRef o As creator) As Boolean
            If v Is Nothing Then
                Return False
            Else
                o = creator.[New]().with_var(v)
                Return o.valid()
            End If
        End Function

        Public Function valid() As Boolean
            Return max_connecting > 0 AndAlso
                   (max_connecting <= max_connected OrElse max_connected = 0) AndAlso
                   (is_outgoing Xor host_or_ip.null_or_empty()) AndAlso
                   port <> socket_invalid_port
        End Function

        Public Function with_token(ByVal token As String) As creator
            Me.token = token
            Return Me
        End Function

        Public Function without_token() As creator
            Return with_token(Nothing)
        End Function

        Public Function with_host_or_ip(ByVal host_or_ip As String) As creator
            Me.host_or_ip = host_or_ip
            is_outgoing = Not host_or_ip.null_or_empty()
            Return Me
        End Function

        Public Function without_host_or_ip() As creator
            Return with_host_or_ip(Nothing)
        End Function

        Public Function with_port(ByVal port As UInt16) As creator
            Me.port = port
            Return Me
        End Function

        Public Function with_port_str(ByVal port As String) As creator
            Return with_port(port.to(Of UInt16)())
        End Function

        Public Function without_port() As creator
            Return with_port(uint16_0)
        End Function

        Public Function with_endpoint(ByVal ep As IPEndPoint) As creator
            If ep Is Nothing Then
                Return without_host_or_ip().without_port()
            Else
                If ep.Address().Equals(IPAddress.Any) OrElse
                   ep.Address().Equals(IPAddress.IPv6Any) Then
                    without_host_or_ip()
                Else
                    with_host_or_ip(Convert.ToString(ep.Address()))
                End If
                Return with_port(CUShort(ep.Port()))
            End If
        End Function

        Public Function with_ipv4() As creator
            Me.ipv4 = True
            Return Me
        End Function

        Public Function with_ipv4_str(ByVal ipv4 As String) As creator
            If ipv4.to(Of Boolean)(default_value.ipv4) Then
                Return with_ipv4()
            Else
                Return with_ipv6()
            End If
        End Function

        Public Function with_ipv6() As creator
            Me.ipv4 = False
            Return Me
        End Function

        Public Function without_ipv4() As creator
            Me.ipv4 = default_value.ipv4
            Return Me
        End Function

        Public Function with_connecting_timeout_ms(ByVal ms As Int64) As creator
            Me.connecting_timeout_ms = ms
            Return Me
        End Function

        Public Function with_connecting_timeout_ms_str(ByVal ms As String) As creator
            Return with_connecting_timeout_ms(ms.to(Of Int64)(default_value.outgoing.connecting_timeout_ms))
        End Function

        Public Function without_connecting_timeout_ms() As creator
            Return with_connecting_timeout_ms(default_value.outgoing.connecting_timeout_ms)
        End Function

        Public Function with_send_rate_sec(ByVal rate_sec As UInt32) As creator
            Me.send_rate_sec = rate_sec
            Return Me
        End Function

        Public Function with_send_rate_sec_str(ByVal rate_sec As String) As creator
            Return with_send_rate_sec(rate_sec.to(Of UInt32)(default_value.send_rate_sec))
        End Function

        Public Function without_send_rate_sec() As creator
            Return with_send_rate_sec(default_value.send_rate_sec)
        End Function

        Public Function with_response_timeout_ms(ByVal ms As Int64) As creator
            Me.response_timeout_ms = ms
            Return Me
        End Function

        Public Function with_response_timeout_ms_str(ByVal ms As String) As creator
            Return with_response_timeout_ms(ms.to(Of Int64)(default_value.response_timeout_ms))
        End Function

        Public Function without_response_timeout_ms() As creator
            Return with_response_timeout_ms(default_value.response_timeout_ms)
        End Function

        Public Function with_receive_rate_sec(ByVal rate_sec As UInt32) As creator
            Me.receive_rate_sec = rate_sec
            Return Me
        End Function

        Public Function with_receive_rate_sec_str(ByVal rate_sec As String) As creator
            Return with_receive_rate_sec(rate_sec.to(Of UInt32)(default_value.receive_rate_sec))
        End Function

        Public Function without_receive_rate_sec() As creator
            Return with_receive_rate_sec(default_value.receive_rate_sec)
        End Function

        Public Function with_max_connecting(ByVal max_connecting As UInt32) As creator
            Me.max_connecting = max_connecting
            Return Me
        End Function

        Public Function with_max_connecting_str(ByVal max_connecting As String) As creator
            Return with_max_connecting(max_connecting.to(Of UInt32)(If(is_outgoing,
                                                                   default_value.outgoing.max_connecting,
                                                                   default_value.incoming.max_connecting)))
        End Function

        Public Function without_max_connecting() As creator
            Return with_max_connecting(If(is_outgoing,
                                          default_value.outgoing.max_connecting,
                                          default_value.incoming.max_connecting))
        End Function

        Public Function with_max_connected(ByVal max_connected As UInt32) As creator
            Me.max_connected = max_connected
            Return Me
        End Function

        Public Function with_max_connected_str(ByVal max_connected As String) As creator
            Return with_max_connected(max_connected.to(Of UInt32)(If(is_outgoing,
                                                                 default_value.outgoing.max_connected,
                                                                 default_value.incoming.max_connected)))
        End Function

        Public Function without_max_connected() As creator
            Return with_max_connected(If(is_outgoing,
                                         default_value.outgoing.max_connected,
                                         default_value.incoming.max_connected))
        End Function

        Public Function with_no_delay(ByVal no_delay As Boolean) As creator
            Me.no_delay = no_delay
            Return Me
        End Function

        Public Function with_no_delay_str(ByVal no_delay As String) As creator
            Return with_no_delay(no_delay.to(Of Boolean)(default_value.no_delay))
        End Function

        Public Function without_no_delay() As creator
            Return with_no_delay(default_value.no_delay)
        End Function

        Public Function with_max_lifetime_ms(ByVal max_lifetime_ms As Int64) As creator
            Me.max_lifetime_ms = max_lifetime_ms
            Return Me
        End Function

        Public Function with_max_lifetime_ms_str(ByVal max_lifetime_ms As String) As creator
            Return with_max_lifetime_ms(max_lifetime_ms.to(Of Int64)(If(is_outgoing,
                                                                    default_value.outgoing.max_lifetime_ms,
                                                                    default_value.incoming.max_lifetime_ms)))
        End Function

        Public Function without_max_lifetime_ms() As creator
            Return with_max_lifetime_ms(If(is_outgoing,
                                           default_value.outgoing.max_lifetime_ms,
                                           default_value.incoming.max_lifetime_ms))
        End Function

        Public Function with_outgoing() As creator
            Me.is_outgoing = True
            Return Me
        End Function

        Public Function with_incoming() As creator
            Me.is_outgoing = False
            Return Me
        End Function

        Public Function with_is_outgoing_str(ByVal is_outgoing As String) As creator
            If is_outgoing.to(Of Boolean)(Not host_or_ip.null_or_empty()) Then
                Return with_outgoing()
            Else
                Return with_incoming()
            End If
        End Function

        Public Function without_is_outgoing() As creator
            If Not host_or_ip.null_or_empty() Then
                Return with_outgoing()
            Else
                Return with_incoming()
            End If
        End Function

        Public Function with_enable_keepalive(ByVal enable_keepalive As Boolean) As creator
            Me.enable_keepalive = enable_keepalive
            Return Me
        End Function

        Public Function with_enable_keepalive_str(ByVal enable_keepalive As String) As creator
            Return with_enable_keepalive(enable_keepalive.to(Of Boolean)(enable_socket_keepalive))
        End Function

        Public Function without_enable_keepalive() As creator
            Return with_enable_keepalive(enable_socket_keepalive)
        End Function

        Public Function with_first_keepalive_ms(ByVal ms As UInt32) As creator
            Me.first_keepalive_ms = ms
            Return Me
        End Function

        Public Function with_first_keepalive_ms_str(ByVal ms As String) As creator
            Return with_first_keepalive_ms(ms.to(Of UInt32)(socket_first_keepalive_ms))
        End Function

        Public Function without_first_keepalive_ms() As creator
            Return with_first_keepalive_ms(socket_first_keepalive_ms)
        End Function

        Public Function with_keepalive_interval_ms(ByVal ms As UInt32) As creator
            Me.keepalive_interval_ms = ms
            Return Me
        End Function

        Public Function with_keepalive_interval_ms_str(ByVal ms As String) As creator
            Return with_keepalive_interval_ms(ms.to(Of UInt32)(socket_keepalive_interval_ms))
        End Function

        Public Function without_keepalive_interval_ms() As creator
            Return with_keepalive_interval_ms(socket_keepalive_interval_ms)
        End Function

        Public Function with_tokener(ByVal tokener As String) As creator
            Me.tokener = tokener
            Return Me
        End Function

        Public Function with_token1() As creator
            Return with_tokener(constants.token1)
        End Function

        Public Function with_bypass_tokener() As creator
            Return with_tokener(constants.bypass_tokener)
        End Function

        Public Function without_tokener() As creator
            Return with_tokener(Nothing)
        End Function

        Public Function with_delay_connect(ByVal delay_connect As Boolean) As creator
            Me.delay_connect = delay_connect
            Return Me
        End Function

        Public Function with_delay_connect_str(ByVal delay_connect As String) As creator
            Return with_delay_connect(delay_connect.to(Of Boolean)(default_value.delay_connect))
        End Function

        Public Function without_delay_connect() As creator
            Return with_delay_connect(default_value.delay_connect)
        End Function

        Public Function with_var(ByVal v As var) As creator
            Const p_is_outgoing As String = "is-outgoing"
            Const p_host As String = "host"
            Const p_port As String = "port"
            Const p_token As String = "token"
            Const p_max_connecting As String = "max-connecting"
            Const p_max_connected As String = "max-connected"
            Const p_no_delay As String = "no-delay"
            Const p_connecting_timeout_ms As String = "connecting-timeout-ms"
            Const p_response_timeout_ms As String = "response-timeout-ms"
            Const p_send_rate_sec As String = "send-rate-sec"
            Const p_receive_rate_sec As String = "receive-rate-sec"
            Const p_max_lifetime_ms As String = "max-lifetime-ms"
            Const p_ipv4 As String = "ipv4"
            Const p_enable_keepalive As String = "enable-keepalive"
            Const p_first_keepalive_ms As String = "first-keepalive-ms"
            Const p_keepalive_interval_ms As String = "keepalive-interval-ms"
            Const p_tokener As String = "tokener"
            Const p_delay_connect As String = "delay-connect"
            assert(Not v Is Nothing)
            v.bind(p_is_outgoing,
                   p_host,
                   p_port,
                   p_token,
                   p_max_connecting,
                   p_max_connected,
                   p_no_delay,
                   p_connecting_timeout_ms,
                   p_response_timeout_ms,
                   p_send_rate_sec,
                   p_receive_rate_sec,
                   p_max_lifetime_ms,
                   p_ipv4,
                   p_enable_keepalive,
                   p_first_keepalive_ms,
                   p_keepalive_interval_ms,
                   p_tokener,
                   p_delay_connect)
            ' Several other fields depend on is_outgoing, set is_outgoing_str() first.
            Return with_host_or_ip(v(p_host)).
                   with_is_outgoing_str(v(p_is_outgoing)).
                   with_token(v(p_token)).
                   with_port_str(v(p_port)).
                   with_ipv4_str(v(p_ipv4)).
                   with_connecting_timeout_ms_str(v(p_connecting_timeout_ms)).
                   with_send_rate_sec_str(v(p_send_rate_sec)).
                   with_response_timeout_ms_str(v(p_response_timeout_ms)).
                   with_receive_rate_sec_str(v(p_receive_rate_sec)).
                   with_max_connecting_str(v(p_max_connecting)).
                   with_max_connected_str(v(p_max_connected)).
                   with_no_delay_str(v(p_no_delay)).
                   with_max_lifetime_ms_str(v(p_max_lifetime_ms)).
                   with_enable_keepalive_str(v(p_enable_keepalive)).
                   with_first_keepalive_ms_str(v(p_first_keepalive_ms)).
                   with_keepalive_interval_ms_str(v(p_keepalive_interval_ms)).
                   with_tokener(v(p_tokener)).
                   with_delay_connect_str(v(p_delay_connect))
        End Function

        Public Function create(ByRef o As powerpoint) As Boolean
            If max_connecting = 0 Then
                max_connecting = If(is_outgoing, uint32_1, max_uint32)
            End If
            If max_connecting > max_connected AndAlso max_connected > 0 Then
                max_connecting = max_connected
            End If
            If valid() Then
                o = New powerpoint(token,
                                   host_or_ip,
                                   port,
                                   ipv4,
                                   connecting_timeout_ms,
                                   send_rate_sec,
                                   response_timeout_ms,
                                   receive_rate_sec,
                                   max_connecting,
                                   max_connected,
                                   no_delay,
                                   max_lifetime_ms,
                                   is_outgoing,
                                   enable_keepalive,
                                   first_keepalive_ms,
                                   keepalive_interval_ms,
                                   tokener,
                                   delay_connect)
                Return True
            Else
                Return False
            End If
        End Function

        Public Function create() As powerpoint
            Dim o As powerpoint = Nothing
            assert(create(o))
            Return o
        End Function
    End Class
End Class
