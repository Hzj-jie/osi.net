
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports bd = osi.service.ml.bernoulli_distribution

Namespace bernoulli_distribution
    <test>
    Public NotInheritable Class estimator_estimate_test
        <test>
        Private Shared Sub case1()
            Dim n As bd = New bd(0.2)
            Dim samples() As Boolean = streams.range(0, 100).
                                               map(Function(ByVal i As Int32) As Boolean
                                                       Return i < 20
                                                   End Function).
                                               to_array()
            assertion.is_true(n.near_match(bd.estimator.estimate(samples)))
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
