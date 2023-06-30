

Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.automata
Imports osi.service.automata.nlexer

Namespace nlexer
    <test>
    Public NotInheritable Class rule_c_str_test
        <test>
        Private Shared Sub run()
            Dim r As rule = Nothing
            assertion.is_true(rule.of("""[\"",*|""]*""", r))

            assertions.of(r.match("""abc""")).has_value(5)
            assertions.of(r.match("""a bc\""""")).has_value(8)
        End Sub

        <test>
        Private Shared Sub longest_match()
            Dim r As rule = Nothing
            assertion.is_true(rule.of("""[*,\""|""]*""", r))

            assertions.of(r.match("""abc""")).has_value(5)
            assertions.of(r.match("""a bc\""""")).has_value(8)
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
