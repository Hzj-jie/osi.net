
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.automata
Imports osi.service.automata.rlexer

Namespace rlexer
    Public Class optional_matching_group_test
        Inherits [case]

        Private Shared Function case1() As Boolean
            Dim g As optional_matching_group = Nothing
            g = New optional_matching_group(New string_matching_group("a", "b", "c"))
            Dim v As vector(Of UInt32) = Nothing
            v = g.match("abc")
            If assertion.is_true(Not v.null_or_empty()) AndAlso
               assertion.equal(v.size(), CUInt(2)) Then
                assertion.equal(v(0), uint32_1)
                assertion.equal(v(1), uint32_0)
            End If
            Return True
        End Function

        Private Shared Function case2() As Boolean
            Dim g As optional_matching_group = Nothing
            g = New optional_matching_group(New string_matching_group("abc"))
            Dim v As vector(Of UInt32) = Nothing
            v = g.match("abc")
            If assertion.is_true(Not v.null_or_empty()) AndAlso
               assertion.equal(v.size(), CUInt(2)) Then
                assertion.equal(v(0), CUInt(3))
                assertion.equal(v(1), uint32_0)
            End If
            Return True
        End Function

        Private Shared Function case3() As Boolean
            Dim g As optional_matching_group = Nothing
            g = New optional_matching_group(New any_character_matching_group())
            Dim v As vector(Of UInt32) = Nothing
            v = g.match("abc")
            If assertion.is_true(Not v.null_or_empty()) AndAlso
               assertion.equal(v.size(), CUInt(2)) Then
                assertion.equal(v(0), uint32_1)
                assertion.equal(v(1), uint32_0)
            End If
            Return True
        End Function

        Private Shared Function case4() As Boolean
            Dim g As optional_matching_group = Nothing
            g = New optional_matching_group(New string_matching_group("abc"))
            Dim v As vector(Of UInt32) = Nothing
            v = g.match("bcd")
            If assertion.is_true(Not v.null_or_empty()) AndAlso
               assertion.equal(v.size(), uint32_1) Then
                assertion.equal(v(0), uint32_0)
            End If
            Return True
        End Function

        Private Shared Function case5() As Boolean
            Dim g As optional_matching_group = Nothing
            g = New optional_matching_group(New any_character_matching_group())
            Dim v As vector(Of UInt32) = Nothing
            v = g.match("")
            If assertion.is_true(Not v.null_or_empty()) AndAlso
               assertion.equal(v.size(), uint32_1) Then
                assertion.equal(v(0), uint32_0)
            End If
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
