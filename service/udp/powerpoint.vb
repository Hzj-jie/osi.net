
Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.service.device

Partial Public Class powerpoint
    Public ReadOnly host_or_ip As String
    Public ReadOnly remote_port As UInt16
    Public ReadOnly local_port As UInt16
    Public ReadOnly response_timeout_ms As Int64
    Public ReadOnly ipv4 As Boolean
    Public ReadOnly max_receive_buffer_size As UInt32
    Public ReadOnly address_family As AddressFamily
    Public ReadOnly identity As String
    Public ReadOnly packet_size As UInt32
    Private ReadOnly send_rate_sec As UInt32
    Private ReadOnly receive_rate_sec As UInt32
    Private ReadOnly overhead As UInt32

    Private Sub New(ByVal host_or_ip As String,
                    ByVal remote_port As UInt16,
                    ByVal local_port As UInt16,
                    ByVal response_timeout_ms As Int64,
                    ByVal send_rate_sec As UInt32,
                    ByVal receive_rate_sec As UInt32,
                    ByVal ipv4 As Boolean,
                    ByVal max_receive_buffer_size As UInt32)
        Me.host_or_ip = host_or_ip
        Me.remote_port = remote_port
        Me.local_port = local_port
        Me.response_timeout_ms = response_timeout_ms
        Me.send_rate_sec = send_rate_sec
        Me.receive_rate_sec = receive_rate_sec
        Me.ipv4 = ipv4
        Me.max_receive_buffer_size = max_receive_buffer_size

        Me.address_family = If(ipv4, AddressFamily.InterNetwork, AddressFamily.InterNetworkV6)
        Me.overhead = If(ipv4, constants.ipv4_overhead, constants.ipv6_overhead)
        Me.identity = strcat("udp@port:", local_port, "@remote:", host_or_ip, ":", remote_port)
        Me.packet_size = If(ipv4, constants.ipv4_packet_size, constants.ipv6_packet_size)
        If Me.max_receive_buffer_size < packet_size * 2 Then
            Me.max_receive_buffer_size = packet_size * 2
        End If
    End Sub

    Public Function transceive_timeout() As transceive_timeout
        Return New transceive_timeout(send_rate_sec, receive_rate_sec, overhead)
    End Function
End Class
