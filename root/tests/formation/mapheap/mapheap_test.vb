
Imports osi.root.utt
Imports osi.root.connector

Public Class mapheap_test
    Inherits repeat_case_wrapper

    Private Shared ReadOnly round As Int64

    Shared Sub New()
        round = 1000000 << (If(isreleasebuild(), 2, 0))
    End Sub

    Public Sub New()
        MyBase.New(New mapheap_case(True), round)
    End Sub
End Class
