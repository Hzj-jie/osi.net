
Imports osi.root.connector
Imports osi.root.utt

Public Class rnd_perf
    Inherits performance_case_wrapper

    Public Sub New()
        MyBase.New(New rnd_test(), If(isreleasebuild(), 34000000000, 1800000000))
    End Sub
End Class
