
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.ml.onebound(Of String)

Namespace onebound
    <test>
    Public NotInheritable Class selector_test
        <test>
        Private Shared Sub p0()
            Dim m As model = selector.exponential(New trainer().
                                                      accumulate("a", "b").
                                                      accumulate("a", "c").
                                                      accumulate("a", "c").
                                                      accumulate("a", "d").
                                                      accumulate("a", "d").
                                                      accumulate("a", "d").
                                                      dump(),
                                                  0)
            assertion.near_match(m.affinity("a", "b"), 0.3935)
            assertion.near_match(m.affinity("a", "c"), 0.6321)
            assertion.near_match(m.affinity("a", "d"), 0.7769)
        End Sub

        <test>
        Private Shared Sub p0_5()
            Dim m As model = selector.exponential(New trainer().
                                                      accumulate("a", "b").
                                                      accumulate("a", "c").
                                                      accumulate("a", "c").
                                                      accumulate("a", "d").
                                                      accumulate("a", "d").
                                                      accumulate("a", "d").
                                                      dump(),
                                                  0.5)
            assertion.near_match(m.affinity("a", "b"), 0)
            assertion.near_match(m.affinity("a", "c"), 0.6321)
            assertion.near_match(m.affinity("a", "d"), 0.7769)
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
