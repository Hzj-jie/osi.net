
Imports osi.root.utt
Imports osi.root.connector

Public Class vector_perf
    Inherits performance_case_wrapper
    Private Const size_base As Int32 = 8290304

    Public Sub New()
        MyBase.New(New vector_test(False, size_base))
    End Sub

    Protected Overrides Function max_loops() As UInt64
        Return If(isreleasebuild(), 220000000000, 154000000000)
    End Function
End Class
