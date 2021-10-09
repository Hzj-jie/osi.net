
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

' TODO: Constants here should be uint32.
<global_init(global_init_level.other)>
Public Module _timeslice
    Public ReadOnly timeslice_length_ticks As Int64 = decide_timeslice_length_ticks()
    Public ReadOnly timeslice_length_ms As Int64 = max(timeslice_length_ticks.ticks_to_milliseconds(), 1)

    Private Function decide_timeslice_length_ticks_sleep_ticks(ByVal this_round As Int64) As Int64
        Return this_round * 15 \ 16
    End Function

    Private Function decide_timeslice_length_ticks() As Int64
        Dim timeslice_length_ticks As Int64 = 0
        Const count As Int32 = 8
        Dim this_round As Int64 = default_timeslice_length_ticks
        For i As Int32 = 0 To count - 1
            sleep(0)
            Dim startticks As Int64 = nowadays.high_res_ticks()
            sleep_ticks(decide_timeslice_length_ticks_sleep_ticks(this_round))
            this_round = (nowadays.high_res_ticks() - startticks + this_round) >> 1
            timeslice_length_ticks += this_round
        Next
        timeslice_length_ticks \= count
        If timeslice_length_ticks <= milliseconds_to_ticks(1) Then
            timeslice_length_ticks = default_timeslice_length_ticks
            raise_error("cannot determined length of timeslice, use default value as ",
                          ticks_to_milliseconds(default_timeslice_length_ticks), "ms.")
        End If

        Return timeslice_length_ticks
    End Function

    Private Sub init()
        If env_bool(env_keys("report", "timeslice", "length")) Then
            raise_error("timeslice length in ticks = ", timeslice_length_ticks)
        End If
    End Sub
End Module
