
Option Explicit On
Option Infer Off
Option Strict On

Public Module _computer_info
    Public ReadOnly processor_architecture As String = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")

    ' TODO: Implement Linux memory inspection via /proc/meminfo or GC.GetGCMemoryInfo() in modern .NET.
    Public Function available_physical_memory() As UInt64
        Return computer.Info().AvailablePhysicalMemory()
    End Function

    Public Function available_virtual_memory() As UInt64
        Return computer.Info().AvailableVirtualMemory()
    End Function

    Public Function total_physical_memory() As UInt64
        Return computer.Info().TotalPhysicalMemory()
    End Function

    Public Function total_virtual_memory() As UInt64
        Return computer.Info().TotalVirtualMemory()
    End Function
End Module
