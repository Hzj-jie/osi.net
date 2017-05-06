
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.transmitter

' Share one local shareable component with several components, each one is identified by
' (PORT_T: local_port, PORT_T: remote_port, ADDRESS_T: remote_address).
' A typical usage is to share one local udp client to talk with several remote hosts.
' @PORT_T: the type to define a resource id, say, uint16 for socket ports.
' @ADDRESS_T: the type to define a remote const_pair(Of ADDRESS_T, PORT_T), say, IPAddress for socket ip address.
' @COMPONENT_T: the type of the underlying component, say, UdpClient for udp.
' @DATA_T: the type of the data send to and receive from COMPONENT_T, say, byte() for udp.
' @PARAMETER_T: the type of an extra parameter for device collection, to create COMPONENT_T or dispenser, say,
'               udp.powerpoint for udp.
Partial Public Class shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)
    Inherits disposer
    ' The input PARAMETER_T of constructor.
    Public ReadOnly p As PARAMETER_T
    ' The PORT_T to represent local resource id allocated in constructor.
    Public ReadOnly local_port As PORT_T
    ' The pair of ADDRESS_T and PORT_T to represent the remote endpoint.
    Public ReadOnly remote As const_pair(Of ADDRESS_T, PORT_T)
    ' The converted receiver of @pump.
    Public ReadOnly receiver As event_sync_T_pump_T_receiver_adapter(Of DATA_T)
    ' The sender to @remote.
    Public ReadOnly sender As exclusive_sender
    ' The underlying component.
    Private ReadOnly component_ref As ref_instance(Of COMPONENT_T)
    ' The data received from dispenser.
    Private ReadOnly pump As slimqless2_event_sync_T_pump(Of DATA_T)
    ' The accepter to receive from dispenser and send the data to @pump.
    Private ReadOnly accepter As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)).accepter
    ' The dispenser which takes responsibility to dispense data to current shared_device.
    Private ReadOnly dispenser As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T))
    ' Whether current shared_device is valid.
    Private ReadOnly valid As Boolean

    Private Sub New(ByVal valid As Boolean,
                    ByVal p As PARAMETER_T,
                    ByVal component_ref As ref_instance(Of COMPONENT_T),
                    ByVal local_port As PORT_T,
                    ByVal remote As const_pair(Of ADDRESS_T, PORT_T),
                    ByVal sender As exclusive_sender,
                    ByVal accepter As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)).accepter,
                    ByVal dispenser As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)),
                    ByVal data As DATA_T,
                    ByVal has_data As Boolean)
        Me.valid = valid
        If is_valid() Then
            assert(Not component_ref Is Nothing)
            assert(Not sender Is Nothing)
            assert(Not accepter Is Nothing)
            assert(Not dispenser Is Nothing)
            Me.p = p
            Me.local_port = local_port
            Me.remote = remote
            Me.component_ref = component_ref
            Me.sender = sender
            Me.accepter = accepter
            Me.dispenser = dispenser

            Me.component_ref.ref()
            Me.pump = New slimqless2_event_sync_T_pump(Of DATA_T)()
            Me.receiver = event_sync_T_pump_T_receiver_adapter.[New](Me.pump)
            AddHandler Me.accepter.received, AddressOf push_queue
            If has_data Then
                push_queue(data, Nothing)
            End If
            Me.dispenser.attach(Me.accepter)
        End If
    End Sub

    Public Function component() As COMPONENT_T
        If is_valid() Then
            assert(Not component_ref Is Nothing)
            assert(component_ref.referred())
            Dim r As COMPONENT_T = Nothing
            r = component_ref.get()
            Return r
        Else
            Return Nothing
        End If
    End Function

    Public Function is_valid() As Boolean
        Return valid
    End Function

    Protected Overrides Sub disposer()
        If is_valid() Then
            assert(Not component_ref Is Nothing)
            component_ref.unref()
            assert(Not dispenser Is Nothing)
            assert(dispenser.detach(accepter))
        End If
    End Sub

    Private Sub push_queue(ByVal b As DATA_T, ByVal remote As const_pair(Of ADDRESS_T, PORT_T))
        pump.emplace(b)
    End Sub
End Class
