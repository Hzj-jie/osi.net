
Imports System.Net.Sockets

Public Module _socket
    Public Const tcp_socket_state_update_interval_ms As Int64 = 5 * second_milli
    Public Const failure_send_buff_size As UInt32 = 0
    Public Const failure_receive_buff_size As UInt32 = 0
    Public Const enable_socket_keepalive As Boolean = True
    Public Const socket_first_keepalive_ms As UInt32 = 8 * second_milli
    Public Const socket_keepalive_interval_ms As UInt32 = 8 * second_milli
    Public Const socket_invalid_port As UInt16 = 0  ' A definitely invalid port
End Module

Public NotInheritable Class IOControlCodeExt
    ' Use Int64 to follow IOControlCode
    Public Const IOC_IN_int As Int64 = &H80000000
    Public Const IOC_VENDOR_int As Int64 = &H18000000
    Public Const SIO_UDP_CONNRESET_int As Int64 = IOC_IN_int Or IOC_VENDOR_int Or 12

    Public Const SIO_UDP_CONNRESET As IOControlCode = CType(SIO_UDP_CONNRESET_int, IOControlCode)

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class SocketOptionExt
    Public Const ipv6_only As SocketOptionName = 27
End Class