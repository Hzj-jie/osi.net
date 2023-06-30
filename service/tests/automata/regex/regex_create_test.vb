
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.automata
Imports osi.service.automata.rlexer

Namespace rlexer
    Public Class regex_create_test
        Inherits [case]

        Private Shared ReadOnly failure_cases() As String =
            {"[]]",
             "[]??[",
             "abbcde[afgadsf"}

        Private Shared Function case1() As Boolean
            Dim c As regex = Nothing
            assertion.is_true(regex.create("[a,b,c]def[g,h,i]", c))
            Dim v As vector(Of matching_group) = Nothing
            v = (+c)
            If assertion.is_not_null(v) AndAlso
               assertion.equal(v.size(), CUInt(3)) Then
                Dim g As string_matching_group = Nothing
                If assertion.is_true(cast(v(0), g)) Then
                    assertion.array_equal(+g, {"a", "b", "c"})
                End If
                If assertion.is_true(cast(v(1), g)) Then
                    assertion.array_equal(+g, {"def"})
                End If
                If assertion.is_true(cast(v(2), g)) Then
                    assertion.array_equal(+g, {"g", "h", "i"})
                End If
            End If
            Return True
        End Function

        Private Shared Function case2() As Boolean
            Dim c As regex = Nothing
            assertion.is_true(regex.create("abcde", c))
            Dim v As vector(Of matching_group) = Nothing
            v = (+c)
            If assertion.is_not_null(v) AndAlso
               assertion.equal(v.size(), uint32_1) Then
                Dim g As string_matching_group = Nothing
                If assertion.is_true(cast(v(0), g)) Then
                    assertion.array_equal(+g, {"abcde"})
                End If
            End If
            Return True
        End Function

        Private Shared Function case3() As Boolean
            Dim c As regex = Nothing
            assertion.is_true(regex.create("[a]", c))
            Dim v As vector(Of matching_group) = Nothing
            v = (+c)
            If assertion.is_not_null(v) AndAlso
               assertion.equal(v.size(), uint32_1) Then
                Dim g As string_matching_group = Nothing
                If assertion.is_true(cast(v(0), g)) Then
                    assertion.array_equal(+g, {"a"})
                End If
            End If
            Return True
        End Function

        Private Shared Function case4() As Boolean
            Dim c As regex = Nothing
            assertion.is_true(regex.create("[\x5C]", c))
            Dim v As vector(Of matching_group) = Nothing
            v = (+c)
            If assertion.is_not_null(v) AndAlso
               assertion.equal(v.size(), uint32_1) Then
                Dim g As string_matching_group = Nothing
                If assertion.is_true(cast(v(0), g)) Then
                    assertion.array_equal(+g, {"\"})
                End If
            End If
            Return True
        End Function

        Private Shared Function case5() As Boolean
            Dim c As regex = Nothing
            assertion.is_true(regex.create("[a,b,c]*def", c))
            Dim v As vector(Of matching_group) = Nothing
            v = (+c)
            If assertion.is_not_null(v) AndAlso
               assertion.equal(v.size(), CUInt(2)) Then
                Dim g As any_matching_group = Nothing
                Dim sg As string_matching_group = Nothing
                If assertion.is_true(cast(v(0), g)) Then
                    If assertion.is_true(cast(+g, sg)) Then
                        assertion.array_equal(+sg, {"a", "b", "c"})
                    End If
                End If
                If assertion.is_true(cast(v(1), sg)) Then
                    assertion.array_equal(+sg, {"def"})
                End If
            End If
            Return True
        End Function

        Private Shared Function case6() As Boolean
            Dim c As regex = Nothing
            assertion.is_true(regex.create("[a,b,c]!", c))
            Dim v As vector(Of matching_group) = Nothing
            v = (+c)
            If assertion.is_not_null(v) AndAlso
               assertion.equal(v.size(), uint32_1) Then
                Dim g As reverse_matching_group = Nothing
                If assertion.is_true(cast(v(0), g)) Then
                    Dim sg As string_matching_group = Nothing
                    If assertion.is_true(cast(+g, sg)) Then
                        assertion.array_equal(+sg, {"a", "b", "c"})
                    End If
                End If
            End If
            Return True
        End Function

        Private Shared Function case7() As Boolean
            Dim c As regex = Nothing
            assertion.is_true(regex.create("[*]!!", c))
            Dim v As vector(Of matching_group) = Nothing
            v = (+c)
            If assertion.is_not_null(v) AndAlso
               assertion.equal(v.size(), uint32_1) Then
                Dim g As reverse_matching_group = Nothing
                If assertion.is_true(cast(v(0), g)) AndAlso
                   assertion.is_true(cast(+g, g)) Then
                    assertion.is_true(TypeOf +g Is any_character_matching_group)
                End If
            End If
            Return True
        End Function

        Private Shared Function case8() As Boolean
            Dim c As regex = Nothing
            assertion.is_true(regex.create("[abc]-def", c))
            Dim v As vector(Of matching_group) = Nothing
            v = (+c)
            If assertion.is_not_null(v) AndAlso
               assertion.equal(v.size(), CUInt(2)) Then
                Dim g As unmatched_matching_group = Nothing
                Dim sg As string_matching_group = Nothing
                If cast(v(0), g) AndAlso
                   cast(+g, sg) Then
                    assertion.array_equal(+sg, {"abc"})
                End If
                If cast(v(1), sg) Then
                    assertion.array_equal(+sg, {"def"})
                End If
            End If
            Return True
        End Function

        Private Shared Function case9() As Boolean
            Dim c As regex = Nothing
            assertion.is_true(regex.create("[a,b,c]+def", c))
            Dim v As vector(Of matching_group) = Nothing
            v = (+c)
            If assertion.is_not_null(v) AndAlso
               assertion.equal(v.size(), CUInt(2)) Then
                Dim g As multi_matching_group = Nothing
                Dim sg As string_matching_group = Nothing
                If assertion.is_true(cast(v(0), g)) Then
                    If assertion.is_true(cast(+g, sg)) Then
                        assertion.array_equal(+sg, {"a", "b", "c"})
                    End If
                End If
                If assertion.is_true(cast(v(1), sg)) Then
                    assertion.array_equal(+sg, {"def"})
                End If
            End If
            Return True
        End Function

        Private Shared Function failure_case() As Boolean
            For i As UInt32 = 0 To array_size(failure_cases) - uint32_1
                assertion.is_false(regex.create(failure_cases(i), Nothing))
            Next
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Return case1() AndAlso
                   case2() AndAlso
                   case3() AndAlso
                   case4() AndAlso
                   case5() AndAlso
                   case6() AndAlso
                   case7() AndAlso
                   case8() AndAlso
                   failure_case()
        End Function
    End Class
End Namespace
