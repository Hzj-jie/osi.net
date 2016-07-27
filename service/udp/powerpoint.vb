
Imports System.Net.Sockets
Imports osi.root.connector

Partial Public Class powerpoint
    Public ReadOnly host_or_ip As String
    Public ReadOnly remote_port As UInt16
    Public ReadOnly local_port As UInt16
    Public ReadOnly response_timeout_ms As Int64
    Public ReadOnly ipv4 As Boolean
    Public ReadOnly address_family As AddressFamily
    Public ReadOnly identity As String
    Private ReadOnly send_rate_sec As UInt32
    Private ReadOnly receive_rate_sec As UInt32
    Private ReadOnly overhead As UInt32

    Private Sub New(ByVal host_or_ip As String,
                    ByVal remote_port As UInt16,
                    ByVal local_port As UInt16,
                    ByVal response_timeout_ms As Int64,
                    ByVal send_rate_sec As UInt32,
                    ByVal receive_rate_sec As UInt32,
                    ByVal ipv4 As Boolean)
        Me.host_or_ip = host_or_ip
        Me.remote_port = remote_port
        Me.local_port = local_port
        Me.response_timeout_ms = response_timeout_ms
        Me.send_rate_sec = send_rate_sec
        Me.receive_rate_sec = receive_rate_sec
        Me.ipv4 = ipv4
        Me.address_family = If(ipv4, AddressFamily.InterNetwork, AddressFamily.InterNetworkV6)

        Me.overhead = If(ipv4, 28, 48)
    End Sub

    Public Function transceive_timeout() As transceive_timeout
        Return New transceive_timeout(send_rate_sec, receive_rate_sec, overhead)
    End Function
End Class
