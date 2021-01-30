
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Public Module _memory_usage
    Public ReadOnly max_physical_memory_usage As Int64 = calculate_max_physical_memory_usage()
    Public ReadOnly max_virtual_memory_usage As Int64 = calculate_max_virtual_memory_usage()

    Private Function calculate_max_physical_memory_usage() As Int64
        Dim max_physical_memory_usage As Int64
        If Not env_value(env_keys("max", "physical", "memory", "usage"), max_physical_memory_usage) Then
            max_physical_memory_usage = CLng(total_physical_memory())
        End If
        Return max_physical_memory_usage
    End Function

    Private Function calculate_max_virtual_memory_usage() As Int64
        Dim max_virtual_memory_usage As Int64
        If Not env_value(env_keys("max", "virtual", "memory", "usage"), max_virtual_memory_usage) Then
            max_virtual_memory_usage = CLng(total_virtual_memory())
        End If
        Return max_virtual_memory_usage
    End Function
End Module