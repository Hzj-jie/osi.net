
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports base_sharedtransmitter = osi.service.sharedtransmitter.sharedtransmitter(Of System.UInt16,
                                                                                    String,
                                                                                    System.Net.Sockets.UdpClient,
                                                                                    Byte(),
                                                                                    osi.service.udp.powerpoint)

Public NotInheritable Class shared_receiver
    Inherits base_sharedtransmitter.shared_receiver

    Private ReadOnly d As udp_bytes_dev

    Public Sub New(ByVal d As ref_instance(Of UdpClient))
        MyBase.New(d)
        Me.d = New udp_bytes_dev(component_getter())
    End Sub

    Public Overrides Function receive(ByVal o As ref(Of pair(Of Byte(),
                                      const_pair(Of String, UInt16)))) As event_comb
        Return d.receive(o)
    End Function

    Public Overrides Function sense(ByVal pending As ref(Of Boolean),
                                    ByVal timeout_ms As Int64) As event_comb
        Return d.sense(pending, timeout_ms)
    End Function
End Class
