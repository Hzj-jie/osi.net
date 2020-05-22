
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
        column = hash(value)
        If find_first_cell(value, column, row) Then
            Return iterator_at(column, row)
        End If
        Return iterator.end
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function emplace(ByVal value As T) As pair(Of iterator, Boolean)
        Dim column As UInt32 = 0
        Dim row As UInt32 = 0
        Dim r As Boolean = False
        r = emplace(value, column, row)
        Return pair.emplace_of(iterator_at(column, row), r)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function insert(ByVal value As T) As pair(Of iterator, Boolean)
        Return emplace(copy_no_error(value))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function [erase](ByVal value As T) As UInt32
        Dim r As UInt32 = 0
        Dim column As UInt32 = 0
        Dim row As UInt32 = 0
        column = hash(value)
        While row < row_count(column)
            If cell_is(column, row, value) Then
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
    Public Function [erase](ByVal it As iterator) As Boolean
        If it = [end]() OrElse object_compare(it.ref().owner, Me) <> 0 OrElse it.ref().empty() Then
            Return False
        End If

        clear_cell(it.ref().column, it.ref().row)
        Return True
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub clear()
        reset_array()
        s = 0
    End Sub
End Class
