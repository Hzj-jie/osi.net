
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports osi.root.constants
Imports osi.root.connector
Imports osi.service.argument

Partial Public Class powerpoint
    Public Class creator
        Private host_or_ip As String
        Private remote_port As UInt16
        Private local_port As UInt16
        Private send_rate_sec As UInt32
        Private response_timeout_ms As Int64
        Private receive_rate_sec As UInt32
        Private ipv4 As Boolean
        Private max_receive_buffer_size As UInt32
        Private accept_new_connection As Boolean
        Private max_connected As uint32

        Shared Sub New()
            assert(IPEndPoint.MinPort >= 0 AndAlso IPEndPoint.MaxPort <= max_uint16)
        End Sub

        Public Sub New()
            Me.without_host_or_ip().
               without_ipv4().
               without_local_port().
               without_receive_rate_sec().
               without_remote_port().
               without_response_timeout_ms().
               without_send_rate_sec().
               without_max_receive_buffer_size().
               without_accept_new_connection().
               without_max_connected()
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

        Public Function remote_defined() As Boolean
            Return Not String.IsNullOrEmpty(host_or_ip) AndAlso remote_port <> socket_invalid_port
        End Function

        Public Function local_defined() As Boolean
            Return local_port <> socket_invalid_port
        End Function

        Public Function valid() As Boolean
            Return remote_defined() OrElse
                   local_defined()
        End Function

        Public Function with_host_or_ip(ByVal host_or_ip As String) As creator
            Me.host_or_ip = host_or_ip
            Return Me
        End Function

        Public Function without_host_or_ip() As creator
            Return with_host_or_ip(Nothing)
        End Function

        Public Function with_remote_port(ByVal port As UInt16) As creator
            Me.remote_port = port
            Return Me
        End Function

        Public Function with_remote_port_str(ByVal port As String) As creator
            Return with_remote_port(port.to(Of UInt16)())
        End Function

        Public Function without_remote_port() As creator
            Return with_remote_port(socket_invalid_port)
        End Function

        Public Function with_remote_endpoint(ByVal ep As IPEndPoint) As creator
            If ep Is Nothing Then
                Return without_host_or_ip().without_remote_port()
            Else
                If ep.Address().Equals(IPAddress.Any) OrElse
                   ep.Address().Equals(IPAddress.IPv6Any) Then
                    without_host_or_ip()
                Else
                    with_host_or_ip(Convert.ToString(ep.Address()))
                End If
                Return with_remote_port(CUShort(ep.Port()))
            End If
        End Function

        Public Function with_local_port(ByVal port As UInt16) As creator
            Me.local_port = port
            Return Me
        End Function

        Public Function with_local_port_str(ByVal port As String) As creator
            Return with_local_port(port.to(Of UInt16)())
        End Function

        Public Function without_local_port() As creator
            Return with_local_port(socket_invalid_port)
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

        Public Function with_max_receive_buffer_size(ByVal max_receive_buffer_size As UInt32) As creator
            Me.max_receive_buffer_size = max_receive_buffer_size
            Return Me
        End Function

        Public Function with_max_receive_buffer_size_str(ByVal max_receive_buffer_size As String) As creator
            Return with_max_receive_buffer_size(
                       max_receive_buffer_size.to(Of UInt32)(default_value.max_receive_buffer_size))
        End Function

        Public Function without_max_receive_buffer_size() As creator
            Return with_max_receive_buffer_size(default_value.max_receive_buffer_size)
        End Function

        Public Function with_accept_new_connection(ByVal accept_new_connection As Boolean) As creator
            Me.accept_new_connection = accept_new_connection
            Return Me
        End Function

        Public Function with_accept_new_connection_str(ByVal accept_new_connection As String) As creator
            Return with_accept_new_connection(accept_new_connection.to(Of Boolean)(default_value.accept_new_connection))
        End Function

        Public Function without_accept_new_connection() As creator
            Return with_accept_new_connection(default_value.accept_new_connection)
        End Function

        Public Function with_max_connected(ByVal max_connected As UInt32) As creator
            Me.max_connected = max_connected
            Return Me
        End Function

        Public Function with_max_connected_str(ByVal max_connected As String) As creator
            Return with_max_connected(max_connected.to(Of UInt32)(default_value.max_connected))
        End Function

        Public Function without_max_connected() As creator
            Return with_max_connected(default_value.max_connected)
        End Function

        Public Function with_var(ByVal v As var) As creator
            Const p_host As String = "host"
            Const p_remote_port As String = "remote-port"
            Const p_local_port As String = "local-port"
            Const p_response_timeout_ms As String = "response-timeout-ms"
            Const p_send_rate_sec As String = "send-rate-sec"
            Const p_receive_rate_sec As String = "receive-rate-sec"
            Const p_ipv4 As String = "ipv4"
            Const p_max_receive_buffer_size As String = "max-receive-buffer-size"
            Const p_accept_new_connection As String = "accept-new-connection"
            Const p_max_connected As String = "max-connected"
            assert(v IsNot Nothing)
            v.bind(p_host,
                   p_remote_port,
                   p_local_port,
                   p_response_timeout_ms,
                   p_send_rate_sec,
                   p_receive_rate_sec,
                   p_ipv4,
                   p_max_receive_buffer_size,
                   p_accept_new_connection,
                   p_max_connected)
            Return with_host_or_ip(v(p_host)).
                   with_remote_port_str(v(p_remote_port)).
                   with_local_port_str(v(p_local_port)).
                   with_response_timeout_ms_str(v(p_response_timeout_ms)).
                   with_send_rate_sec_str(v(p_send_rate_sec)).
                   with_receive_rate_sec_str(v(p_receive_rate_sec)).
                   with_ipv4_str(v(p_ipv4)).
                   with_max_receive_buffer_size_str(v(p_max_receive_buffer_size)).
                   with_accept_new_connection_str(v(p_accept_new_connection)).
                   with_max_connected_str(v(p_max_connected))
        End Function

        Public Function create(ByRef o As powerpoint) As Boolean
            If valid() Then
                o = New powerpoint(host_or_ip,
                                   remote_port,
                                   local_port,
                                   response_timeout_ms,
                                   send_rate_sec,
                                   receive_rate_sec,
                                   ipv4,
                                   max_receive_buffer_size,
                                   accept_new_connection,
                                   max_connected)
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
