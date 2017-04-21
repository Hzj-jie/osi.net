
Imports System.Net
Imports System.Net.Sockets
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.template
Imports osi.service.selector
Imports osi.root.formation
Imports osi.service.transmitter
Imports osi.service.udp

Public NotInheritable Class collection
    Inherits shared_component(Of UInt16, IPAddress, UdpClient, Byte(), powerpoint).
             collection(Of _max_uint16, _uint16_to_uint32)

    Protected Overrides Sub dispose_component(ByVal c As UdpClient)
        c.shutdown()
    End Sub

    Protected Overrides Function accept_new_component(ByVal p As powerpoint) As Boolean
        Return p.incoming()
    End Function

    Protected Overrides Function create_component(ByVal p As powerpoint,
                                                  ByVal id As UInt16,
                                                  ByRef o As UdpClient) As Boolean
        assert(is_valid_port(id))
        Return connector.[New](p, id, o)
    End Function

    Protected Overrides Function is_valid_port(ByVal id As UInt16) As Boolean
        Return id <> socket_invalid_port
    End Function

    Protected Overrides Function local_port(ByVal p As powerpoint) As UInt16
        Return p.local_port
    End Function

    Protected Overrides Function create_receiver(ByVal dev As ref_instance(Of UdpClient)) _
                                                As T_receiver(Of pair(Of Byte(), const_pair(Of IPAddress, UInt16)))
        Return New receiver(dev)
    End Function
End Class
