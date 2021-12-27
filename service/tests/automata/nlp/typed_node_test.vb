
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.automata
Imports osi.service.resource

<test>
Public NotInheritable Class typed_node_test
    <test>
    Private Shared Sub input_returns_input()
        Dim r As nlp = Nothing
        assertion.is_true(nlp.of(nlp_test_rules.nlexer_rule.as_text(),
                                 nlp_test_rules.syntaxer_rule.as_text(),
                                 r))
        Const code As String = "string main() { return ""\""""; }"
        Dim n As typed_node = Nothing
        assertion.is_true(r.parse(code, root:=n))
        assertion.equal(code, n.input())
    End Sub

    Private Sub New()
    End Sub
End Class
