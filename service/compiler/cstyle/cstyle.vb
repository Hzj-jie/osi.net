
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.resource

Public NotInheritable Class cstyle
    Public Shared ReadOnly nlexer_rule As String
    Public Shared ReadOnly syntaxer_rule As String

    Public Shared Function nlp() As nlp
        Return nlp_holder.nlp
    End Function

    Shared Sub New()
        nlexer_rule = cstyle_rules.nlexer_rule.as_text()
        syntaxer_rule = cstyle_rules.syntaxer_rule.as_text()
    End Sub

    Private NotInheritable Class nlp_holder
        Public Shared ReadOnly nlp As nlp

        Shared Sub New()
            assert(nlp.of(nlexer_rule, syntaxer_rule, nlp))
        End Sub
    End Class
End Class
