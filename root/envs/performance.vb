
Imports System.DateTime

Public Module _performance
    Private Sub refresh_process_status()
        current_process.Refresh()
    End Sub

    Private Function processor_usage(ByVal usage As Int64,
                                     ByVal last_usage As Int64,
                                     ByVal now_ticks As Int64,
                                     ByVal last_ticks As Int64) As Double
        Return (usage - last_usage) * 100 / If(now_ticks = last_ticks, 1, now_ticks - last_ticks)
    End Function

    Private last_usage_ticks As Int64 = 0
    Private last_ticks As Int64 = 0

    Public Function recent_processor_usage() As Double
        Dim ticks As Int64 = 0
        Dim usage As Int64 = 0
        ticks = Now().Ticks()
        usage = total_processor_time().Ticks()
        If last_ticks = 0 Then
            last_ticks = current_process.StartTime().Ticks()
        End If
        Dim rtn As Double = 0
        rtn = processor_usage(usage, last_usage_ticks,
                              ticks, last_ticks)
        last_usage_ticks = usage
        last_ticks = ticks
        Return rtn
    End Function

    Public Function processor_usage() As Double
        Return processor_usage(total_processor_time().Ticks(), 0,
                               Now().Ticks(), current_process.StartTime().Ticks())
    End Function

    Public Function total_processor_time() As TimeSpan
        refresh_process_status()
        Try
            Return current_process.TotalProcessorTime()
        Catch
            Return Nothing
        End Try
    End Function

    Public Function total_processor_time_ms() As Int64
        Return Math.Floor(total_processor_time().TotalMilliseconds())
    End Function

    Public Function private_bytes_usage() As Int64
        refresh_process_status()
        Try
            Return current_process.PrivateMemorySize64()
        Catch
            Return 0
        End Try
    End Function

    Public Function virtual_bytes_usage() As Int64
        refresh_process_status()
        Try
            Return current_process.VirtualMemorySize64()
        Catch
            Return 0
        End Try
    End Function

    Public Function workingset_bytes_usage() As Int64
        refresh_process_status()
        Try
            Return current_process.WorkingSet64()
        Catch
            Return 0
        End Try
    End Function

    Public Function min_workingset_bytes_usage() As Int64
        refresh_process_status()
        Try
            Return current_process.MinWorkingSet()
        Catch
            Return 0
        End Try
    End Function

    Public Function max_workingset_bytes_usage() As Int64
        refresh_process_status()
        Try
            Return current_process.MaxWorkingSet()
        Catch
            Return 0
        End Try
    End Function

    Public Function nonpaged_system_memory_bytes_usage() As Int64
        refresh_process_status()
        Try
            Return current_process.NonpagedSystemMemorySize64()
        Catch
            Return 0
        End Try
    End Function

    Public Function paged_memory_bytes_usage() As Int64
        refresh_process_status()
        Try
            Return current_process.PagedMemorySize64()
        Catch
            Return 0
        End Try
    End Function

    Public Function paged_system_memory_bytes_usage() As Int64
        refresh_process_status()
        Try
            Return current_process.PagedSystemMemorySize64()
        Catch
            Return 0
        End Try
    End Function

    Public Function peak_paged_memory_bytes_usage() As Int64
        refresh_process_status()
        Try
            Return current_process.PeakPagedMemorySize64()
        Catch
            Return 0
        End Try
    End Function

    Public Function peak_virtual_memory_bytes_usage() As Int64
        refresh_process_status()
        Try
            Return current_process.PeakVirtualMemorySize64()
        Catch
            Return 0
        End Try
    End Function

    Public Function peak_workingset_bytes_usage() As Int64
        refresh_process_status()
        Try
            Return current_process.PeakWorkingSet64()
        Catch
            Return 0
        End Try
    End Function

    Public Function gc_total_memory() As Int64
        Return GC.GetTotalMemory(False)
    End Function
End Module
