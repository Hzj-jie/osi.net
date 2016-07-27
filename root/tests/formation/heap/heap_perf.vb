
Imports osi.root.utt
Imports osi.root.connector

Public Class heap_perf
    Inherits performance_case_wrapper

    Private Shared Function round() As Int64
        Return (1000000 << (If(isreleasebuild(), 2, 0)))
    End Function

    Public Sub New()
        MyBase.New(repeat(New heap_case(False), round()))
    End Sub

    Protected Overrides Function max_loops() As UInt64
        Return If(isreleasebuild(), 28000000000, 2500000000)
    End Function
End Class
