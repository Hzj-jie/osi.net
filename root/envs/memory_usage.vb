
Imports osi.root.constants

Public Module _memory_usage
    Public ReadOnly max_physical_memory_usage As Int64
    Public ReadOnly max_virtual_memory_usage As Int64

    Sub New()
        If Not env_value("max-physical-memory-usage", max_physical_memory_usage) Then
            max_physical_memory_usage = total_physical_memory()
        End If
        If Not env_value("max-virtual-memory-usage", max_virtual_memory_usage) Then
            max_virtual_memory_usage = total_virtual_memory()
        End If
    End Sub
End Module