﻿
Imports System.Net.Sockets
Imports osi.root.template

' A collection of all udp-clients, each one can be retrieved by its local port.
Public NotInheritable Class udp_clients
    Inherits _46_collection(Of UdpClient, _New)

    Public NotInheritable Class _New
        Inherits __do(Of powerpoint, UdpClient, Boolean)

        Public Overrides Function at(ByRef i As powerpoint, ByRef j As UdpClient) As Boolean
            Return connector.[New](i, j)
        End Function
    End Class

    Private Sub New()
        MyBase.New()
    End Sub
End Class
