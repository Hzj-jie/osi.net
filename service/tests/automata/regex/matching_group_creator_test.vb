
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.automata
Imports osi.service.automata.rlexer

Namespace rlexer
    Public Class matching_group_creator_test
        Inherits [case]

        Private Shared ReadOnly failure_cases() As String =
            {"abc[", "[][", "]][]"}

        Private Shared Function case1() As Boolean
            Dim g As matching_group = Nothing
            If assertion.is_true(matching_group_creator.create("[a,b,c]*", g)) Then
                Dim mg As any_matching_group = Nothing
                If assertion.is_true(cast(g, mg)) Then
                    assert(Not mg Is Nothing)
                    Dim sg As string_matching_group = Nothing
                    If assertion.is_true(cast(+mg, sg)) Then
                        assertion.array_equal(+sg, {"a", "b", "c"})
                    End If
                End If
            End If
            Return True
        End Function

        Private Shared Function case2() As Boolean
            assertion.is_false(matching_group_creator.create("[]", Nothing))
            Return True
        End Function

        Private Shared Function case3() As Boolean
            Dim g As matching_group = Nothing
            If assertion.is_true(matching_group_creator.create("[*]*", g)) Then
                Dim mg As any_matching_group = Nothing
                If assertion.is_true(cast(g, mg)) Then
                    assert(Not mg Is Nothing)
                    Dim am As any_character_matching_group = Nothing
                    assertion.is_true(cast(+mg, am))
                    assert(Not am Is Nothing)
                End If
            End If
            Return True
        End Function

        Private Shared Function case4() As Boolean
            Dim g As matching_group = Nothing
            If assertion.is_true(matching_group_creator.create("[*]--", g)) Then
                Dim ug As unmatched_matching_group = Nothing
                If assertion.is_true(cast(g, ug)) Then
                    assert(Not ug Is Nothing)
                    If assertion.is_true(cast(+ug, ug)) Then
                        assert(Not ug Is Nothing)
                        Dim am As any_character_matching_group = Nothing
                        assertion.is_true(cast(+ug, am))
                        assert(Not am Is Nothing)
                    End If
                End If
            End If
            Return True
        End Function

        Private Shared Function failure_case() As Boolean
            For i As UInt32 = 0 To array_size(failure_cases) - uint32_1
                assertion.is_false(matching_group_creator.create(failure_cases(i), Nothing))
            Next
            Return True
        End Function

        Private Shared Function case5() As Boolean
            Dim g As matching_group = Nothing
            If assertion.is_true(matching_group_creator.create("[a,b,c]+", g)) Then
                Dim mg As multi_matching_group = Nothing
                If assertion.is_true(cast(g, mg)) Then
                    assert(Not mg Is Nothing)
                    Dim sg As string_matching_group = Nothing
                    If assertion.is_true(cast(+mg, sg)) Then
                        assertion.array_equal(+sg, {"a", "b", "c"})
                    End If
                End If
            End If
            Return True
        End Function

        Private Shared Function case6() As Boolean
            Dim g As matching_group = Nothing
            If assertion.is_true(matching_group_creator.create("[*]+", g)) Then
                Dim mg As multi_matching_group = Nothing
                If assertion.is_true(cast(g, mg)) Then
                    assert(Not mg Is Nothing)
                    Dim am As any_character_matching_group = Nothing
                    assertion.is_true(cast(+mg, am))
                    assert(Not am Is Nothing)
                End If
            End If
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Return failure_case() AndAlso
                   case1() AndAlso
                   case2() AndAlso
                   case3() AndAlso
                   case4() AndAlso
                   case5() AndAlso
                   case6()
        End Function
    End Class
End Namespace
