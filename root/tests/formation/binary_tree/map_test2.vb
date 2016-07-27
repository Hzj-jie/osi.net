
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt

Public Class map_test2
    Inherits [case]

    Private Shared ReadOnly first_round() As pair(Of String, String) =
        {
            emplace_make_pair("herald", "http-post"),
            emplace_make_pair("host", "localhost"),
            emplace_make_pair("port", "11001"),
            emplace_make_pair("target", "fces")
        }

    Private Shared ReadOnly second_round() As pair(Of String, String) =
        {
            emplace_make_pair("herald", "http-post")
        }

    Public Overrides Function run() As Boolean
        Dim m As map(Of String, String) = Nothing
        m = New map(Of String, String)()
        Dim it As map(Of String, String).iterator = Nothing
        For i As Int32 = 0 To array_size(first_round) - 1
            it = m.emplace(first_round(i).first, first_round(i).second)
            assert_not_nothing(it)
            assert_false(it.is_end())
        Next
        For i As Int32 = 0 To array_size(second_round) - 1
            assert_true(m.erase(second_round(i).first))
        Next
        For i As Int32 = 0 To array_size(second_round) - 1
            it = m.emplace(second_round(i).first, second_round(i).second)
            assert_not_nothing(it)
            assert_false(it.is_end())
        Next
        For i As Int32 = 0 To array_size(first_round) - 1
            it = m.find(first_round(i).first)
            If assert_not_nothing(it) AndAlso
               assert_false(it.is_end()) Then
                assert_equal((+it).second, first_round(i).second)
            End If
        Next
        For i As Int32 = 0 To array_size(second_round) - 1
            it = m.find(second_round(i).first)
            If assert_not_nothing(it) AndAlso
               assert_false(it.is_end()) Then
                assert_equal((+it).second, second_round(i).second)
            End If
        Next

        Dim m2 As map(Of String, String) = Nothing
        copy(m2, m)
        For i As Int32 = 0 To array_size(first_round) - 1
            assert_true(m2.erase(first_round(i).first))
            it = m2.emplace(first_round(i).first, first_round(i).second)
            assert_not_nothing(it)
            assert_false(it.is_end())
        Next
        For i As Int32 = 0 To array_size(second_round) - 1
            assert_true(m2.erase(second_round(i).first))
            it = m2.emplace(second_round(i).first, second_round(i).second)
            assert_not_nothing(it)
            assert_false(it.is_end())
        Next
        For i As Int32 = 0 To array_size(first_round) - 1
            it = m2.find(first_round(i).first)
            If assert_not_nothing(it) AndAlso
               assert_false(it.is_end()) Then
                assert_equal((+it).second, first_round(i).second)
            End If
        Next
        For i As Int32 = 0 To array_size(second_round) - 1
            it = m2.find(second_round(i).first)
            If assert_not_nothing(it) AndAlso
               assert_false(it.is_end()) Then
                assert_equal((+it).second, second_round(i).second)
            End If
        Next
        Return True
    End Function
End Class
