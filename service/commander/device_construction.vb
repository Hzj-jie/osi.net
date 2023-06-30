
Imports osi.root.constants
Imports osi.service.device
Imports osi.service.transmitter

<global_init(global_init_level.services)>
Public Module _device_construction
    Sub New()
        adapter_registry.register(Of block, herald)(constants.block_herald_adapter_type,
                                                   constants.block_secondary_type_name,
                                                   AddressOf block_herald_adapter.[New])
        adapter_registry.register(Of text, herald)(constants.text_herald_adapter_type,
                                                   constants.text_secondary_type_name,
                                                   AddressOf text_herald_adapter.[New])
        adapter_registry.register(Of stream_text, herald)(constants.stream_text_herald_adapter_type,
                                                          constants.stream_text_secondary_type_name,
                                                          AddressOf stream_text_herald_adapter.[New])
    End Sub

    Private Sub init()
    End Sub
End Module
