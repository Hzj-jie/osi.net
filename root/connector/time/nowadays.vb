
Option Explicit On
Option Infer Off
Option Strict On

Imports System.DateTime
Imports osi.root.constants

' The following functions all return Int64 to make sure, we will never fail when
' nowadays.milliseconds() - nowadays.milliseconds()
' The timestamp relies on hardware or external software, we cannot guarantee it won't return a smaller number in a later
' call.
' http://stackoverflow.com/questions/1008345/system-diagnostics-stopwatch-returns-negative-numbers-in-elapsed-properties
' TODO: low_res & high_res or at least their tests are not stable on one core machine.
<global_init(global_init_level.foundamental)>
Public NotInheritable Class nowadays
    Private Shared Sub init()
        high_res_ticks()
        normal_res_ticks()
        low_res_ticks()
    End Sub

    Public Shared Function high_res_ticks() As Int64
        Return high_res_ticks_retriever.ticks()
    End Function

    Public Shared Function strong_high_res_ticks() As Int64
        Return strong_high_res_ticks_retriever.ticks()
    End Function

    Public Shared Function high_res_milliseconds() As Int64
        Return ticks_to_milliseconds(high_res_ticks())
    End Function

    Public Shared Function strong_high_res_milliseconds() As Int64
        Return ticks_to_milliseconds(strong_high_res_ticks())
    End Function

    Public Shared Function normal_res_ticks() As Int64
        Return Now().Ticks()
    End Function

    Public Shared Function normal_res_milliseconds() As Int64
        Return ticks_to_milliseconds(normal_res_ticks())
    End Function

    Public Shared Function low_res_ticks() As Int64
        Return milliseconds_to_ticks(low_res_milliseconds())
    End Function

    Public Shared Function low_res_milliseconds() As Int64
        Return low_res_ticks_retriever.milliseconds()
    End Function

    Public Shared Function milliseconds() As Int64
        Return low_res_milliseconds()
    End Function

    Public Shared Function ticks() As Int64
        Return low_res_ticks()
    End Function

    Public Shared Function seconds() As Int64
        Return milliseconds_to_seconds(milliseconds())
    End Function
End Class
