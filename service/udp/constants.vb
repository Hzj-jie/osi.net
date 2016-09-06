
Imports osi.root.constants

Public NotInheritable Class constants
    Public Const ipv4_overhead As UInt32 = 28
    Public Const ipv6_overhead As UInt32 = 48
    Public Const ipv4_packet_size As UInt32 = max_uint16 - ipv4_overhead
    Public Const ipv6_packet_size As UInt32 = max_uint16 - ipv6_overhead  ' The max size to avoid jumbogram.

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class default_value
    Public Const response_timeout_ms As Int64 = 30 * second_milli
    Public Const send_rate_sec As UInt32 = 1
    Public Const receive_rate_sec As UInt32 = 1
    Public Const ipv4 As Boolean = True
    Public Const max_receive_buffer_size As UInt32 = constants.ipv4_packet_size * 10

    Private Sub New()
    End Sub
End Class