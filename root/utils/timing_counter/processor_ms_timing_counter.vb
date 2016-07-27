
Imports osi.root.formation
Imports osi.root.envs

Public Class processor_ms_timing_counter
    Inherits timing_counter

    Public Sub New(ByVal p As pointer(Of Int64))
        MyBase.New(p)
    End Sub

    Protected Overrides Function now() As Int64
        Return total_processor_time_ms()
    End Function
End Class
