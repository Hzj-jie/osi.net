
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.ml
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

        <test>
        <command_line_specified>
        Private Shared Sub from_tracer_0_9()
            onebound(Of Char).selector.exponential(
                onebound(Of Char).model.load("cjk.words.2.bin"),
                0.9).dump("cjk.words.2.bin.e0.9")
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub dump_from_tracer_0_9()
            onebound(Of Char).model.
                load("cjk.words.2.bin.e0.9").
                flat_map().
                foreach(Sub(ByVal x As first_const_pair(Of const_pair(Of Char, Char), Double))
                            Console.WriteLine(strcat(x.first.first, " ", x.first.second, ", ", x.second))
                        End Sub)
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
