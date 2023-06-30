
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.automata
Imports osi.service.automata.rlexer

Namespace rlexer
    Public Class any_character_matching_group_test
        Inherits [case]

        Private Shared Function case1() As Boolean
            Dim g As any_character_matching_group = Nothing
            g = New any_character_matching_group()
            Dim v As vector(Of UInt32) = Nothing
            If assertion.is_true(g.match("abc", v)) AndAlso
               assertion.is_true(Not v.null_or_empty()) AndAlso
               assertion.equal(v.size(), uint32_1) Then
                assertion.equal(v(0), uint32_1)
            End If
            Return True
        End Function

        Private Shared Function case2() As Boolean
            Dim g As any_character_matching_group = Nothing
            g = New any_character_matching_group()
            assertion.is_false(g.match("", New vector(Of UInt32)()))
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Return case1() AndAlso
                   case2()
        End Function
    End Class
End Namespace
