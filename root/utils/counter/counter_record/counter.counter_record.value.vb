
Option Explicit On
Option Infer Off
Option Strict On

Imports System.DateTime
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation

Namespace counter
    Partial Friend Class counter_record
        Private Shared Sub try_inc(ByRef i As Int64, ByVal j As Int64, ByVal name As String)
            If Not connector.try_inc(i, j) Then
                raise_error(error_type.warning, "overflow for count ", name, ", the value may not correct.")
            End If
        End Sub

        Public Function average() As Int64
            If calltimes > 0 Then
                Return count \ calltimes
            Else
                Return npos
            End If
        End Function

        Private Function last_average_count() As Int64
            If debug_assert(Not last_averages Is Nothing AndAlso last_averages.Length() > 0,
                           "last_averages is not initialized, the counter may not enabled last_average.") Then
                Dim c As Int64 = 0
                For i As Int32 = 0 To CInt(min(last_averages.Length(), calltimes)) - 1
                    try_inc(c, last_averages(i), name)
                Next
                Return c
            Else
                Return npos
            End If
        End Function

        Private Function last_averages_length() As Int64
            Return max(min(last_averages.Length(), calltimes), 1)
        End Function

        Public Function last_average() As Int64
            Return last_average_count() \ last_averages_length()
        End Function

        Private Function time_interval(ByVal start As Int64) As Double
            Dim t As Double
            t = (Now().Ticks() - start) / milli_tick / second_milli / interval_scale
            Return If(t <= 0, 1, t)
        End Function

        Public Function rate() As Int64
            Return CLng(count / time_interval(register_time_ticks))
        End Function

        Public Function last_rate() As Int64
            If debug_assert(Not last_times_ticks Is Nothing AndAlso last_times_ticks.Length() > 0,
                           "last_times_ticks is not initialized, the counter may not enabled last_rate.") Then
                Dim c As Int64 = 0
                Dim oldest As Int64 = 0
                oldest = max_int64
                For i As Int32 = 0 To CInt(min(calltimes, last_times_ticks.Length())) - 1
                    c = last_times_ticks(i)
                    If c > 0 Then
                        If oldest > c Then
                            oldest = c
                        End If
                    End If
                Next
                If oldest < max_int64 Then
                    Return CLng(last_average_count() / time_interval(oldest))
                Else
                    Return 0
                End If
            Else
                Return npos
            End If
        End Function

        Public Function snapshot() As snapshot
            Dim count As constant(Of Int64) = Nothing
            Dim average As constant(Of Int64) = Nothing
            Dim last_average As constant(Of Int64) = Nothing
            Dim rate As constant(Of Int64) = Nothing
            Dim last_rate As constant(Of Int64) = Nothing
            With Me
                If count_selected() Then
                    count = constant.[New](.count)
                End If
                If average_selected() Then
                    average = constant.[New](.average())
                End If
                If last_average_selected() Then
                    last_average = constant.[New](.last_average())
                End If
                If rate_selected() Then
                    rate = constant.[New](.rate())
                End If
                If last_rate_selected() Then
                    last_rate = constant.[New](.last_rate())
                End If
            End With
            Return New snapshot(name, count, average, last_average, rate, last_rate)
        End Function
    End Class
End Namespace
