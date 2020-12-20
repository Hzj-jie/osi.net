
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.automata
Imports osi.service.resource

<test>
Public NotInheritable Class nlp_test
    <test>
    Private Shared Sub parsable()
        Dim n As nlp = Nothing
        assertion.is_true(nlp.of(nlp_test_rules.nlexer_rule.as_text(), nlp_test_rules.syntaxer_rule.as_text(), n))
    End Sub

    <test>
    Private Shared Function parse_test() As Boolean
        Return lang_parser_test.run_cases(Function(ByRef o As lang_parser) As Boolean
                                              Dim r As nlp = Nothing
                                              Return nlp.of(nlp_test_rules.nlexer_rule.as_text(),
                                                            nlp_test_rules.syntaxer_rule.as_text(),
                                                            r) AndAlso
                                                     eva(o, r)
                                          End Function)
    End Function

    <test>
    Private Shared Sub escape_str()
        Dim r As nlp = Nothing
        assertion.is_true(nlp.of(nlp_test_rules.nlexer_rule.as_text(),
                                 nlp_test_rules.syntaxer_rule.as_text(),
                                 r))
        assertion.is_true(r.parse("string main() { return ""\""""; }"))
    End Sub

    Private Sub New()
    End Sub
End Class
