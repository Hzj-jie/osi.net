
Imports System.IO
Imports osi.service.storage
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.template

Public Class fces_test
    Inherits istrkeyvt_case

    Protected Sub New(ByVal i As iistrkeyvt_case)
        MyBase.New(i)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Function create_istrkeyvt(ByVal r As pointer(Of istrkeyvt)) As event_comb
        Return fces.ctor(r, virtdisk.memory_virtdisk(), virtdisk.memory_virtdisk())
    End Function
End Class

Public Class fces_perf_test
    Inherits fces_test

    Public Sub New()
        MyBase.New(New default_istrkeyvt_perf_case())
    End Sub
End Class