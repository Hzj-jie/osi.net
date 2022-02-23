
Option Explicit On
Option Infer Off
Option Strict On

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

        Private Sub init()
            stopwatch.repeat(write_frequence_milliseconds, AddressOf write)
        End Sub

        Private Sub write(ByVal cr As counter_record)
            assert(cr IsNot Nothing)
            Dim startticks As Int64 = 0
            If envs.counter_selfhealth Then
                startticks = Now().Ticks()
            End If
            Dim snapshot As snapshot = Nothing
            snapshot = cr.snapshot()
            Dim msg As StringBuilder = Nothing
            msg = New StringBuilder()
            With cr
                If snapshot.count Then
                    msg.Append(patchup_counter_msg(.count_name, +snapshot.count))
                End If
                If snapshot.average Then
                    msg.Append(patchup_counter_msg(.average_name, +snapshot.average))
                End If
                If snapshot.last_average Then
                    msg.Append(patchup_counter_msg(.last_average_name, +snapshot.last_average))
                End If
                If snapshot.rate Then
                    msg.Append(patchup_counter_msg(.rate_name, +snapshot.rate))
                End If
                If snapshot.last_rate Then
                    msg.Append(patchup_counter_msg(.last_rate_name, +snapshot.last_rate))
                End If
            End With
            counter_distributor.distribute(startticks, Convert.ToString(msg))
        End Sub

        Private Sub write()
            If write_entry.mark_in_use() Then
                foreach(Sub(ByVal cr As counter_record)
                            assert(cr IsNot Nothing)
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
