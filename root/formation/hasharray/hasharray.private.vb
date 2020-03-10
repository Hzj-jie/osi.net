
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

Partial Public Class hasharray(Of T,
                                  _UNIQUE As _boolean,
                                  _HASHER As _to_uint32(Of T),
                                  _EQUALER As _equaler(Of T))
    Private Function hash(ByVal v As T) As UInt32
        Return hasher(v) Mod column_count()
    End Function

    Private Function hash(ByVal v As hasher_node(Of T)) As UInt32
        assert(Not v Is Nothing)
        Return v.hash_code() Mod column_count()
    End Function

    Private Function column_count() As UInt32
        Return predefined_column_counts(c)
    End Function

    Private Sub reset_array()
        v = New array(Of vector(Of hasher_node(Of T)))(column_count())
        For i As UInt32 = 0 To v.size() - uint32_1
            v(i) = New vector(Of hasher_node(Of T))()
        Next
    End Sub

    Private Function average_row_count() As UInt32
        Return size() \ column_count()
    End Function

    Private Function row_count(ByVal column As UInt32) As UInt32
        Return v(column).size()
    End Function

    Private Function empty_column(ByVal column As UInt32) As Boolean
        Return row_count(column) = uint32_0
    End Function

    Private Function last_row(ByVal column As UInt32) As UInt32
        assert(Not empty_column(column))
        Return row_count(column) - uint32_1
    End Function

    Private Sub set_cell(ByVal column As UInt32, ByVal row As UInt32, ByVal value As hasher_node(Of T))
        assert(row <= max_int32)
        v(column).data()(CInt(row)) = value
    End Sub

    Private Sub add_cell(ByVal column As UInt32, ByVal row As UInt32, ByVal value As hasher_node(Of T))
        assert(cell_is_empty(column, row))
        set_cell(column, row, value)
        s += uint32_1
    End Sub

    Private Sub emplace_back(ByVal column As UInt32, ByVal value As hasher_node(Of T))
        assert(Not value Is Nothing)
        v(column).emplace_back(value)
        s += uint32_1
    End Sub

    Private Function new_node(ByVal value As T) As hasher_node(Of T)
        Return New hasher_node(Of T)(value, hasher)
    End Function

    Private Sub clear_cell(ByVal column As UInt32, ByVal row As UInt32)
        assert(Not cell_is_empty(column, row))
        set_cell(column, row, [default](Of hasher_node(Of T)).null)
        s -= uint32_1
    End Sub

    Private Function first_non_empty_column() As UInt32
        assert(Not empty())
        For i As UInt32 = 0 To v.size() - uint32_1
            If Not v(i).empty() Then
                Return i
            End If
        Next
        assert(False)
        Return max_uint32
    End Function

    Private Function last_non_empty_column() As UInt32
        assert(Not empty())
        Dim i As UInt32 = 0
        i = v.size()
        While i > uint32_0
            i -= uint32_1
            If Not v(i).empty() Then
                Return i
            End If
        End While
        assert(False)
        Return max_uint32
    End Function

    Private Function cell_is(ByVal column As UInt32, ByVal row As UInt32, ByVal value As T) As Boolean
        Return Not cell_is_empty(column, row) AndAlso equaler(v(column)(row).get(), value)
    End Function

    Private Function cell_is_empty(ByVal column As UInt32, ByVal row As UInt32) As Boolean
        Return v(column)(row) Is Nothing
    End Function

    Private Function iterator_at(ByVal column As UInt32, ByVal row As UInt32) As iterator
        Return New iterator(Me, column, row)
    End Function

    Private Function ref_at(ByVal column As UInt32, ByVal row As UInt32) As ref
        Return New ref(Me, column, row)
    End Function

    Private Function find_first_cell(ByVal value As T, ByVal column As UInt32, ByRef row As UInt32) As Boolean
        row = 0
        While row < row_count(column)
            If cell_is(column, row, value) Then
                Return True
            End If
            row += uint32_1
        End While
        Return False
    End Function

    Private Function find_empty_cell(ByVal column As UInt32, ByRef row As UInt32) As Boolean
        row = 0
        While row < row_count(column)
            If cell_is_empty(column, row) Then
                Return True
            End If
            row += uint32_1
        End While
        Return False
    End Function

    Private Function emplace(ByVal value As hasher_node(Of T), ByRef column As UInt32, ByRef row As UInt32) As Boolean
        column = hash(value)
        If unique AndAlso find_first_cell(+value, column, row) Then
            Return False
        End If

        If find_empty_cell(column, row) Then
            add_cell(column, row, value)
            Return True
        End If

        If should_rehash() AndAlso rehash() Then
            Return emplace(value, column, row)
        End If

        emplace_back(column, value)
        row = last_row(column)
        Return True
    End Function

    Private Function emplace(ByVal value As T, ByRef column As UInt32, ByRef row As UInt32) As Boolean
        Return emplace(new_node(value), column, row)
    End Function

    Private Function should_rehash() As Boolean
        Return average_row_count() >= row_count_upper_bound
    End Function

    Private Sub rehash_move_in(ByVal c As hasher_node(Of T))
        assert(Not c Is Nothing)
        Dim column As UInt32 = 0
        column = hash(c)
        If isdebugmode() Then
            assert(Not find_first_cell(+c, column, uint32_0))
        End If
        emplace_back(column, c)
    End Sub

    Private Function rehash() As Boolean
        If c = predefined_column_counts.size() - uint32_1 Then
            Return False
        End If

        Dim r As hasharray(Of T, _UNIQUE, _HASHER, _EQUALER) = Nothing
        r = New hasharray(Of T, _UNIQUE, _HASHER, _EQUALER)(c + uint32_1)
        For i As UInt32 = 0 To v.size() - uint32_1
            Dim j As UInt32 = 0
            While j < row_count(i)
                If Not cell_is_empty(i, j) Then
                    r.rehash_move_in(v(i)(j))
                End If
                j += uint32_1
            End While
        Next
        swap(r, Me)
        Return True
    End Function
End Class
