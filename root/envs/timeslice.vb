
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Public Module _timeslice
    Public ReadOnly timeslice_length_ticks As Int64
    Public ReadOnly timeslice_length_ms As Int64
    Public ReadOnly half_timeslice_length_ticks As Int64
    Public ReadOnly half_timeslice_length_ms As Int64
    Public ReadOnly quarter_timeslice_length_ticks As Int64
    Public ReadOnly quarter_timeslice_length_ms As Int64
    Public ReadOnly eighth_timeslice_length_ticks As Int64
    Public ReadOnly eighth_timeslice_length_ms As Int64
    Public ReadOnly sixteenth_timeslice_length_ticks As Int64
    Public ReadOnly sixteenth_timeslice_length_ms As Int64
    Public ReadOnly two_timeslice_length_ticks As Int64
    Public ReadOnly two_timeslice_length_ms As Int64
    Public ReadOnly four_timeslice_length_ticks As Int64
    Public ReadOnly four_timeslice_length_ms As Int64
    Public ReadOnly eight_timeslice_length_ticks As Int64
    Public ReadOnly eight_timeslice_length_ms As Int64
    Public ReadOnly sixteen_timeslice_length_ticks As Int64
    Public ReadOnly sixteen_timeslice_length_ms As Int64

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

    Sub New()
        timeslice_length_ticks = decide_timeslice_length_ticks()
        half_timeslice_length_ticks = sr(1)
        quarter_timeslice_length_ticks = sr(2)
        eighth_timeslice_length_ticks = sr(3)
        sixteenth_timeslice_length_ticks = sr(4)
        two_timeslice_length_ticks = sl(1)
        four_timeslice_length_ticks = sl(2)
        eight_timeslice_length_ticks = sl(3)
        sixteen_timeslice_length_ticks = sl(4)

        timeslice_length_ms = to_ms(timeslice_length_ticks)
        half_timeslice_length_ms = to_ms(half_timeslice_length_ticks)
        quarter_timeslice_length_ms = to_ms(quarter_timeslice_length_ticks)
        eighth_timeslice_length_ms = to_ms(eighth_timeslice_length_ticks)
        sixteenth_timeslice_length_ms = to_ms(sixteenth_timeslice_length_ticks)
        two_timeslice_length_ms = to_ms(two_timeslice_length_ticks)
        four_timeslice_length_ms = to_ms(four_timeslice_length_ticks)
        eight_timeslice_length_ms = to_ms(eight_timeslice_length_ticks)
        sixteen_timeslice_length_ms = to_ms(sixteen_timeslice_length_ticks)

        ' This function is usually executed before global_init(), so using raise_error() does not output the result to
        ' file.
        If env_bool(env_keys("report", "timeslice", "length")) Then
            raise_error("timeslice length in ticks = ", timeslice_length_ticks)
        End If
    End Sub
End Module
