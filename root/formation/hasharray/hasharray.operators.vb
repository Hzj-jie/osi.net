
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

Partial Public Class hasharray(Of T,
                                  _UNIQUE As _boolean,
                                  _HASHER As _to_uint32(Of T),
                                  _EQUALER As _equaler(Of T))
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function find(ByVal value As T) As iterator
        Dim column As UInt32 = 0
        Dim row As UInt32 = 0
        Dim n As hasher_node = new_node(value)
        column = hash(n)
        If find_first_cell(n, column, row) Then
            Return iterator_at(column, row)
        End If
        Return iterator.end
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function emplace(ByVal value As T) As tuple(Of iterator, Boolean)
        Dim column As UInt32 = 0
        Dim row As UInt32 = 0
        Dim r As Boolean = emplace(value, column, row)
        Return tuple.emplace_of(iterator_at(column, row), r)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function insert(ByVal value As T) As tuple(Of iterator, Boolean)
        Return emplace(copy_no_error(value))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function [erase](ByVal value As T) As UInt32
        Dim r As UInt32 = 0
        Dim row As UInt32 = 0
        Dim n As hasher_node = new_node(value)
        Dim column As UInt32 = hash(n)
        Dim rc As UInt32 = row_count(column)
        While row < rc
            If cell_is(column, row, n) Then
                clear_cell(column, row)
                r += uint32_1
                If unique Then
                    Exit While
                End If
            End If
            row += uint32_1
        End While
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function [erase](ByVal it As iterator) As iterator
        If it = [end]() OrElse object_compare(it.ref().owner, Me) <> 0 OrElse it.ref().empty() Then
            Return [end]()
        End If

        clear_cell(it.ref().column, it.ref().row)
        Return it + 1
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub clear()
        reset_array()
        s = 0
    End Sub
End Class
