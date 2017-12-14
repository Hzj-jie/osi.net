
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Public NotInheritable Class mock_tick_clock
    Inherits tick_clock

    Private t As UInt64

    Public Sub New()
        MyBase.New()
        ' Consumers should not reply on a specific initial value of the tick_clock.
        t = rnd_uint64(max_uint16, max_uint32)
    End Sub

    Public Overrides Function ticks() As UInt64
        Return t
    End Function

    Public Sub set_ticks(ByVal t As UInt64)
        Me.t = t
    End Sub

    Public Sub set_milliseconds(ByVal ms As UInt64)
        set_ticks(milliseconds_to_ticks(ms))
    End Sub

    Public Sub set_seconds(ByVal s As UInt64)
        set_milliseconds(seconds_to_milliseconds(s))
    End Sub

    Public Sub advance_ticks(ByVal t As UInt64)
        Me.t += t
    End Sub

    Public Sub advance_milliseconds(ByVal ms As UInt64)
        advance_ticks(milliseconds_to_ticks(ms))
    End Sub

    Public Sub advance_seconds(ByVal s As UInt64)
        advance_milliseconds(seconds_to_milliseconds(s))
    End Sub

    Public Sub advance(ByVal ts As TimeSpan)
        Dim t As Int64 = 0
        t = ts.Ticks()
        ' Negative ticks (clock goes backward) is not allowed.
        assert(t >= 0)
        advance_milliseconds(CULng(t))
    End Sub
End Class
