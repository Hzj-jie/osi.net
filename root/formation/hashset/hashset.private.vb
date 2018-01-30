
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.template

Partial Public Class hashset(Of T,
                                _UNIQUE As _boolean,
                                _HASHER As _to_uint32(Of T),
                                _EQUALER As _equaler(Of T))
    Private Function hash(ByVal v As T) As UInt32
        Return hasher(v) Mod row_count()
    End Function

    Private Sub reset_array()
        v = New array(Of [set](Of T))(row_count())
        For i As UInt32 = 0 To v.size() - uint32_1
            v(i) = New [set](Of T)()
        Next
    End Sub

    Private Function row_count() As UInt32
        Return predefined_row_counts(c)
    End Function

    Private Function last_row() As UInt32
        Return row_count() - uint32_1
    End Function

    Private Function average_column_count() As UInt32
        Return size() \ row_count()
    End Function

    Private Function set_cell(ByVal row As UInt32, ByVal value As T) As [set](Of T).iterator
        Dim r As pair(Of [set](Of T).iterator, Boolean) = Nothing
        r = v(row).emplace(value)
        assert(r.second)
        s += uint32_1
        Return r.first
    End Function

    Private Function clear_cell(ByVal row As UInt32, ByVal value As T) As Boolean
        Dim column As [set](Of T).iterator = Nothing
        If Not find_cell(value, row, column) Then
            Return False
        End If
        clear_cell(row, column)
        Return True
    End Function

    Private Sub clear_cell(ByVal row As UInt32, ByVal column As [set](Of T).iterator)
        assert(v(row).erase(column))
        s -= uint32_1
    End Sub

    Private Function ref_at(ByVal row As UInt32, ByVal column As [set](Of T).iterator) As ref
        Return New ref(Me, row, column)
    End Function

    Private Function iterator_at(ByVal row As UInt32, ByVal column As [set](Of T).iterator) As iterator
        Return New iterator(Me, row, column)
    End Function

    Private Sub rehash()
        If c = predefined_row_counts.size() - uint32_1 Then
            Return
        End If

        Dim r As hashset(Of T, _UNIQUE, _HASHER, _EQUALER) = Nothing
        r = New hashset(Of T, _UNIQUE, _HASHER, _EQUALER)(c + uint32_1)
        Dim it As iterator = Nothing
        it = begin()
        While it <> [end]()
            assert(r.emplace(+it, uint32_0, Nothing))
            it += 1
        End While
        swap(r, Me)
    End Sub

    Private Function should_rehash() As Boolean
        Return average_column_count() >= row_count_upper_bound
    End Function

    Private Function find_cell(ByVal value As T, ByVal row As UInt32, ByRef column As [set](Of T).iterator) As Boolean
        column = v(row).find(value)
        Return column.is_not_end()
    End Function

    Private Function emplace(ByVal value As T, ByRef row As UInt32, ByRef column As [set](Of T).iterator) As Boolean
        row = hash(value)
        If find_cell(value, row, column) Then
            Return False
        End If

        column = set_cell(row, value)

        If should_rehash() Then
            rehash()
        End If
        Return True
    End Function
End Class
