
Imports osi.root.template
Imports osi.service.storage

Public Class istrkeyvt_test
    Inherits istrkeyvt_case

    Protected Sub New(ByVal i As iistrkeyvt_case)
        MyBase.New(i)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Function create_istrkeyvt() As istrkeyvt
        Return adapt(New memory())
    End Function
End Class

Public Class istrkeyvt_perf_test
    Inherits istrkeyvt_test

    Public Sub New()
        MyBase.New(New default_istrkeyvt_perf_case())
    End Sub
End Class