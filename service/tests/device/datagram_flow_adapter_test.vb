
Imports osi.service.device

Public Class datagram_flow_adapter_test
    Inherits complete_io_test2(Of datagram_flow_adapter)

    Private ReadOnly m As mock_block

    Public Sub New()
        MyBase.New()
        m = New mock_block()
    End Sub

    Protected Overrides Function create_receive_flow() As datagram_flow_adapter
        Return New datagram_flow_adapter(New flow_datagram_adapter(New block_flow_adapter(m)))
    End Function

    Protected Overrides Function create_send_flow() As datagram_flow_adapter
        Return New datagram_flow_adapter(New flow_datagram_adapter(New block_flow_adapter(m.the_other_end())))
    End Function
End Class
