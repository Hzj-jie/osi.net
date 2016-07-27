﻿
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.automata
Imports osi.service.automata.rlexer

Namespace rlexer
    Public Class multi_matching_group_test
        Inherits [case]

        Private Shared Function case1() As Boolean
            Dim g As multi_matching_group = Nothing
            g = New multi_matching_group(New string_matching_group("a", "b", "c"))
            Dim v As vector(Of UInt32) = Nothing
            v = g.match("abc")
            If assert_true(Not v.null_or_empty()) Then
                assert_equal(v.size(), CUInt(3))
                For i As UInt32 = 0 To v.size() - uint32_1
                    assert_equal(v(i), i + uint32_1)
                Next
            End If
            Return True
        End Function

        Private Shared Function case2() As Boolean
            Dim g As multi_matching_group = Nothing
            g = New multi_matching_group(New string_matching_group("", "a", "b"))
            Dim v As vector(Of UInt32) = Nothing
            v = g.match("abc")
            If assert_true(Not v.null_or_empty()) Then
                assert_equal(v.size(), CUInt(3))
                For i As UInt32 = 0 To v.size() - uint32_1
                    assert_equal(v(i), i)
                Next
            End If
            Return True
        End Function

        Private Shared Function case3() As Boolean
            Dim g As multi_matching_group = Nothing
            g = New multi_matching_group(New string_matching_group("", "a", "b"))
            Dim v As vector(Of UInt32) = Nothing
            v = g.match("c")
            If assert_true(Not v.null_or_empty()) Then
                assert_equal(v.size(), uint32_1)
                For i As UInt32 = 0 To v.size() - uint32_1
                    assert_equal(v(i), i)
                Next
            End If
            Return True
        End Function

        Private Shared Function case4() As Boolean
            Dim g As multi_matching_group = Nothing
            g = New multi_matching_group(New any_character_matching_group())
            Dim v As vector(Of UInt32) = Nothing
            v = g.match("abc")
            If assert_true(Not v.null_or_empty()) AndAlso
               assert_equal(v.size(), CUInt(3)) Then
                For i As UInt32 = 0 To v.size() - uint32_1
                    assert_equal(v(i), i + uint32_1)
                Next
            End If
            Return True
        End Function

        Private Shared Function case5() As Boolean
            Dim g As multi_matching_group = Nothing
            g = New multi_matching_group(New any_character_matching_group())
            Dim v As vector(Of UInt32) = Nothing
            v = g.match("")
            assert_true(v.null_or_empty())
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Return case1() AndAlso
                   case2() AndAlso
                   case3() AndAlso
                   case4() AndAlso
                   case5()
        End Function
    End Class
End Namespace
