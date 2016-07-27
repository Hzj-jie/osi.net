
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.automata
Imports osi.service.automata.rlexer

Namespace rlexer
    Public Class string_matching_group_test
        Inherits [case]

        Private Shared Function create_case() As Boolean
            Dim g As string_matching_group = Nothing
            assert_false(string_matching_group.create_as_multi_matches("", 0, 0, Nothing))
            assert_false(string_matching_group.create_as_multi_matches("a,b,c", 5, 0, Nothing))
            assert_false(string_matching_group.create_as_multi_matches("a,b,c", 5, 5, Nothing))
            assert_true(string_matching_group.create_as_multi_matches("a,b,c", 0, 5, g))
            Return True
        End Function

        Private Shared Function all_matching_case() As Boolean
            Dim s As String = Nothing
            s = english_characters
            Dim g As string_matching_group = Nothing
            g = New string_matching_group(s.ToCharArray())
            Dim v As vector(Of UInt32) = Nothing
            For i As UInt32 = 0 To strlen(s) - uint32_1
                v = g.match(s, i)
                If assert_true(Not v.null_or_empty()) AndAlso
                   assert_equal(v.size(), uint32_1) Then
                    assert_equal(v(0), i + uint32_1)
                End If
            Next
            Return True
        End Function

        Private Shared Function multi_matching_case() As Boolean
            Dim g As string_matching_group = Nothing
            g = New string_matching_group("a", "aa", "aaa", "aaab", "aaabc")
            Dim v As vector(Of UInt32) = Nothing
            v = g.match("aaabb", uint32_0)
            If assert_true(Not v.null_or_empty()) AndAlso
               assert_equal(v.size(), CUInt(4)) Then
                For i As UInt32 = 0 To v.size() - uint32_1
                    assert_equal(v(i), i + uint32_1)
                Next
            End If
            Return True
        End Function

        Private Shared Function empty_matching_case() As Boolean
            Dim g As string_matching_group = Nothing
            g = New string_matching_group("a", "")
            Dim v As vector(Of UInt32) = Nothing
            v = g.match("a", uint32_0)
            If assert_true(Not v.null_or_empty()) AndAlso
               assert_equal(v.size(), CUInt(2)) Then
                assert_equal(v(0), uint32_1)
                assert_equal(v(1), uint32_0)
            End If
            Return True
        End Function

        Private Shared Function unescape_matching_case() As Boolean
            Dim ss() As String = Nothing
            ss = {"\x2C", "\x5D", "\x2A", "\x5C", "\r", "\f", "\t", "\n"}
            Dim g As string_matching_group = Nothing
            g = New string_matching_group(ss)
            Dim s As String = Nothing
            s = c_unescape(strcat(ss))
            For i As UInt32 = 0 To strlen(s) - uint32_1
                Dim v As vector(Of UInt32) = Nothing
                v = g.match(s, i)
                If assert_true(Not v.null_or_empty()) AndAlso
                   assert_equal(v.size(), uint32_1) Then
                    assert_equal(v(0), i + uint32_1)
                End If
            Next
            Return True
        End Function

        Private Shared Function not_matching_case() As Boolean
            Dim g As string_matching_group = Nothing
            g = New string_matching_group("a")
            Dim v As vector(Of UInt32) = Nothing
            v = g.match("b", uint32_0)
            assert_true(v.null_or_empty())
            v = g.match("ab", uint32_1)
            assert_true(v.null_or_empty())
            Return True
        End Function

        Private Shared Function matching_case() As Boolean
            Return all_matching_case() AndAlso
                   multi_matching_case() AndAlso
                   empty_matching_case() AndAlso
                   unescape_matching_case() AndAlso
                   not_matching_case()
        End Function

        Public Overrides Function run() As Boolean
            Return create_case() AndAlso
                   matching_case()
        End Function
    End Class
End Namespace
