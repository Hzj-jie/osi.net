
Imports System.Net.Sockets
Imports osi.root.constants
Imports osi.root.template

' A collection of all udp-clients, each one can be retrieved by its local port.
Public NotInheritable Class udp_clients
    Inherits _46_collection(Of UdpClient, _New)

    Public NotInheritable Class _New
        Inherits __do(Of powerpoint, UInt16, UdpClient, Boolean)

        Public Overrides Function at(ByRef i As powerpoint, ByRef j As UInt16, ByRef k As UdpClient) As Boolean
            If j = socket_invalid_port Then
                Return False
            Else
                Return connector.[New](i, j, k)
            End If
        End Function
    End Class

    Private Sub New()
        MyBase.New()
    End Sub
End Class
