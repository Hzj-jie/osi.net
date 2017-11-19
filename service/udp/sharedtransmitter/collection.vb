
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.template
Imports base_sharedtransmitter = osi.service.sharedtransmitter.sharedtransmitter(Of System.UInt16,
                                                                                    String,
                                                                                    System.Net.Sockets.UdpClient,
                                                                                    Byte(),
                                                                                    osi.service.udp.powerpoint)

Public NotInheritable Class collection
    Inherits base_sharedtransmitter.collection(Of _max_uint16, _uint16_to_uint32, functor)
End Class
