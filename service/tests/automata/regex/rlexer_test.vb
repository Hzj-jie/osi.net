
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.automata
Imports osi.service.automata.rlexer
Imports lexer = osi.service.automata.rlexer

Namespace rlexer
    Public Class rlexer_test
        Inherits [case]

        Private Shared ReadOnly cases() As pair(Of String, String())

        Shared Sub New()
            cases = {make_case("void main(string arg) {  " +
                               "if (isempty(arg)) { __X__ = 100; } else { __X__ = -100; _a = +100;}}",
                               "name", "blank", "name", "start-bracket", "name", "blank", "name", "end-bracket",
                               "blank", "start-paragraph", "blank", "blank", "if-clause", "blank", "start-bracket",
                               "name", "start-bracket", "name", "end-bracket", "end-bracket", "blank",
                               "start-paragraph", "blank", "name", "blank", "equal", "blank", "number", "semi-colon",
                               "blank", "end-paragraph", "blank", "else-clause", "blank", "start-paragraph", "blank",
                               "name", "blank", "equal", "blank", "number", "semi-colon", "blank", "name", "blank",
                               "equal", "blank", "number", "semi-colon", "end-paragraph", "end-paragraph"),
                     make_case("if(100) { _a = +99;_a=99;} else{ x_1= -99;};",
                               "if-clause", "start-bracket", "number", "end-bracket", "blank", "start-paragraph",
                               "blank", "name", "blank", "equal", "blank", "number", "semi-colon", "name", "equal",
                               "number", "semi-colon", "end-paragraph", "blank", "else-clause", "start-paragraph",
                               "blank", "name", "equal", "blank", "number", "semi-colon", "end-paragraph",
                               "semi-colon")}
        End Sub

        Private Shared Function make_case(ByVal doc As String,
                                          ByVal ParamArray types() As String) As pair(Of String, String())
            Return pair.of(doc, types)
        End Function

        Private Shared Function run_case(ByVal r As rule) As Boolean
            assert(Not isemptyarray(cases))
            assert(r IsNot Nothing)
            Dim e As rule.exporter = Nothing
            If assertion.is_true(r.export(e)) AndAlso assertion.is_not_null(e) Then
                Dim l As lexer = Nothing
                l = e.rlexer
                If assertion.is_not_null(l) Then
                    For i As UInt32 = 0 To array_size(cases) - uint32_1
                        assert(cases(i) IsNot Nothing)
                        Dim v As vector(Of typed_word) = Nothing
                        Dim o As vector(Of String) = Nothing
                        If assertion.is_true(l.match(cases(i).first, v)) AndAlso
                           e.types_to_strs(v, o) AndAlso
                           assertion.is_not_null(o) Then
                            assertion.array_equal(+o, cases(i).second)
                        End If
                    Next
                End If
            End If
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Dim r As rule = Nothing
            r = New rule()
            assertion.is_true(r.parse_file(rlexer_test_rule_files.rule1))
            If Not run_case(r) Then
                Return False
            End If
            r = New rule()
            assertion.is_true(r.parse_file(rlexer_test_rule_files.rule3))
            If Not run_case(r) Then
                Return False
            End If
            Return True
        End Function
    End Class
End Namespace
