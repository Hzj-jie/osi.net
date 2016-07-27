
Imports System.Net
Imports System.Net.Sockets
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.device
Imports osi.service.udp
Imports osi.tests.service.device

Public Class client_datagram_adapter_fix_source_test
    Inherits client_datagram_adapter_test

    Public Sub New()
        MyBase.New(True, False)
    End Sub
End Class

Public Class client_datagram_adapter_missing_target_test
    Inherits client_datagram_adapter_test

    Public Sub New()
        MyBase.New(False, True)
    End Sub
End Class

Public Class client_datagram_adapter_test
    Inherits complete_io_test2(Of datagram_flow_adapter)

    Private ReadOnly fixed_source As Boolean
    Private ReadOnly missing_target As Boolean
    Private ReadOnly l As IPEndPoint
    Private ReadOnly r As IPEndPoint

    Public Sub New()
        Me.New(False, False)
    End Sub

    Protected Sub New(ByVal fixed_source As Boolean, ByVal missing_target As Boolean)
        l = New IPEndPoint(IPAddress.Loopback, rnd_port())
        r = New IPEndPoint(IPAddress.Loopback, rnd_port())
        Me.fixed_source = fixed_source
        Me.missing_target = missing_target
    End Sub

    Public Overrides Function prepare() As Boolean
        If MyBase.prepare() Then
            suppress.pending_io_punishment.inc()
            Return True
        Else
            Return False
        End If
    End Function

    Public Overrides Function finish() As Boolean
        suppress.pending_io_punishment.dec()
        Return MyBase.finish()
    End Function

    Protected Overrides Function create_receive_flow() As datagram_flow_adapter
        Dim c As UdpClient = Nothing
        c = New UdpClient(r.Port())
        c.set_receive_buffer_size(8192 * 512)
        Return New datagram_flow_adapter(New client_datagram_adapter(c,
                                                                     If(fixed_source, {l}, Nothing),
                                                                     If(missing_target, Nothing, l),
                                                                     New transceive_timeout(1024, 1024)))
    End Function

    Protected Overrides Function create_send_flow() As datagram_flow_adapter
        Dim c As UdpClient = Nothing
        c = New UdpClient(l.Port())
        c.set_send_buffer_size(8192 * 512)
        Return New datagram_flow_adapter(New client_datagram_adapter(c,
                                                                     If(fixed_source, {r}, Nothing),
                                                                     r,
                                                                     New transceive_timeout(1024, 1024)))
    End Function
End Class
