
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public NotInheritable Class mock_tick_clock
    Inherits tick_clock

    Private t As UInt64

    Public Sub New()
        MyBase.New()
        t = 0
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

    Public Sub add_ticks(ByVal t As UInt64)
        Me.t += t
    End Sub

    Public Sub add_milliseconds(ByVal ms As UInt64)
        add_ticks(milliseconds_to_ticks(ms))
    End Sub

    Public Sub add_seconds(ByVal s As UInt64)
        add_milliseconds(seconds_to_milliseconds(s))
    End Sub
End Class
