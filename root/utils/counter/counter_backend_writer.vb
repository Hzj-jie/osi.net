
Imports System.DateTime
Imports System.Text
Imports osi.root.connector
Imports osi.root.constants.counter
Imports osi.root.lock
Imports osi.root.constants

Namespace counter
    <global_init(global_init_level.log_and_counter_services)>
    Friend Module _counter_backend_writer
        Private write_entry As singleentry

        Private Function patchup_counter_msg(ByVal name As String, ByVal count As Int64) As String
            Return strcat(short_time(), separator, name, separator, Convert.ToString(count), newline.incode())
        End Function

        Sub New()
            stopwatch.repeat(write_frequence_milliseconds, AddressOf write)
        End Sub

        Private Sub init()
        End Sub

        Private Sub write(ByVal cr As counter_record)
            assert(Not cr Is Nothing)
            Dim startticks As Int64 = 0
            If envs.counter_selfhealth Then
                startticks = Now().Ticks()
            End If
            Dim count As Int64? = 0
            Dim average As Int64? = 0
            Dim last_average As Int64? = 0
            Dim rate As Int64? = 0
            Dim last_rate As Int64? = 0
            Dim msg As StringBuilder = Nothing
            msg = New StringBuilder()
            assert(cr.value(Nothing, count, average, last_average, rate, last_rate))
            With cr
                If Not count Is Nothing Then
                    msg.Append(patchup_counter_msg(.count_name, count))
                End If
                If Not average Is Nothing Then
                    msg.Append(patchup_counter_msg(.average_name, average))
                End If
                If Not last_average Is Nothing Then
                    msg.Append(patchup_counter_msg(.last_average_name, last_average))
                End If
                If Not rate Is Nothing Then
                    msg.Append(patchup_counter_msg(.rate_name, rate))
                End If
                If Not last_rate Is Nothing Then
                    msg.Append(patchup_counter_msg(.last_rate_name, last_rate))
                End If
            End With
            counter_distributor.distribute(startticks, Convert.ToString(msg))
        End Sub

        Private Sub write()
            If write_entry.mark_in_use() Then
                foreach(Sub(ByRef cr As counter_record)
                            assert(Not cr Is Nothing)
                            If cr.has_value() Then
                                write(cr)
                            End If
                        End Sub)

                Dim startticks As Int64 = 0
                If envs.counter_selfhealth Then
                    startticks = Now().Ticks()
                End If
                Dim msg As StringBuilder = Nothing
                msg = New StringBuilder()
                For i As Int32 = 0 To internal_counters.Length() - 1
                    msg.Append(patchup_counter_msg(internal_counters(i).name, internal_counters(i).value()))
                Next
                counter_distributor.distribute(startticks, Convert.ToString(msg))
                write_entry.release()
            End If
        End Sub
    End Module
End Namespace
