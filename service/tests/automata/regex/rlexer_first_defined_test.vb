
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.automata
Imports osi.service.automata.rlexer
Imports rl = osi.service.automata.rlexer

Namespace rlexer
    Public Class rlexer_first_defined_test
        Inherits [case]

        Private Shared Function create_rlexer(ByVal l As rl, ByVal ParamArray regexes() As String) As Boolean
            assert(Not l Is Nothing)
            assert(Not isemptyarray(regexes))
            For i As UInt32 = 0 To array_size(regexes) - uint32_1
                Dim r As regex = Nothing
                Dim s As String = Nothing
                s = macros.default.expand(regexes(i))
                If Not regex.create(s, r) OrElse
                   Not assert_not_nothing(r) OrElse
                   Not assert_true(l.define(r)) Then
                    Return False
                End If
            Next
            Return True
        End Function

        Private Shared Function case1() As Boolean
            Dim l As rl = Nothing
            l = New rl(rl.match_choice.first_defined, rl.match_choice.greedy)
            If assert_true(create_rlexer(l,
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
                Dim r As vector(Of typed_word) = Nothing
                Const doc As String = "if(100) { _a = +99;_a=99;} else{ x_1= -99;};"
                assert_true(l.match(doc, r))
                If assert_false(r.null_or_empty()) AndAlso
                   assert_equal(r.size(), CUInt(29)) Then
                    assert_equal(r.str(), doc)
                    Dim types() As UInt32 = {0, 6, 8, 7, 10, 2, 10, 4, 10, 5, 10, 8, 9, 4,
                                             5, 8, 9, 3, 10, 1, 2, 10, 4, 5, 10, 8, 9, 3, 9}
                    assert(array_size(types) = r.size())
                    For i As UInt32 = 0 To r.size() - uint32_1
                        assert_equal(r(i).type, types(i), r(i).str())
                    Next
                End If
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Function run() As Boolean
            Return case1()
        End Function
    End Class
End Namespace
