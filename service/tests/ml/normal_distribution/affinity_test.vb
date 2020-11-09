﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.ml
Imports nd = osi.service.ml.normal_distribution

Namespace normal_distribution
    '<test>
    Public NotInheritable Class affinity_test
        '<test>
        Private Shared Sub case1()
            assertion.equal(affinity.of(New nd(1, 0.5), New nd(1, 0.5)), 0)
        End Sub

        '<test>
        Private Shared Sub case2()
            assertions.of(affinity.of(New nd(2, 1), New nd(1, 0.5))).in_range(0)
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace

