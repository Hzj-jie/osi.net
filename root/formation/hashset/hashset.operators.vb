
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

Partial Public Class hashset(Of T,
                                _UNIQUE As _boolean,
                                _HASHER As _to_uint32(Of T),
                                _EQUALER As _equaler(Of T))
    Public Function find(ByVal value As T) As iterator
        Dim row As UInt32 = 0
        Dim column As [set](Of T).iterator = Nothing
        row = hash(value)
        If find_cell(value, row, column) Then
            Return iterator_at(row, column)
        End If
        Return iterator.[end]
    End Function

    Public Function emplace(ByVal value As T) As pair(Of iterator, Boolean)
        Dim row As UInt32 = 0
        Dim column As [set](Of T).iterator = Nothing
        Dim r As Boolean = False
        r = emplace(value, row, column)
        Return pair.emplace_of(iterator_at(row, column), r)
    End Function

    Public Function insert(ByVal value As T) As pair(Of iterator, Boolean)
        Return emplace(copy_no_error(value))
    End Function

    Public Function [erase](ByVal value As T) As UInt32
        If clear_cell(hash(value), value) Then
            Return uint32_1
        End If
        Return uint32_0
    End Function

    Public Function [erase](ByVal it As iterator) As Boolean
        If it = [end]() OrElse object_compare(it.ref().owner, Me) <> 0 Then
            Return False
        End If

        clear_cell(it.ref().row, it.ref().column)
        Return True
    End Function

    Public Sub clear()
        reset_array()
        s = 0
    End Sub
End Class
