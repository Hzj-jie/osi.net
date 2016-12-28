
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
    ' The converted receiver of @pump.
    Public ReadOnly receiver As event_sync_T_pump_T_receiver_adapter(Of DATA_T)
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

    Private Sub New(ByVal p As PARAMETER_T,
                    ByVal c As collection,
                    ByVal accepter As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)).accepter,
                    ByVal data As DATA_T,
                    ByVal has_data As Boolean)
        assert(Not c Is Nothing)
        assert(Not accepter Is Nothing)
        If c.[New](p, local_port, component_ref) Then
            component_ref.ref()
            If c.[New](p, local_port, component_ref, dispenser) Then
                pump = New slimqless2_event_sync_T_pump(Of DATA_T)()
                receiver = event_sync_T_pump_T_receiver_adapter.[New](pump)
                Me.accepter = accepter
                AddHandler Me.accepter.received, AddressOf push_queue
                If has_data Then
                    push_queue(data, Nothing)
                End If
                dispenser.attach(accepter)
                valid = True
            Else
                valid = False
            End If
        Else
            valid = False
        End If
    End Sub

    ' Incoming
    Public Sub New(ByVal p As PARAMETER_T,
                   ByVal local_port As PORT_T,
                   ByVal component As ref_instance(Of COMPONENT_T),
                   ByVal dispenser As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)),
                   ByVal accepter As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)).accepter)
        Me.New(p, New pass_through_collection(local_port, component, dispenser), accepter)
    End Sub

    ' Outgoing
    Public Sub New(ByVal p As PARAMETER_T,
                   ByVal c As collection,
                   ByVal accepter As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)).accepter)
        Me.New(p, c, accepter, Nothing, False)
    End Sub

    ' Incoming
    Public Sub New(ByVal p As PARAMETER_T,
                   ByVal local_port As PORT_T,
                   ByVal component As ref_instance(Of COMPONENT_T),
                   ByVal dispenser As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)),
                   ByVal accepter As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)).accepter,
                   ByVal data As DATA_T)
        Me.New(p, New pass_through_collection(local_port, component, dispenser), accepter, data)
    End Sub

    ' Outgoing
    Public Sub New(ByVal p As PARAMETER_T,
                   ByVal c As collection,
                   ByVal accepter As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)).accepter,
                   ByVal data As DATA_T)
        Me.New(p, c, accepter, data, True)
    End Sub

    ' Outgoing
    Public Sub New(ByVal p As PARAMETER_T,
                   ByVal c As collection,
                   ByVal remote As const_pair(Of ADDRESS_T, PORT_T))
        Me.New(p, c, New default_accepter(remote))
    End Sub

    ' Outgoing
    Public Sub New(ByVal p As PARAMETER_T,
                   ByVal c As collection,
                   ByVal remote As const_pair(Of ADDRESS_T, PORT_T),
                   ByVal data As DATA_T)
        Me.New(p, c, New default_accepter(remote), data)
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
        If Not component_ref Is Nothing Then
            component_ref.unref()
        End If
        If is_valid() Then
            assert(Not dispenser Is Nothing)
            assert(dispenser.detach(accepter))
        End If
    End Sub

    Private Sub push_queue(ByVal b As DATA_T, ByVal remote As const_pair(Of ADDRESS_T, PORT_T))
        pump.emplace(b)
    End Sub
End Class

Public NotInheritable Class shared_component
    ' Outgoing
    Public Shared Function [New](Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T) _
                                (ByVal p As PARAMETER_T,
                                 ByVal c As shared_component(Of PORT_T,
                                                                ADDRESS_T,
                                                                COMPONENT_T,
                                                                DATA_T,
                                                                PARAMETER_T).collection,
                                 ByVal accepter As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)).accepter,
                                 ByVal data As DATA_T) _
                                As shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)
        Return New shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)(p, c, accepter, data)
    End Function

    ' Incoming
    Public Shared Function [New](Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T) _
                                (ByVal p As PARAMETER_T,
                                 ByVal local_port As PORT_T,
                                 ByVal component As ref_instance(Of COMPONENT_T),
                                 ByVal dispenser As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)),
                                 ByVal accepter As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)).accepter,
                                 ByVal data As DATA_T) _
                                As shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)
        Return New shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T) _
                                   (p, local_port, component, dispenser, accepter, data)
    End Function

    ' Outgoing
    Public Shared Function [New](Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T) _
                                (ByVal p As PARAMETER_T,
                                 ByVal c As shared_component(Of PORT_T,
                                                                ADDRESS_T,
                                                                COMPONENT_T,
                                                                DATA_T,
                                                                PARAMETER_T).collection,
                                 ByVal remote As const_pair(Of ADDRESS_T, PORT_T),
                                 ByVal data As DATA_T) _
                                As shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)
        Return New shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)(p, c, remote, data)
    End Function

    ' Outgoing
    Public Shared Function [New](Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T) _
                                (ByVal p As PARAMETER_T,
                                 ByVal c As shared_component(Of PORT_T,
                                                                ADDRESS_T,
                                                                COMPONENT_T,
                                                                DATA_T,
                                                                PARAMETER_T).collection,
                                 ByVal accepter As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)).accepter) _
                                As shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)
        Return New shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)(p, c, accepter)
    End Function

    ' Incoming
    Public Shared Function [New](Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T) _
                                (ByVal p As PARAMETER_T,
                                 ByVal local_port As PORT_T,
                                 ByVal component As ref_instance(Of COMPONENT_T),
                                 ByVal dispenser As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)),
                                 ByVal accepter As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)).accepter) _
                                As shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)
        Return New shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T) _
                                   (p, local_port, component, dispenser, accepter)
    End Function

    ' Outgoing
    Public Shared Function [New](Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T) _
                                (ByVal p As PARAMETER_T,
                                 ByVal c As shared_component(Of PORT_T,
                                                                ADDRESS_T,
                                                                COMPONENT_T,
                                                                DATA_T,
                                                                PARAMETER_T).collection,
                                 ByVal remote As const_pair(Of ADDRESS_T, PORT_T)) _
                                As shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)
        Return New shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)(p, c, remote)
    End Function

    Private Sub New()
    End Sub
End Class
