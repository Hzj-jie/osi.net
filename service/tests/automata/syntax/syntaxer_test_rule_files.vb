
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.resource

Namespace syntaxer
    Public NotInheritable Class syntaxer_test_rule_files
        Public Shared ReadOnly rlexer As String
        Public Shared ReadOnly rlexer2 As String
        Public Shared ReadOnly syntaxer As String

        Shared Sub New()
            rlexer = syntaxer_test_rules.rlexer_rule.as_text()
            rlexer2 = syntaxer_test_rules.rlexer_rule2.as_text()
            syntaxer = syntaxer_test_rules.syntaxer_rule.as_text()
        End Sub
    End Class
End Namespace