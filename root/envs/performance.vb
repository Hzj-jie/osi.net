
Option Explicit On
Option Infer Off
Option Strict On

Imports System.DateTime
Imports osi.root.connector

Public Module _performance
    Private Sub refresh_process_status()
        this_process.ref.Refresh()
    End Sub

    Private Function processor_usage(ByVal usage As Int64,
                                     ByVal last_usage As Int64,
                                     ByVal now_ticks As Int64,
                                     ByVal last_ticks As Int64) As Double
        Return (usage - last_usage) * 100 / If(now_ticks = last_ticks, 1, now_ticks - last_ticks)
    End Function

    Public Class recent_processor_usage_record
        Public Shared Function [New]() As recent_processor_usage_record
            Return New recent_processor_usage_record_implementation()
        End Function

        Protected Sub New()
        End Sub
    End Class

    Private NotInheritable Class recent_processor_usage_record_implementation
        Inherits recent_processor_usage_record

        Public last_usage_ticks As Int64 = 0
        Public last_ticks As Int64 = 0
    End Class

    Private Function recent_processor_usage(ByVal impl As recent_processor_usage_record_implementation) As Double
        assert(Not impl Is Nothing)
        Dim ticks As Int64 = 0
        Dim usage As Int64 = 0
        ticks = Now().Ticks()
        usage = total_processor_time().Ticks()
        If impl.last_ticks = 0 Then
            impl.last_ticks = this_process.ref.StartTime().Ticks()
        End If
        Dim rtn As Double = 0
        rtn = processor_usage(usage, impl.last_usage_ticks, ticks, impl.last_ticks)
        impl.last_usage_ticks = usage
        impl.last_ticks = ticks
        Return rtn
    End Function

    Private ReadOnly default_recent_processor_usage_record As New recent_processor_usage_record_implementation()

    ' Return the processor neutral usage between now and the timestamp when last recent_processor_usage() is called.
    ' The resource usage does not depend on how many processors are installed on the system. The range of the return of
    ' this function is [0, 100 * number-of-processors].
    Public Function recent_processor_usage() As Double
        Return recent_processor_usage(default_recent_processor_usage_record)
    End Function

    Public Function recent_processor_usage(ByVal record As recent_processor_usage_record) As Double
        Return recent_processor_usage(direct_cast(Of recent_processor_usage_record_implementation)(record))
    End Function

    ' Return the processor neutral usage. I.e. the resource usage no matter how many processors are installed on the
    ' system. The range of the return of this function is [0, 100 * number-of-processors].
    Public Function processor_usage() As Double
        Return processor_usage(total_processor_time().Ticks(), 0,
                               Now().Ticks(), this_process.ref.StartTime().Ticks())
    End Function

    Public Function recent_processor_usage_percentage() As Double
        Return recent_processor_usage() / Environment.ProcessorCount()
    End Function

    Public Function recent_processor_usage_percentage(ByVal record As recent_processor_usage_record) As Double
        Return recent_processor_usage(record) / Environment.ProcessorCount()
    End Function

    Public Function processor_usage_percentage() As Double
        Return processor_usage() / Environment.ProcessorCount()
    End Function

    Public Function total_processor_time() As TimeSpan
        refresh_process_status()
        Try
            Return this_process.ref.TotalProcessorTime()
        Catch
            Return Nothing
        End Try
    End Function

    Public Function total_processor_time_ms() As Int64
        Return CLng(total_processor_time().TotalMilliseconds())
    End Function

    Public Function private_bytes_usage() As Int64
        refresh_process_status()
        Try
            Return this_process.ref.PrivateMemorySize64()
        Catch
            Return 0
        End Try
    End Function

    Public Function virtual_bytes_usage() As Int64
        refresh_process_status()
        Try
            Return this_process.ref.VirtualMemorySize64()
        Catch
            Return 0
        End Try
    End Function

    Public Function workingset_bytes_usage() As Int64
        refresh_process_status()
        Try
            Return this_process.ref.WorkingSet64()
        Catch
            Return 0
        End Try
    End Function

    Public Function min_workingset_bytes_usage() As Int64
        refresh_process_status()
        Try
            Return this_process.ref.MinWorkingSet().ToInt64()
        Catch
            Return 0
        End Try
    End Function

    Public Function max_workingset_bytes_usage() As Int64
        refresh_process_status()
        Try
            Return this_process.ref.MaxWorkingSet().ToInt64()
        Catch
            Return 0
        End Try
    End Function

    Public Function nonpaged_system_memory_bytes_usage() As Int64
        refresh_process_status()
        Try
            Return this_process.ref.NonpagedSystemMemorySize64()
        Catch
            Return 0
        End Try
    End Function

    Public Function paged_memory_bytes_usage() As Int64
        refresh_process_status()
        Try
            Return this_process.ref.PagedMemorySize64()
        Catch
            Return 0
        End Try
    End Function

    Public Function paged_system_memory_bytes_usage() As Int64
        refresh_process_status()
        Try
            Return this_process.ref.PagedSystemMemorySize64()
        Catch
            Return 0
        End Try
    End Function

    Public Function peak_paged_memory_bytes_usage() As Int64
        refresh_process_status()
        Try
            Return this_process.ref.PeakPagedMemorySize64()
        Catch
            Return 0
        End Try
    End Function

    Public Function peak_virtual_memory_bytes_usage() As Int64
        refresh_process_status()
        Try
            Return this_process.ref.PeakVirtualMemorySize64()
        Catch
            Return 0
        End Try
    End Function

    Public Function peak_workingset_bytes_usage() As Int64
        refresh_process_status()
        Try
            Return this_process.ref.PeakWorkingSet64()
        Catch
            Return 0
        End Try
    End Function

    Public Function gc_total_memory() As Int64
        Return GC.GetTotalMemory(False)
    End Function
End Module
