
Imports System.DateTime
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.constants
Imports osi.root.constants.counter
Imports System.Threading

Namespace counter
    Partial Friend Class counter_record
        Private Function hit_sample() As Boolean
            Return sample_rate = 1 OrElse rnd(0, 1) < sample_rate
        End Function

        Private Sub force_increase(ByVal v As Int64)
            Dim startticks As Int64 = 0
            If envs.counter_selfhealth Then
                startticks = Now().Ticks()
            End If
            Dim index As Int64 = 0
            index = Interlocked.Increment(calltimes) - 1
            If count_selected() OrElse
               average_selected() OrElse
               rate_selected() Then
                Interlocked.Add(count, v)
            End If
            If last_average_selected() OrElse
               last_rate_selected() Then
                last_averages(index Mod last_averages.Length()) = v
            End If
            If last_rate_selected() Then
                If Not envs.counter_selfhealth Then
                    startticks = Now().Ticks()
                End If
                last_times_ticks(index Mod last_times_ticks.Length()) = startticks
            End If
            If envs.counter_selfhealth Then
                selfhealth.record_increase_latency(startticks)
            End If
        End Sub

        Public Function increase(ByVal v As Int64) As Boolean
            If hit_sample() Then
                force_increase(v)
                Return True
            Else
                Return False
            End If
        End Function

        Public Function increase(ByVal dc As lazier(Of Int64)) As Boolean
            If hit_sample() Then
                force_increase(+dc)
                Return True
            Else
                Return False
            End If
        End Function
    End Class
End Namespace
