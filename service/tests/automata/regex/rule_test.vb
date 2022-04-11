
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.automata.rlexer

Namespace rlexer
    Public Class rule_test
        Inherits [case]

        Private Overloads Shared Function run(ByVal r As rule) As Boolean
            assert(Not r Is Nothing)
            Dim e As rule.exporter = Nothing
            Dim macros As macros = Nothing
            If Not assertion.is_true(r.export(e, macros)) OrElse Not assertion.is_not_null(e) Then
                Return False
            End If
            assertion.equal(e.rlexer.type_choice, match_choice.first_defined)
            assertion.equal(e.rlexer.word_choice, match_choice.greedy)
            assertion.is_true(macros.defined("vowel"))
            assertion.is_true(macros.defined("consonant"))
            assertion.is_false(macros.defined("ABC"))
            Dim default_macros As vector(Of pair(Of String, String)) = Nothing
            default_macros = macros.default.export()
            assert(Not default_macros.null_or_empty())
            For i As UInt32 = 0 To default_macros.size() - uint32_1
                assertion.is_true(macros.defined(default_macros(i).first))
                'some macros need to be expanded for several times until to get the final format
                assertion.equal(macros.expand(characters.macro_escape + default_macros(i).first),
                                macros.expand(default_macros(i).second))
            Next
            For i As UInt32 = 0 To macros.export().size() - uint32_1
                assertion.is_true(macros.defined(macros.export()(i).first))
                'some macros need to be expanded for several times until to get the final format
                assertion.equal(macros.expand(characters.macro_escape + macros.export()(i).first),
                                macros.expand(macros.export()(i).second))
            Next
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Dim r As New rule()
            assertion.is_true(r.parse_file(rlexer_test_rule_files.rule1))
            If Not run(r) Then
                Return False
            End If

            r = New rule()
            assertion.is_true(r.parse_file(rlexer_test_rule_files.rule3))
            If Not run(r) Then
                Return False
            End If

            r = New rule()
            assertion.is_true(r.parse_content(File.ReadAllText(rlexer_test_rule_files.rule3)))
            If Not run(r) Then
                Return False
            End If

            r = New rule()
            assertion.is_true(r.parse(File.ReadAllLines(rlexer_test_rule_files.rule3)))
            If Not run(r) Then
                Return False
            End If
            Return True
        End Function
    End Class
End Namespace
