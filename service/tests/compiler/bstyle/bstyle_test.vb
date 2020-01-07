
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.automata
Imports osi.service.compiler

<test>
Public NotInheritable Class bstyle_test
    <test>
    Private Shared Sub nlp_parsable()
        assertion.is_true(nlp.of(cstyle.nlexer_rule, cstyle.syntaxer_rule, Nothing))
    End Sub

    Private Sub New()
    End Sub
End Class
