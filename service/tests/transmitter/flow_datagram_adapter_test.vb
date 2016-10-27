
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.template
Imports osi.service.transmitter

Public Class flow_datagram_adapter_test
    Inherits complete_io_test(Of datagram_flow_adapter)

    Protected Overrides Function create_receive_flow(ByVal buff() As Byte) As datagram_flow_adapter
        Return New datagram_flow_adapter(New flow_datagram_adapter(Of _1500)(
                                         New mock_flow_dev(buff, uint32_0, True, False)))
    End Function

    Protected Overrides Function create_send_flow(ByVal send_size As UInt32) As datagram_flow_adapter
        Return New datagram_flow_adapter(New flow_datagram_adapter(Of _1500)(
                                         New mock_flow_dev(send_size, True, False)))
    End Function

    Protected Overrides Function receive_consistent(ByVal flow_dev As datagram_flow_adapter,
                                                    ByVal buff() As Byte) As Boolean
        Return cast(Of mock_flow_dev)(cast(Of flow_datagram_adapter(Of _1500))(
               flow_dev.underlying_device).underlying_device).receive_consistent(buff)
    End Function

    Protected Overrides Function send_consistent(ByVal flow_dev As datagram_flow_adapter,
                                                 ByVal buff() As Byte) As Boolean
        Return cast(Of mock_flow_dev)(cast(Of flow_datagram_adapter(Of _1500))(
               flow_dev.underlying_device).underlying_device).send_consistent(buff)
    End Function
End Class
