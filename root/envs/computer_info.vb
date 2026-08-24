
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector

Public Module _computer_info
    Public ReadOnly processor_architecture As String = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")

#If Not NET8_0_OR_GREATER Then
    ' Microsoft.VisualBasic.Devices.ComputerInfo is not thread-safe: its internal
    ' InternalMemoryStatus uses a shared mutable MEMORYSTATUSEX struct field that races
    ' and throws Win32Exception ("Could not obtain memory information...") under concurrent queries.
    ' Furthermore, querying ComputerInfo incurs Win32 GlobalMemoryStatusEx syscalls and kernel lock
    ' contention. We cache the memory values with a 1-second TTL using atomic operations to guarantee
    ' thread safety and drastically improve performance under heavy concurrent usage.
    Private last_refresh_ms As Int64 = 0
    Private updating As Int32 = 0
    Private cached_avail_phys As Int64 = 0
    Private cached_avail_virt As Int64 = 0
    Private cached_total_phys As Int64 = 0
    Private cached_total_virt As Int64 = 0

    Private Sub refresh()
        Dim now_ms As Int64 = nowadays.milliseconds()
        Dim last_ms As Int64 = Interlocked.Read(last_refresh_ms)
        If last_ms > 0 AndAlso now_ms - last_ms < 1000 Then
            Return
        End If
        If Interlocked.CompareExchange(updating, 1, 0) <> 0 Then
            Return
        End If
        Try
            Dim ap As UInt64 = computer.Info().AvailablePhysicalMemory()
            Interlocked.Exchange(cached_avail_phys, CLng(ap))
        Catch
        End Try
        Try
            Dim av As UInt64 = computer.Info().AvailableVirtualMemory()
            Interlocked.Exchange(cached_avail_virt, CLng(av))
        Catch
        End Try
        Try
            Dim tp As UInt64 = computer.Info().TotalPhysicalMemory()
            Interlocked.Exchange(cached_total_phys, CLng(tp))
        Catch
        End Try
        Try
            Dim tv As UInt64 = computer.Info().TotalVirtualMemory()
            Interlocked.Exchange(cached_total_virt, CLng(tv))
        Catch
        End Try
        Interlocked.Exchange(last_refresh_ms, now_ms)
        Interlocked.Exchange(updating, 0)
    End Sub

    Public Function available_physical_memory() As UInt64
        refresh()
        Return CULng(Interlocked.Read(cached_avail_phys))
    End Function

    Public Function available_virtual_memory() As UInt64
        refresh()
        Return CULng(Interlocked.Read(cached_avail_virt))
    End Function

    Public Function total_physical_memory() As UInt64
        refresh()
        Return CULng(Interlocked.Read(cached_total_phys))
    End Function

    Public Function total_virtual_memory() As UInt64
        refresh()
        Return CULng(Interlocked.Read(cached_total_virt))
    End Function
#Else
    Public Function available_physical_memory() As UInt64
        Dim gc_info As GCMemoryInfo = GC.GetGCMemoryInfo()
        Return CULng(Math.Max(0L, gc_info.TotalAvailableMemoryBytes))
    End Function

    Public Function available_virtual_memory() As UInt64
        Return available_physical_memory()
    End Function

    Public Function total_physical_memory() As UInt64
        Dim gc_info As GCMemoryInfo = GC.GetGCMemoryInfo()
        Return CULng(Math.Max(0L, gc_info.TotalAvailableMemoryBytes))
    End Function

    Public Function total_virtual_memory() As UInt64
        Return total_physical_memory()
    End Function
#End If
End Module
