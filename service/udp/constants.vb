
Imports osi.root.constants

Public NotInheritable Class default_value
    Public Const response_timeout_ms As Int64 = 30 * second_milli
    Public Const send_rate_sec As UInt32 = 1
    Public Const receive_rate_sec As UInt32 = 1
    Public Const ipv4 As Boolean = True

    Private Sub New()
    End Sub
End Class