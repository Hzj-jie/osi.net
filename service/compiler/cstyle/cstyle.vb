
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.resource

Public NotInheritable Class cstyle
    Public Shared ReadOnly nlexer_rule As String
    Public Shared ReadOnly syntaxer_rule As String
    Public Shared ReadOnly nlp As nlp

    Shared Sub New()
        nlexer_rule = cstyle_rules.nlexer_rule.as_text()
        syntaxer_rule = cstyle_rules.syntaxer_rule.as_text()
        assert(nlp.of(nlexer_rule, syntaxer_rule, nlp))
    End Sub
End Class
