
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.automata
Imports osi.service.automata.rlexer
Imports rl = osi.service.automata.rlexer

Namespace rlexer
    Public Class rlexer_first_defined_test
        Inherits [case]

        Private Shared Function create_rlexer(ByVal l As rl, ByVal ParamArray regexes() As String) As Boolean
            assert(l IsNot Nothing)
            assert(Not isemptyarray(regexes))
            For i As Int32 = 0 To regexes.Length() - 1
                Dim r As regex = Nothing
                Dim s As String = macros.default.expand(regexes(i))
                If Not regex.create(s, r) OrElse
                   Not assertion.is_not_null(r) OrElse
                   Not assertion.is_true(l.define(r)) Then
                    Return False
                End If
            Next
            Return True
        End Function

        Private Shared Function case1() As Boolean
            Dim l As New rl(rl.match_choice.first_defined, rl.match_choice.greedy)
            If Not assertion.is_true(create_rlexer(l,
                                     "if",
                                     "else",
                                     "{",
                                     "}",
                                     "[\d]-[\w,\d,_]+",
                                     "=",
                                     "(",
                                     ")",
                                     "[+,-]?[\d]+",
                                     ";",
                                     "[\b]")) Then
                Return False
            End If
            Dim r As vector(Of typed_word) = Nothing
            Const doc As String = "if(100) { _a = +99;_a=99;} else{ x_1= -99;};"
            assertion.is_true(l.match(doc, r))
            If assertion.is_false(r.null_or_empty()) AndAlso
               assertion.equal(r.size(), CUInt(29)) Then
                assertion.equal(r.str(Function(ByVal w As typed_word) As String
                                          assert(w IsNot Nothing)
                                          Return w.str()
                                      End Function), doc)
                Dim types() As UInt32 = {0, 6, 8, 7, 10, 2, 10, 4, 10, 5, 10, 8, 9, 4,
                                             5, 8, 9, 3, 10, 1, 2, 10, 4, 5, 10, 8, 9, 3, 9}
                assert(array_size(types) = r.size())
                For i As UInt32 = 0 To r.size() - uint32_1
                    assertion.equal(r(i).type, types(CInt(i)), r(i).str())
                Next
            End If
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Return case1()
        End Function
    End Class
End Namespace
