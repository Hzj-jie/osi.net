
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.template
Imports osi.service.automata

Public Class rule_wrapper(Of _nlexer_rule As __do(Of String),
                             _syntaxer_rule As __do(Of String))
    Public Shared ReadOnly nlexer_rule As String = +alloc(Of _nlexer_rule)()
    Public Shared ReadOnly syntaxer_rule As String = +alloc(Of _syntaxer_rule)()

    Public Shared Function nlp() As nlp
        Return nlp_holder.nlp
    End Function

    Private NotInheritable Class nlp_holder
        Public Shared ReadOnly nlp As nlp = nlp.of_file(nlexer_rule, syntaxer_rule)

        Private Sub New()
        End Sub
    End Class

    Protected Sub New()
    End Sub
End Class
