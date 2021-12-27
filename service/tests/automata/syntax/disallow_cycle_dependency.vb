
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.automata
Imports osi.service.automata.syntaxer

Namespace syntaxer
    <test>
    Public NotInheritable Class disallow_cycle_dependency
        <test>
        Private Shared Sub run()
            Dim r As rule = Nothing
            r = New rule(New syntax_collection(map.of(pair.emplace_of("self-inc", uint32_1),
                                                      pair.emplace_of("self-dec", uint32_2),
                                                      pair.emplace_of("variable-name", uint32_3))))
            assertion.is_true(r.parse_content(syntaxer_test_rule_files.cycle_dependency_syntaxer))
            assertion.is_false(r.export().syntaxer.match(typed_word.fakes(uint32_3, uint32_1)))
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
