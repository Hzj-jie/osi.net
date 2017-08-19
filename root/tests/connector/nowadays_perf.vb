
Imports System.DateTime
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.utt

Public Class nowadays_perf
    Inherits [case]

    Private Const size As Int64 = 16 * 1024 * 1024

    Private Shared Sub now_perf(ByRef ms As Int64, ByRef pms As Int64)
        Dim m As pointer(Of Int64) = Nothing
        m = New pointer(Of Int64)()
        Dim pm As pointer(Of Int64) = Nothing
        pm = New pointer(Of Int64)()
        Using New ms_timing_counter(m)
            Using New processor_ms_timing_counter(pm)
                For i As Int64 = 0 To size - 1
                    Dim t As Int64 = 0
                    t = Now().milliseconds()
                Next
            End Using
        End Using
        ms = +m
        pms = +pm
    End Sub

    Private Shared Sub nowadays_perf(ByRef ms As Int64, ByRef pms As Int64)
        Dim m As pointer(Of Int64) = Nothing
        m = New pointer(Of Int64)()
        Dim pm As pointer(Of Int64) = Nothing
        pm = New pointer(Of Int64)()
        Using New ms_timing_counter(m)
            Using New processor_ms_timing_counter(pm)
                For i As Int64 = 0 To size - 1
                    Dim t As Int64 = 0
                    t = nowadays.milliseconds()
                Next
            End Using
        End Using
        ms = +m
        pms = +pm
    End Sub

    Public Overrides Function run() As Boolean
        Dim nms As Int64 = 0
        Dim npms As Int64 = 0
        Dim ams As Int64 = 0
        Dim apms As Int64 = 0
        Using New realtime()
            now_perf(nms, npms)
            nowadays_perf(ams, apms)
        End Using
        'the performance of nowadays is 15 times faster than Now() on some machines,
        'but just 1.2 times on some others, which is wired.
        'the OS version or .net version are the same.
        assert_less_or_equal(ams * 8, nms)
        assert_less_or_equal(apms * 8, npms)
        Return True
    End Function

    Public Overrides Function reserved_processors() As Int16
        Return Environment.ProcessorCount()
    End Function
End Class
