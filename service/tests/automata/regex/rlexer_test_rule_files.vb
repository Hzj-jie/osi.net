
Imports osi.root.envs
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.utils

Namespace rlexer
    Public Class rlexer_test_rule_files
        Public Shared ReadOnly rule1 As String
        Public Shared ReadOnly rule3 As String

        Shared Sub New()
            rule1 = pather.default.combine(temp_folder, "rlexer_test_rule1.txt")
            rule3 = pather.default.combine(temp_folder, "rlexer_test_rule3.txt")
            assert(rlexer_test_rules.rule1.sync_export(rule1))
            assert(rlexer_test_rules.rule2.sync_export(pather.default.combine(temp_folder, "rlexer_test_rule2.txt")))
            assert(rlexer_test_rules.rule3.sync_export(rule3))
        End Sub
    End Class
End Namespace
