
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.ml
Imports bd = osi.service.ml.bernoulli_distribution

Namespace bernoulli_distribution
    <test>
    Public NotInheritable Class affinity_test
        <test>
        Private Shared Sub case1()
            assertion.equal(affinity.of(New bd(0.5), New bd(0.5)), 0)
        End Sub

        <test>
        Private Shared Sub case2()
            assertions.of(affinity.of(New bd(0.2), New bd(0.8))).in_range(0.72)
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
