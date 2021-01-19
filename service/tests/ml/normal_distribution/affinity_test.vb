
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.ml
Imports nd = osi.service.ml.normal_distribution

Namespace normal_distribution
    <test>
    Public NotInheritable Class affinity_test
        <test>
        Private Shared Sub case1()
            assertion.equal(affinity.of(New nd(1, 0.5), New nd(1, 0.5)), 0)
        End Sub

        <test>
        Private Shared Sub case2()
            assertions.of(affinity.of(New nd(2, 1), New nd(1, 0.5))).in_range(0.00128538448296893)
        End Sub

        <test>
        Private Shared Sub case3()
            assertions.of(affinity.of(New nd(100, 1), New nd(-100, 1))).in_range(0.00169252882322651)
        End Sub

        <test>
        Private Shared Sub case4()
            assertions.of(affinity.of(New nd(0, 1), New nd(0, 1.01))).in_range(0)
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace

