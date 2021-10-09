
Option Explicit On
Option Infer Off
Option Strict On

Public Module _computer_info
    Public ReadOnly processor_architecture As String = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")

    'for mono
    Public Function available_physical_memory() As UInt64
        Try
            Return computer.Info().AvailablePhysicalMemory()
        Catch
            Return 0
        End Try
    End Function

    Public Function available_virtual_memory() As UInt64
        Try
            Return computer.Info().AvailableVirtualMemory()
        Catch
            Return 0
        End Try
    End Function

    Public Function total_physical_memory() As UInt64
        Try
            Return computer.Info().TotalPhysicalMemory()
        Catch
            Return 0
        End Try
    End Function

    Public Function total_virtual_memory() As UInt64
        Try
            Return computer.Info().TotalVirtualMemory()
        Catch
            Return 0
        End Try
    End Function
End Module
