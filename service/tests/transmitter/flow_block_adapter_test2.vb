
Imports osi.service.transmitter

Public Class flow_block_adapter_test2
    Inherits complete_io_test2(Of block_flow_adapter)

    Private ReadOnly m As mock_block

    Public Sub New()
        MyBase.New()
        m = New mock_block()
    End Sub

    Protected Overrides Function create_receive_flow() As block_flow_adapter
        Return New block_flow_adapter(New flow_block_adapter(New block_flow_adapter(m)))
    End Function

    Protected Overrides Function create_send_flow() As block_flow_adapter
        Return New block_flow_adapter(New flow_block_adapter(New block_flow_adapter(m.the_other_end())))
    End Function
End Class
