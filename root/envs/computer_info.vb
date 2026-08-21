
Option Explicit On
Option Infer Off
Option Strict On

Public Module _computer_info
    Public ReadOnly processor_architecture As String = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")

    Public Function available_physical_memory() As UInt64
#If NET8_0_OR_GREATER Then
        Dim gc_info As GCMemoryInfo = GC.GetGCMemoryInfo()
        Return CULng(Math.Max(0L, gc_info.TotalAvailableMemoryBytes))
#Else
        Return computer.Info().AvailablePhysicalMemory()
#End If
    End Function

    Public Function available_virtual_memory() As UInt64
#If NET8_0_OR_GREATER Then
        Return available_physical_memory()
#Else
        Return computer.Info().AvailableVirtualMemory()
#End If
    End Function

    Public Function total_physical_memory() As UInt64
#If NET8_0_OR_GREATER Then
        Dim gc_info As GCMemoryInfo = GC.GetGCMemoryInfo()
        Return CULng(Math.Max(0L, gc_info.TotalAvailableMemoryBytes))
#Else
        Return computer.Info().TotalPhysicalMemory()
#End If
    End Function

    Public Function total_virtual_memory() As UInt64
#If NET8_0_OR_GREATER Then
        Return total_physical_memory()
#Else
        Return computer.Info().TotalVirtualMemory()
#End If
    End Function
End Module
