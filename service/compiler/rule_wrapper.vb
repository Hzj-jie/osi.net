
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.template
Imports osi.service.automata
Imports osi.service.resource

Public Class rule_wrapper(Of _nlexer_rule As __do(Of Byte()), _syntaxer_rule As __do(Of Byte()))
    Public Shared ReadOnly nlexer_rule As String
    Public Shared ReadOnly syntaxer_rule As String

    Public Shared Function nlp() As nlp
        Return nlp_holder.nlp
    End Function

    Shared Sub New()
        nlexer_rule = (+alloc(Of _nlexer_rule)()).as_text()
        syntaxer_rule = (+alloc(Of _syntaxer_rule)()).as_text()
    End Sub

    Private NotInheritable Class nlp_holder
        Public Shared ReadOnly nlp As nlp

        Shared Sub New()
            assert(nlp.of(nlexer_rule, syntaxer_rule, nlp))
        End Sub
    End Class

    Protected Sub New()
    End Sub
End Class
