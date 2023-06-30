
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.ml.onebound(Of Char)

Namespace onebound
    <test>
    Public NotInheritable Class evaluator_test
        <test>
        Private Shared Sub evaluate()
            Dim e As evaluator = Nothing
            e = New evaluator(New trainer().
                                   accumulate("a"c, "n"c).
                                   accumulate("a"c, "p"c).
                                   accumulate("p"c, "p"c).
                                   accumulate("p"c, "l"c).
                                   accumulate("l"c, "e"c).
                                   dump())
            assertion.equal(e(vector.of("anapple".c_str())),
                            vector.of(vector.of("an".c_str()), vector.of("apple".c_str())))
        End Sub

        <test>
        Private Shared Sub evaluate2()
            Dim e As evaluator = Nothing
            e = New evaluator(New trainer().
                                   accumulate("a"c, "n"c).
                                   accumulate("a"c, "p"c).
                                   accumulate("p"c, "p"c).
                                   accumulate("p"c, "l"c).
                                   accumulate("l"c, "e"c).
                                   accumulate("a"c, "p"c).
                                   accumulate("p"c, "p"c).
                                   accumulate("p"c, "l"c).
                                   accumulate("l"c, "e"c).
                                   accumulate("a"c, "n"c).
                                   accumulate("n"c, "d"c).
                                   accumulate("a"c, "n"c).
                                   accumulate("o"c, "r"c).
                                   accumulate("r"c, "a"c).
                                   accumulate("a"c, "n"c).
                                   accumulate("n"c, "g"c).
                                   accumulate("g"c, "e"c).
                                   dump())
            assertion.equal(e(vector.of("anappleandanorange".c_str())),
                            vector.of(vector.of("an".c_str()),
                                      vector.of("apple".c_str()),
                                      vector.of("and".c_str()),
                                      vector.of("an".c_str()),
                                      vector.of("orange".c_str())))
        End Sub

        <test>
        Private Shared Sub evaluate3()
            Dim e As evaluator = Nothing
            e = New evaluator(New trainer().
                                   accumulate("a"c, "n"c).
                                   accumulate("a"c, "n"c).
                                   accumulate("n"c, "d"c).
                                   dump())
            assertion.equal(e(vector.of("an".c_str())),
                            vector.of(vector.of("an".c_str())))
            assertion.equal(e(vector.of("and".c_str())),
                            vector.of(vector.of("and".c_str())))
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
