
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.automata

Namespace syntaxer
    Public NotInheritable Class syntaxer_test
        Inherits [case]

        Public Overrides Function run() As Boolean
            Return lang_parser_test.run_cases(Function(ByRef o As lang_parser) As Boolean
                                                  Dim r As rlp = Nothing
                                                  Return rlp.create_from_content(syntaxer_test_rule_files.rlexer,
                                                                                 syntaxer_test_rule_files.syntaxer,
                                                                                 r) AndAlso
                                                         eva(o, r)
                                              End Function) AndAlso
                   lang_parser_test.run_cases(Function(ByRef o As lang_parser) As Boolean
                                                  Dim r As rlp = Nothing
                                                  Return rlp.create_from_content(syntaxer_test_rule_files.rlexer2,
                                                                                 syntaxer_test_rule_files.syntaxer,
                                                                                 r) AndAlso
                                                         eva(o, r)
                                              End Function)
        End Function
    End Class
End Namespace
