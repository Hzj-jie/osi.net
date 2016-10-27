
Imports osi.service.transmitter

' TODO
' Share one local resource with several devices, each one is identified by
' (RESOURCE_ID_T: local_resource_id, RESOURCE_ID_T: remote_resource_id, REMOTE_ID_T: remote_id).
' A typical usage is to share one local udp client to talk with several remote hosts.
' @RESOURCE_ID_T: the type to define a resource id, say, uint16 for socket ports.
' @REMOTE_ID_T: the type to define a remote endpoint, say, IPAddress for socket ip address.
' @DEV_T: the type of the underlying device, say, UdpClient for udp.
' @DATA_T: the type of the data send to and receive from DEV_T, say, byte() for udp.
' @PARAMETER_T: the type of an extra parameter for device collection, speaker or listener, say, udp.powerpoint for udp.
Partial Public Class shared_device(Of RESOURCE_ID_T, REMOTE_ID_T, DEV_T, DATA_T, PARAMETER_T)
    ' The input PARAMETER_T of constructor.
    Protected ReadOnly p As PARAMETER_T
    ' The RESOURCE_ID_T to represent local resource id allocated in constructor.
    Protected ReadOnly local_resource_id As RESOURCE_ID_T
    ' The data received from dispenser.
    Protected ReadOnly pump As slimqless2_event_sync_T_pump(Of DATA_T)
    ' The converted receiver of @pump.
    Protected ReadOnly receiver As event_sync_T_pump_T_receiver_adapter(Of DATA_T)

    Public Sub New(ByVal p As PARAMETER_T, ByVal c As collection, ByVal buff() As Byte)

    End Sub
End Class
