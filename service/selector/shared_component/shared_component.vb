
Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.transmitter

' Share one local shareable component with several components, each one is identified by
' (RESOURCE_ID_T: local_resource_id, RESOURCE_ID_T: remote_resource_id, REMOTE_ID_T: remote_id).
' A typical usage is to share one local udp client to talk with several remote hosts.
' @RESOURCE_ID_T: the type to define a resource id, say, uint16 for socket ports.
' @REMOTE_ID_T: the type to define a remote endpoint, say, IPAddress for socket ip address.
' @COMPONENT_T: the type of the underlying component, say, UdpClient for udp.
' @DATA_T: the type of the data send to and receive from COMPONENT_T, say, byte() for udp.
' @PARAMETER_T: the type of an extra parameter for device collection, to create COMPONENT_T or dispenser, say,
'               udp.powerpoint for udp.
Partial Public Class shared_component(Of RESOURCE_ID_T, REMOTE_ID_T, COMPONENT_T, DATA_T, PARAMETER_T)
    Inherits disposer
    ' The input PARAMETER_T of constructor.
    Protected ReadOnly p As PARAMETER_T
    ' The RESOURCE_ID_T to represent local resource id allocated in constructor.
    Protected ReadOnly local_resource_id As RESOURCE_ID_T
    ' The converted receiver of @pump.
    Protected ReadOnly receiver As event_sync_T_pump_T_receiver_adapter(Of DATA_T)
    ' The underlying component.
    Private ReadOnly comp As ref_instance(Of COMPONENT_T)
    ' The data received from dispenser.
    Private ReadOnly pump As slimqless2_event_sync_T_pump(Of DATA_T)
    ' The accepter to receive from dispenser and send the data to @pump.
    Private ReadOnly accepter As dispenser(Of DATA_T, endpoint).iaccepter
    ' The dispenser which takes responsibility to dispense data to current shared_device.
    Private ReadOnly dispenser As dispenser(Of DATA_T, endpoint)
    ' Whether current shared_device is valid.
    Private ReadOnly valid As Boolean

    Public Sub New(ByVal p As PARAMETER_T,
                   ByVal c As collection,
                   ByVal accepter As dispenser(Of DATA_T, endpoint).iaccepter,
                   ByVal data As DATA_T)
        assert(Not c Is Nothing)
        assert(Not accepter Is Nothing)
        If c.[New](p, local_resource_id, comp) AndAlso
           c.[New](p, local_resource_id, comp, dispenser) Then
            pump = New slimqless2_event_sync_T_pump(Of DATA_T)()
            receiver = event_sync_T_pump_T_receiver_adapter.[New](pump)
            Me.accepter = accepter
            AddHandler Me.accepter.received, AddressOf push_queue
            If Not data Is Nothing Then
                push_queue(data, Nothing)
            End If
            dispenser.attach(accepter)
            valid = True
        Else
            valid = False
        End If
    End Sub

    Public Function underlying_device() As COMPONENT_T
        assert(is_valid)
        Return +comp
    End Function

    Public Function is_valid() As Boolean
        Return valid
    End Function

    Protected Overrides Sub disposer()
        If is_valid() Then
            assert(Not dispenser Is Nothing)
            dispenser.release()
        End If
    End Sub

    Private Sub push_queue(ByVal b As DATA_T, ByVal remote As endpoint)
        pump.emplace(b)
    End Sub
End Class
