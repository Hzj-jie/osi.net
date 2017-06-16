
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.template

Partial Public Class hashtable(Of T,
                                  _UNIQUE As _boolean,
                                  _HASHER As _to_uint32(Of T),
                                  _COMPARER As _comparer(Of T))
    Private Function hash(ByVal v As T) As UInt32
        Return hasher(v) Mod column_count()
    End Function

    Private Sub new_row()
        v.emplace_back(New array(Of constant(Of T))(column_count()))
    End Sub

    Private Function column_count() As UInt32
        Return predefined_column_counts(c)
    End Function

    Private Function last_column() As UInt32
        Return column_count() - uint32_1
    End Function

    Private Function row_count() As UInt32
        Return v.size()
    End Function

    Private Function last_row() As UInt32
        Return row_count() - uint32_1
    End Function

    Private Function cell_count() As UInt32
        Return row_count() * column_count()
    End Function

    Private Function cell_id(ByVal row As UInt32, ByVal column As UInt32) As UInt32
        Return row * column_count() + column
    End Function

    Private Function row(ByVal cell_id As UInt32) As UInt32
        Return cell_id \ column_count()
    End Function

    Private Function column(ByVal cell_id As UInt32) As UInt32
        Return cell_id Mod column_count()
    End Function

    Private Function cell(ByVal row As UInt32, ByVal column As UInt32) As constant(Of T)
        Return v(row)(column)
    End Function

    Private Function cell(ByVal id As UInt32) As constant(Of T)
        Return cell(row(id), column(id))
    End Function

    Private Sub set_cell(ByVal row As UInt32, ByVal column As UInt32, ByVal value As T)
        v(row)(column) = constant.[New](value)
        s += uint32_1
    End Sub

    Private Sub clear_cell(ByVal row As UInt32, ByVal column As UInt32)
        v(row)(column) = Nothing
        s -= uint32_1
    End Sub

    Private Function ref_at(ByVal row As UInt32, ByVal column As UInt32) As ref
        Return New ref(Me, row, column)
    End Function

    Private Function ref_at(ByVal id As UInt32) As ref
        Return New ref(Me, row(id), column(id))
    End Function

    Private Function iterator_at(ByVal row As UInt32, ByVal column As UInt32) As iterator
        Return New iterator(Me, row, column)
    End Function

    Private Function rehash() As Boolean
        If c = predefined_column_counts.size() - uint32_1 Then
            Return False
        End If

        Dim r As hashtable(Of T, _UNIQUE, _HASHER, _COMPARER) = Nothing
        r = New hashtable(Of T, _UNIQUE, _HASHER, _COMPARER)(c + uint32_1)
        Dim it As iterator = Nothing
        it = begin()
        While it <> [end]()
            assert(r.emplace(+it))
            it += 1
        End While
        move_to(r, Me)
        Return True
    End Function
End Class
