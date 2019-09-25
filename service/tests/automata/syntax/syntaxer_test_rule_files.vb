
Imports System.IO
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.resource

Namespace syntaxer
    Public Class syntaxer_test_rule_files
        Public Shared ReadOnly rlexer As String
        Public Shared ReadOnly rlexer2 As String
        Public Shared ReadOnly syntaxer As String

        Shared Sub New()
            rlexer = rlexer_rule.as_text()
            rlexer2 = rlexer_rule2.as_text()
            syntaxer = syntaxer_rule.as_text()
        End Sub
    End Class
End Namespace