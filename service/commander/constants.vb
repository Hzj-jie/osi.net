
Imports osi.root.constants
Imports osi.root.connector
Imports osi.service.secure

Namespace constants
    Public Module _constants
        Public Const stream_pump_wait_ms As Int32 = 1
        Public Const last_error_count As Int32 = 30
        Public Const ping_interval_ms As Int64 = 1 * second_milli
        Public Const device_change_check_interval_ms As Int64 = 5 * second_milli
        Public Const respond_one_of_wait_device_ms As Int64 = 128
        Public Const accepting_over_max_connecting_wait_ms As Int64 = 100
        Public Const max_concurrent_connecting As UInt32 = max_uint32
        Public Const block_herald_adapter_type = "block-herald-adapter"
        Public Const text_herald_adapter_type = "text-herald-adapter"
        Public Const stream_text_herald_adapter_type = "stream-text-herald-adapter"
        Public Const block_secondary_type_name As String = "block"
        Public Const text_secondary_type_name As String = "text"
        Public Const stream_text_secondary_type_name As String = "stream-text"
        Public Const herald_secondary_type_name As String = "herald"
        Public Const token_default_response_timeout_ms As Int64 = 30 * second_milli
        Public ReadOnly token1_prefix() As Byte = str_bytes("token1")
        Public ReadOnly token1_prefix_len As UInt32 = array_size(token1_prefix)
        Public ReadOnly token2_prefix() As Byte = str_bytes("token2")
        Public ReadOnly token2_prefix_len As UInt32 = array_size(token2_prefix)
        Public ReadOnly default_signer As signer = sign.md5_merge_hasher.ring
    End Module

    Public Enum action As SByte
        ping = min_int8
    End Enum

    Public Enum response As SByte
        invalid_request = min_int8
        failure
        success
    End Enum

    Public Enum parameter As SByte
        name = min_int8
    End Enum
End Namespace
