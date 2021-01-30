
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

' TODO: Constants here should be uint32.
<global_init(global_init_level.max)>
Public Module _timeslice
    Public ReadOnly timeslice_length_ticks As Int64 = decide_timeslice_length_ticks()
    Public ReadOnly timeslice_length_ms As Int64 = to_ms(timeslice_length_ticks)
    Public ReadOnly half_timeslice_length_ticks As Int64 = sr(1)
    Public ReadOnly half_timeslice_length_ms As Int64 = to_ms(half_timeslice_length_ticks)
    Public ReadOnly quarter_timeslice_length_ticks As Int64 = sr(2)
    Public ReadOnly quarter_timeslice_length_ms As Int64 = to_ms(quarter_timeslice_length_ticks)
    Public ReadOnly eighth_timeslice_length_ticks As Int64 = sr(3)
    Public ReadOnly eighth_timeslice_length_ms As Int64 = to_ms(eighth_timeslice_length_ticks)
    Public ReadOnly sixteenth_timeslice_length_ticks As Int64 = sr(4)
    Public ReadOnly sixteenth_timeslice_length_ms As Int64 = to_ms(sixteenth_timeslice_length_ticks)
    Public ReadOnly two_timeslice_length_ticks As Int64 = sl(1)
    Public ReadOnly two_timeslice_length_ms As Int64 = to_ms(two_timeslice_length_ticks)
    Public ReadOnly four_timeslice_length_ticks As Int64 = sl(2)
    Public ReadOnly four_timeslice_length_ms As Int64 = to_ms(four_timeslice_length_ticks)
    Public ReadOnly eight_timeslice_length_ticks As Int64 = sl(3)
    Public ReadOnly eight_timeslice_length_ms As Int64 = to_ms(eight_timeslice_length_ticks)
    Public ReadOnly sixteen_timeslice_length_ticks As Int64 = sl(4)
    Public ReadOnly sixteen_timeslice_length_ms As Int64 = to_ms(sixteen_timeslice_length_ticks)

    Private Function to_ms(ByVal ticks As Int64) As Int64
        Return max(ticks_to_milliseconds(ticks), 1)
    End Function

    Private Function sl(ByVal bits As Byte) As Int64
        assert(timeslice_length_ticks <> 0)
        Return timeslice_length_ticks << bits
    End Function

    Private Function sr(ByVal bits As Byte) As Int64
        assert(timeslice_length_ticks <> 0)
        Return max(timeslice_length_ticks >> bits, 1)
    End Function

    Private Function decide_timeslice_length_ticks_sleep_ticks(ByVal thisRound As Int64) As Int64
        Return thisRound * 15 \ 16
    End Function

    Private Function decide_timeslice_length_ticks() As Int64
        Dim timeslice_length_ticks As Int64 = 0
        Const count As Int32 = 8
        Dim this_round As Int64 = 0
        this_round = default_timeslice_length_ticks
        For i As Int32 = 0 To count - 1
            sleep(0)
            Dim startticks As Int64 = 0
            startticks = nowadays.high_res_ticks()
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
