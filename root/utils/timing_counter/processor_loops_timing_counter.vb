
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.envs

Public Class processor_loops_timing_counter
    Inherits scale_timing_counter

    Public Sub New(ByVal p As ref(Of Int64))
        MyBase.New(p, loops_per_ms)
    End Sub

    Protected Overrides Function unscaled_now() As Int64
        Return total_processor_time_ms()
    End Function
End Class
