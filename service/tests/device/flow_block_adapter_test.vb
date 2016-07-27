
Imports osi.root.constants
Imports osi.root.connector
Imports osi.service.convertor
Imports osi.service.device

Public Class flow_block_adapter_test
    Inherits complete_io_test(Of block_flow_adapter)

    ' flow_block_adapter will convert data into a chunk.
    Protected Overrides Function create_receive_flow(ByVal buff() As Byte) As block_flow_adapter
        Return New block_flow_adapter(New flow_block_adapter(New mock_flow_dev(buff.to_chunk(), uint32_0, True, False)))
    End Function

    Protected Overrides Function create_send_flow(ByVal send_size As UInt32) As block_flow_adapter
        Return New block_flow_adapter(New flow_block_adapter(
                                          New mock_flow_dev(send_size + preamble_length, True, False)))
    End Function

    Protected Overrides Function receive_consistent(ByVal flow_dev As block_flow_adapter,
                                                    ByVal buff() As Byte) As Boolean
        Return cast(Of mock_flow_dev)(cast(Of flow_block_adapter)(flow_dev.underlying_device).underlying_device) _
                   .receive_consistent(buff.to_chunk())
    End Function

    Protected Overrides Function send_consistent(ByVal flow_dev As block_flow_adapter, ByVal buff() As Byte) As Boolean
        Return cast(Of mock_flow_dev)(cast(Of flow_block_adapter)(flow_dev.underlying_device).underlying_device) _
                   .send_consistent(buff.to_chunk())
    End Function
End Class
