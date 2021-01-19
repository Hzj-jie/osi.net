
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt

Public NotInheritable Class map_test2
    Inherits [case]

    Private Shared ReadOnly first_round() As pair(Of String, String) =
        {
            pair.emplace_of("herald", "http-post"),
            pair.emplace_of("host", "localhost"),
            pair.emplace_of("port", "11001"),
            pair.emplace_of("target", "fces")
        }

    Private Shared ReadOnly second_round() As pair(Of String, String) =
        {
            pair.emplace_of("herald", "http-post")
        }

    Public Overrides Function run() As Boolean
        Dim m As map(Of String, String) = Nothing
        m = New map(Of String, String)()
        For i As Int32 = 0 To array_size_i(first_round) - 1
            Dim p As tuple(Of map(Of String, String).iterator, Boolean) = Nothing
            p = m.emplace(first_round(i).first, first_round(i).second)
            assertion.is_true(p.second)
            assertion.is_not_null(p.first)
            assertion.not_equal(p.first, m.end())
        Next
        For i As Int32 = 0 To array_size_i(second_round) - 1
            assertion.is_true(m.erase(second_round(i).first))
        Next
        For i As Int32 = 0 To array_size_i(second_round) - 1
            Dim p As tuple(Of map(Of String, String).iterator, Boolean) = Nothing
            p = m.emplace(second_round(i).first, second_round(i).second)
            assertion.is_true(p.second)
            assertion.is_not_null(p.first)
            assertion.not_equal(p.first, m.end())
        Next
        For i As Int32 = 0 To array_size_i(first_round) - 1
            Dim it As map(Of String, String).iterator
            it = m.find(first_round(i).first)
            If assertion.is_not_null(it) AndAlso
               assertion.is_false(it.is_end()) Then
                assertion.equal((+it).second, first_round(i).second)
            End If
        Next
        For i As Int32 = 0 To array_size_i(second_round) - 1
            Dim it As map(Of String, String).iterator
            it = m.find(second_round(i).first)
            If assertion.is_not_null(it) AndAlso
               assertion.is_false(it.is_end()) Then
                assertion.equal((+it).second, second_round(i).second)
            End If
        Next

        Dim m2 As map(Of String, String) = Nothing
        copy(m2, m)
        For i As Int32 = 0 To array_size_i(first_round) - 1
            assertion.is_true(m2.erase(first_round(i).first))
            Dim p As tuple(Of map(Of String, String).iterator, Boolean) = Nothing
            p = m2.emplace(first_round(i).first, first_round(i).second)
            assertion.is_true(p.second)
            assertion.is_not_null(p.first)
            assertion.not_equal(p.first, m.end())
        Next
        For i As Int32 = 0 To array_size_i(second_round) - 1
            assertion.is_true(m2.erase(second_round(i).first))
            Dim p As tuple(Of map(Of String, String).iterator, Boolean) = Nothing
            p = m2.emplace(second_round(i).first, second_round(i).second)
            assertion.is_true(p.second)
            assertion.is_not_null(p.first)
            assertion.not_equal(p.first, m.end())
        Next
        For i As Int32 = 0 To array_size_i(first_round) - 1
            Dim it As map(Of String, String).iterator
            it = m2.find(first_round(i).first)
            If assertion.is_not_null(it) AndAlso
               assertion.is_false(it.is_end()) Then
                assertion.equal((+it).second, first_round(i).second)
            End If
        Next
        For i As Int32 = 0 To array_size_i(second_round) - 1
            Dim it As map(Of String, String).iterator
            it = m2.find(second_round(i).first)
            If assertion.is_not_null(it) AndAlso
               assertion.is_false(it.is_end()) Then
                assertion.equal((+it).second, second_round(i).second)
            End If
        Next
        Return True
    End Function
End Class
