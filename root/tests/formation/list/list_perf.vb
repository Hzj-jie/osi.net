
Imports osi.root.connector
Imports osi.root.utt

Public Class list_perf
    Inherits performance_case_wrapper

    Private Shared Function round() As Int64
        Return (100000 << (If(isreleasebuild(), 2, 0)))
    End Function

    Public Sub New()
        MyBase.New(repeat(New list_case(False), round()))
    End Sub

    Protected Overrides Function max_loops() As UInt64
        Return If(isreleasebuild(), 55000000000, 22000000000)
    End Function
End Class
