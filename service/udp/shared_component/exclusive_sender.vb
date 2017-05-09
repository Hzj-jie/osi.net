﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports System.Net.Sockets
Imports osi.root.formation
Imports osi.root.procedure
Imports base_shared_component = osi.service.selector.shared_component(Of System.UInt16, System.Net.IPAddress, System.Net.Sockets.UdpClient, Byte(), osi.service.udp.powerpoint)

Public NotInheritable Class exclusive_sender
    Inherits base_shared_component.exclusive_sender

    Private ReadOnly d As udp_bytes_dev

    Public Sub New(ByVal c As ref_instance(Of UdpClient), ByVal remote As const_pair(Of IPAddress, UInt16))
        MyBase.New(c, remote)
        Me.d = New udp_bytes_dev(component_getter())
    End Sub

    Public Overrides Function send(ByVal i() As Byte) As event_comb
        Return d.send(emplace_make_pair(i, remote))
    End Function
End Class
