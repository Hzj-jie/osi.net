
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.automata
Imports osi.service.automata.rlexer

Namespace rlexer
    Public Class reverse_matching_group_test
        Inherits [case]

        Private Shared Function case1() As Boolean
            Dim g As reverse_matching_group = Nothing
            g = New reverse_matching_group(New any_character_matching_group())
            Dim v As vector(Of UInt32) = Nothing
            v = g.match("abc")
            assertion.is_true(v.null_or_empty())
            Return True
        End Function

        Private Shared Function case2() As Boolean
            Dim g As reverse_matching_group = Nothing
            g = New reverse_matching_group(New string_matching_group("a", "b", "c"))
            Dim v As vector(Of UInt32) = Nothing
            v = g.match("d")
            If assertion.is_false(v.null_or_empty()) AndAlso
               assertion.equal(v.size(), uint32_1) Then
                assertion.equal(v(0), uint32_1)
            End If
            v = g.match("ad")
            assertion.is_true(v.null_or_empty())
            v = g.match("b")
            assertion.is_true(v.null_or_empty())
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Return case1() AndAlso
                   case2()
        End Function
    End Class
End Namespace
