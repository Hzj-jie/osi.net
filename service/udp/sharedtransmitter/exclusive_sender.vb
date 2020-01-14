
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net.Sockets
Imports osi.root.formation
Imports osi.root.procedure
Imports base_sharedtransmitter = osi.service.sharedtransmitter.sharedtransmitter(Of System.UInt16,
                                                                                    String,
                                                                                    System.Net.Sockets.UdpClient,
                                                                                    Byte(),
                                                                                    osi.service.udp.powerpoint)

Public NotInheritable Class exclusive_sender
    Inherits base_sharedtransmitter.exclusive_sender

    Private ReadOnly d As udp_bytes_dev

    Public Sub New(ByVal c As ref_instance(Of UdpClient), ByVal remote As const_pair(Of String, UInt16))
        MyBase.New(c, remote)
        Me.d = New udp_bytes_dev(component_getter())
    End Sub

    Public Overrides Function send(ByVal i() As Byte) As event_comb
        Return d.send(pair.emplace_of(i, remote))
    End Function
End Class
