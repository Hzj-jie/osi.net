
Imports System.IO
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.utils

Namespace syntaxer
    Public Class syntaxer_test_rule_files
        Public Shared ReadOnly rlexer As String
        Public Shared ReadOnly rlexer2 As String
        Public Shared ReadOnly syntaxer As String

        Shared Sub New()
            rlexer = Path.Combine(temp_folder, guid_str())
            rlexer2 = Path.Combine(temp_folder, guid_str())
            syntaxer = Path.Combine(temp_folder, guid_str())
            assert(rlexer_rule.sync_export(rlexer))
            assert(rlexer_rule2.sync_export(rlexer2))
            assert(syntaxer_rule.sync_export(syntaxer))
        End Sub
    End Class
End Namespace