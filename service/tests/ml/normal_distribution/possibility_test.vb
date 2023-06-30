
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports nd = osi.service.ml.normal_distribution

Namespace normal_distribution
    <test>
    Public NotInheritable Class possibility_test
        <test>
        Private Shared Sub case1()
            Dim n As nd = Nothing
            n = New nd(0, 1)
            assertions.of(n.cumulative_distribute(0)).in_range(0.5, 0.001)
            assertions.of(n.cumulative_distribute(1)).in_range(0.841, 0.001)
            assertions.of(n.cumulative_distribute(-1)).in_range(0.159, 0.001)
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
