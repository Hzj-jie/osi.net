
Imports osi.root.connector
Imports osi.root.formation

Public Class hires_ticks_timing_counter
    Inherits timing_counter

    Public Sub New(ByVal p As pointer(Of Int64))
        MyBase.New(p)
    End Sub

    Protected Overrides Function now() As Int64
        Return nowadays.high_res_ticks()
    End Function
End Class
