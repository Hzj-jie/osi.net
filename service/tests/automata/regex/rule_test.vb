
Imports System.IO
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.automata
Imports osi.service.automata.rlexer

Namespace rlexer
    Public Class rule_test
        Inherits [case]

        Private Overloads Shared Function run(ByVal r As rule) As Boolean
            assert(Not r Is Nothing)
            Dim e As rule.exporter = Nothing
            If assert_true(r.export(e)) AndAlso assert_not_nothing(e) Then
                assert_equal(e.type_choice, match_choice.first_defined)
                assert_equal(e.word_choice, match_choice.greedy)
                assert_true(e.macros.defined("vowel"))
                assert_true(e.macros.defined("consonant"))
                assert_false(e.macros.defined("ABC"))
                Dim default_macros As vector(Of pair(Of String, String)) = Nothing
                default_macros = macros.default.export()
                assert(Not default_macros.null_or_empty())
                For i As UInt32 = 0 To default_macros.size() - uint32_1
                    assert_true(e.macros.defined(default_macros(i).first))
                    'some macros need to be expanded for several times until to get the final format
                    assert_equal(e.macros.expand(characters.macro_escape + default_macros(i).first),
                                 e.macros.expand(default_macros(i).second))
                Next
                For i As UInt32 = 0 To e.macros.export().size() - uint32_1
                    assert_true(e.macros.defined(e.macros.export()(i).first))
                    'some macros need to be expanded for several times until to get the final format
                    assert_equal(e.macros.expand(characters.macro_escape + e.macros.export()(i).first),
                                 e.macros.expand(e.macros.export()(i).second))
                Next
            End If
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Dim r As rule = Nothing
            r = New rule()
            assert_true(r.parse_file(rlexer_test_rule_files.rule1))
            If Not run(r) Then
                Return False
            End If

            r = New rule()
            assert_true(r.parse_file(rlexer_test_rule_files.rule3))
            If Not run(r) Then
                Return False
            End If

            r = New rule()
            assert_true(r.parse_content(File.ReadAllText(rlexer_test_rule_files.rule3)))
            If Not run(r) Then
                Return False
            End If

            r = New rule()
            assert_true(r.parse(File.ReadAllLines(rlexer_test_rule_files.rule3)))
            If Not run(r) Then
                Return False
            End If
            Return True
        End Function
    End Class
End Namespace
