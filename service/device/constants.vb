
Imports osi.root.constants

Public NotInheritable Class constants
    Public Const default_auto_generation_check_interval_ms As Int64 = 100
    Public Const default_auto_generation_failure_wait_ms As Int64 = 1000
    Public Const default_auto_generation_max_concurrent_generations As Int32 = 10
    Public Const default_checker_interval_ms As Int64 = 60 * second_milli
    Public Const flow_block_adapter_type As String = "flow-block-adapter"
    Public Const block_flow_adapter_type As String = "block-flow-adapter"
    Public Const flow_datagram_adapter_type As String = "flow-datagram-adapter"
    Public Const datagram_flow_adapter_type As String = "datagram-flow-adapter"
    Public Const block_piece_dev_adapter_type As String = "block-piece-dev-adapter"
    Public Const piece_dev_block_adapter_type As String = "piece-dev-block-adapter"
    Public Const flow_piece_dev_adapter_type As String = "flow-piece-dev-adapter"
    Public Const piece_dev_flow_adapter_type As String = "piece-dev-flow-adapter"
    Public Const flow_secondary_type_name As String = "flow"
    Public Const block_secondary_type_name As String = "block"
    Public Const datagram_secondary_type_name As String = "datagram"
    Public Const piece_dev_secondary_type_name As String = "piece-dev"

    Private Sub New()
    End Sub
End Class
