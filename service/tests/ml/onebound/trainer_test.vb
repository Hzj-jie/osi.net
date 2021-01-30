
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.ml.onebound(Of String)

Namespace onebound
    <test>
    Public NotInheritable Class trainer_test
        <test>
        Private Shared Sub case1()
            Dim m As model = Nothing
            m = New trainer().accumulate("a", "b").dump()
            assertion.equal(m.affinity("a", "b"), 1.0)
            assertion.equal(m.affinity("b", "a"), 0.0)
        End Sub

        <test>
        Private Shared Sub case2()
            Dim m As model = Nothing
            m = New trainer().
                        accumulate("a", "b").
                        accumulate("a", "c").
                        accumulate("b", "c").
                        dump()
            assertion.equal(m.affinity("a", "b"), 1.0)
            assertion.equal(m.affinity("a", "c"), 1.0)
            assertion.equal(m.affinity("b", "c"), 1.0)
        End Sub

        <test>
        Private Shared Sub case3()
            Dim m As model = Nothing
            m = New trainer(New trainer.config() With {.compare = trainer.config.comparison.with_average}).
                        accumulate("a", "b", 2).
                        accumulate("a", "c").
                        dump()
            assertion.equal(m.affinity("a", "b"), 4 / 3)
            assertion.equal(m.affinity("a", "c"), 2 / 3)
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
