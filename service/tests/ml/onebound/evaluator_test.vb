
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.ml.onebound

Namespace onebound
    <test>
    Public NotInheritable Class evaluator_test
        <test>
        Private Shared Sub evaluate()
            Dim e As typed(Of Char).evaluator = Nothing
            e = New typed(Of Char).evaluator(New typed(Of Char).trainer().
                                                  accumulate("a"c, "n"c, 1).
                                                  accumulate("n"c, 1).
                                                  accumulate("a"c, "p"c, 1).
                                                  accumulate("p"c, "p"c, 1).
                                                  accumulate("p"c, "l"c, 1).
                                                  accumulate("l"c, "e"c, 1).
                                                  accumulate("e"c, 1).
                                                  dump())
            assertion.equal(e(vector.of("anapple".c_str())),
                            vector.of(vector.of("an".c_str()), vector.of("apple".c_str())))
        End Sub

        <test>
        Private Shared Sub evaluate2()
            Dim e As typed(Of Char).evaluator = Nothing
            e = New typed(Of Char).evaluator(New typed(Of Char).trainer().
                                                  accumulate("a"c, "n"c, 1).
                                                  accumulate("n"c, 0.1).
                                                  accumulate("a"c, "p"c, 1).
                                                  accumulate("p"c, "p"c, 1).
                                                  accumulate("p"c, "l"c, 1).
                                                  accumulate("l"c, "e"c, 1).
                                                  accumulate("e"c, 0.1).
                                                  accumulate("a"c, "n"c, 1).
                                                  accumulate("n"c, "d"c, 1).
                                                  accumulate("d"c, 0.1).
                                                  accumulate("a"c, "n"c, 1).
                                                  accumulate("n"c, 0.1).
                                                  accumulate("o"c, "r"c, 1).
                                                  accumulate("r"c, "a"c, 1).
                                                  accumulate("a"c, "n"c, 1).
                                                  accumulate("n"c, "g"c, 1).
                                                  accumulate("g"c, "e"c, 1).
                                                  accumulate("e"c, 0.1).
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
            Dim e As typed(Of Char).evaluator = Nothing
            e = New typed(Of Char).evaluator(New typed(Of Char).trainer().
                                                  accumulate("a"c, "n"c, 1).
                                                  accumulate("n"c, 0.1).
                                                  accumulate("a"c, "n"c, 1).
                                                  accumulate("n"c, "d"c, 1).
                                                  accumulate("d"c, 0.1).
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
