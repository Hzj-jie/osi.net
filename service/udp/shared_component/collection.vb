
Imports osi.root.connector
Imports osi.root.template
Imports base_shared_component = osi.service.selector.shared_component(Of
        System.UInt16, System.Net.IPAddress, System.Net.Sockets.UdpClient, Byte(), osi.service.udp.powerpoint)

Public NotInheritable Class collection
    Inherits base_shared_component.collection(Of _max_uint16, _uint16_to_uint32, functor)
End Class
