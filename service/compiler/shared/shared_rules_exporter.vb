
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.constants
Imports osi.root.utils
Imports osi.service.resource

<global_init(global_init_level.services)>
Public NotInheritable Class shared_rules_exporter
    Private Shared ReadOnly folder As String = Path.Combine(temp_folder, "service/compiler/shared")

    Private Shared Sub init()
        shared_rules.nlexer_rule.sync_export(Path.Combine(folder, "nlexer_rule.txt"))
        shared_rules.nlexer_rule2.sync_export(Path.Combine(folder, "nlexer_rule2.txt"))
        shared_rules.syntaxer_rule.sync_export(Path.Combine(folder, "syntaxer_rule.txt"))
    End Sub

    Private Sub New()
    End Sub
End Class
