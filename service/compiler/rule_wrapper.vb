
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.template
Imports osi.service.automata
Imports osi.service.resource

Public Class rule_wrapper(Of _nlexer_rule As __do(Of Byte()),
                             _syntaxer_rule As __do(Of Byte()))
    Public Shared ReadOnly nlexer_rule As String = (+alloc(Of _nlexer_rule)()).as_text()
    Public Shared ReadOnly syntaxer_rule As String = (+alloc(Of _syntaxer_rule)()).as_text()

    Public Shared Function nlp() As nlp
        Return nlp_holder.nlp
    End Function

    Private NotInheritable Class nlp_holder
        Public Shared ReadOnly nlp As nlp = nlp.of(nlexer_rule, syntaxer_rule)

        Private Sub New()
        End Sub
    End Class

    Protected Sub New()
    End Sub
End Class
