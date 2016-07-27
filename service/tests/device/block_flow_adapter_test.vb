
Imports osi.root.constants
Imports osi.root.connector
Imports osi.service.convertor
Imports osi.service.device

Public Class block_flow_adapter_test
    Inherits complete_io_test(Of block_flow_adapter)

    Protected Overrides Function create_receive_flow(ByVal buff() As Byte) As block_flow_adapter
        Return New block_flow_adapter(mock_block_dev.create_with_receive_buff(buff))
    End Function

    Protected Overrides Function create_send_flow(ByVal send_size As UInt32) As block_flow_adapter
        Return New block_flow_adapter(New mock_block_dev())
    End Function

    Protected Overrides Function receive_consistent(ByVal flow_dev As block_flow_adapter,
                                                    ByVal buff() As Byte) As Boolean
        ' The data has been received (popped from receive_q already). So following assert should fail as expected.
        ' Return cast(Of mock_block_dev)(flow_dev.underlying_device).receive_q_consistent(buff)
        Return cast(Of mock_block_dev)(flow_dev.underlying_device).receive_q.empty()
    End Function

    Protected Overrides Function send_consistent(ByVal flow_dev As block_flow_adapter, ByVal buff() As Byte) As Boolean
        Return cast(Of mock_block_dev)(flow_dev.underlying_device).send_q_consistent(buff)
    End Function
End Class
