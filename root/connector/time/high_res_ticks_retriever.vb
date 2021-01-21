
Option Explicit On
Option Infer Off
Option Strict On

Imports System.DateTime
Imports System.Diagnostics
Imports System.Threading
Imports osi.root.constants

Public NotInheritable Class high_res_ticks_retriever
    Private Const revise_interval_ticks As Int64 = CLng(15) * minute_second * second_milli * milli_tick
    Private Shared ReadOnly perf_freq As Double = calculate_pref_freq()
    Private Shared distance As Int64
    Private Shared last_revise_ticks As Int64

    Private Shared Function calculate_pref_freq() As Double
        Dim perf_freq As Double = 0
        If Stopwatch.IsHighResolution Then
            perf_freq = Stopwatch.Frequency
            perf_freq /= milli_tick
            perf_freq /= second_milli
            assert(perf_freq <> 0)
        Else
            perf_freq = 0
            raise_error(error_type.system, "high-resolution performance counter is not supported.")
        End If
        Return perf_freq
    End Function

    'force revise the distance during next high_res_ticks() call.
    Public Shared Sub force_revise()
        distance = 0
    End Sub

    Public Shared Function ticks() As Int64
        If Not Stopwatch.IsHighResolution Then
            Return Now().Ticks()
        End If
        Dim c As Int64 = 0
        c = qpc()
        'looks like the QueryPerformanceCounter is not consistent with Now().Ticks()
        If distance = 0 OrElse c - last_revise_ticks >= revise_interval_ticks Then
            For i As Int32 = 0 To 1
                Dim this As Int64 = 0
                this = (((Now().Ticks() - qpc()) -
                             (qpc() - Now().Ticks())) >> 1)
                assert(this > 0)
                If i = 0 OrElse this < distance Then
                    'for 32 bit processors
                    Interlocked.Exchange(distance, this)
                End If
            Next
            c = qpc()
            last_revise_ticks = c
        End If
        assert(distance > 0)
        Return c + distance
    End Function

    Public Shared Function query_performance_count() As Int64
        If Stopwatch.IsHighResolution Then
            Return qpc()
        End If
        Return Now().Ticks()
    End Function

    Private Shared Function qpc() As Int64
        assert(perf_freq > 0)
        Return CLng(Stopwatch.GetTimestamp() / perf_freq)
    End Function

    Private Sub New()
    End Sub
End Class
