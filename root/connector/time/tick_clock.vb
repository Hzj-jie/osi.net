
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

' A class to retrieve ticks. The public functions should not be expected to return data which can be converted to a
' human readable format. All the public functions are thread-safe.
Partial Public Class tick_clock
    Public Shared ReadOnly low_resolution As tick_clock = New low_res_tick_clock()
    Public Shared ReadOnly normal_resolution As tick_clock = New normal_res_tick_clock()
    Public Shared ReadOnly high_resolution As tick_clock = New high_res_tick_clock()
    Public Shared ReadOnly [default] As tick_clock = low_resolution
    Private ReadOnly type_name As String

    Protected Sub New()
        type_name = Me.GetType().Name()
    End Sub

    Public Overridable Function ticks() As UInt64
        Return milliseconds_to_ticks(milliseconds())
    End Function

    Public Overridable Function milliseconds() As UInt64
        Return ticks_to_milliseconds(ticks())
    End Function

    Public Function milliseconds_l() As Int64
        Dim r As UInt64 = 0
        r = milliseconds()
        assert(r <= max_int64)
        Return CLng(r)
    End Function

    Public Function seconds() As UInt64
        Return milliseconds_to_seconds(milliseconds())
    End Function

    Public NotOverridable Overrides Function ToString() As String
        Return strcat(type_name, " - Ticks: ", ticks())
    End Function
End Class
