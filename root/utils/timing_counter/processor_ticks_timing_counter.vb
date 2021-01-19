
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.envs

Public Class processor_ticks_timing_counter
    Inherits timing_counter

    Public Sub New(ByVal p As ref(Of Int64))
        MyBase.New(p)
    End Sub

    Protected Overrides Function now() As Int64
        Return total_processor_time().Ticks()
    End Function
End Class
