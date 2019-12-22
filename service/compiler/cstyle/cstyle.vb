
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.resource

Public NotInheritable Class cstyle
    Public Shared ReadOnly nlp As nlp

    Shared Sub New()
        assert(nlp.of(cstyle_rules.nlexer_rule.as_text(), cstyle_rules.syntaxer_rule.as_text(), nlp))
    End Sub
End Class
