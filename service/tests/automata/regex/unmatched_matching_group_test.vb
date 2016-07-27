
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.automata
Imports osi.service.automata.rlexer

Namespace rlexer
    Public Class unmatched_matching_group_test
        Inherits [case]

        Private Shared Function case1() As Boolean
            Dim g As unmatched_matching_group = Nothing
            g = New unmatched_matching_group(New any_character_matching_group())
            Dim v As vector(Of UInt32) = Nothing
            v = g.match("abc")
            assert_true(v.null_or_empty())
            Return True
        End Function

        Private Shared Function case2() As Boolean
            Dim g As unmatched_matching_group = Nothing
            g = New unmatched_matching_group(New string_matching_group("abc"))
            Dim v As vector(Of UInt32) = Nothing
            v = g.match("abcd")
            assert_true(v.null_or_empty())
            v = g.match("bcd")
            If assert_false(v.null_or_empty()) AndAlso
               assert_equal(v.size(), uint32_1) Then
                assert_equal(v(0), uint32_0)
            End If
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Return case1() AndAlso
                   case2()
        End Function
    End Class
End Namespace
