
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net.Sockets
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.udp
Imports base_shared_component = osi.service.selector.shared_component(Of System.UInt16,
                                                                         String,
                                                                         System.Net.Sockets.UdpClient,
                                                                         Byte(),
                                                                         osi.service.udp.powerpoint)

Public NotInheritable Class functor
    Inherits base_shared_component.default_functor

    Public Overrides Sub dispose_component(ByVal c As UdpClient)
        c.shutdown()
    End Sub

    Public Overrides Function accept_new_component(ByVal p As powerpoint) As Boolean
        Return p.incoming()
    End Function

    Public Overrides Function create_component(ByVal p As powerpoint,
                                               ByVal id As UInt16,
                                               ByRef o As UdpClient) As Boolean
        assert(is_valid_port(id))
        Return connector.[New](p, id, o)
    End Function

    Public Overrides Function is_valid_port(ByVal id As UInt16) As Boolean
        Return id <> socket_invalid_port
    End Function

    Protected Overrides Function create_receiver(ByVal dev As ref_instance(Of UdpClient)) _
                                                As base_shared_component.shared_receiver
        Return New shared_receiver(dev)
    End Function

    Protected Overrides Function create_sender(ByVal dev As ref_instance(Of UdpClient),
                                               ByVal remote As const_pair(Of String, UInt16)) _
                                              As base_shared_component.exclusive_sender
        Return New exclusive_sender(dev, remote)
    End Function
End Class
