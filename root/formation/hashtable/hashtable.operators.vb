﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

Partial Public Class hashtable(Of T,
                                  _UNIQUE As _boolean,
                                  _HASHER As _to_uint32(Of T),
                                  _EQUALER As _equaler(Of T))
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function find(ByVal value As T) As iterator
        Dim column As UInt32 = 0
        Dim row As UInt32 = 0
        column = hash(value)
        If find_first_cell(value, row, column) Then
            Return iterator_at(row, column)
        End If
        Return iterator.[end]
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function emplace(ByVal value As T) As tuple(Of iterator, Boolean)
        Dim row As UInt32 = 0
        Dim column As UInt32 = 0
        Dim r As Boolean = False
        r = emplace(value, row, column)
        Return tuple.emplace_of(iterator_at(row, column), r)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function insert(ByVal value As T) As tuple(Of iterator, Boolean)
        Return emplace(copy_no_error(value))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function [erase](ByVal value As T) As UInt32
        Dim r As UInt32 = 0
        Dim column As UInt32 = 0
        column = hash(value)
        For i As UInt32 = 0 To last_row()
            If cell_is(i, column, value) Then
                clear_cell(i, column)
                r += uint32_1
                If unique Then
                    Exit For
                End If
            End If
        Next
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function [erase](ByVal it As iterator) As Boolean
        If it = [end]() OrElse object_compare(it.ref().owner, Me) <> 0 OrElse it.ref().empty() Then
            Return False
        End If

        clear_cell(it.ref().row, it.ref().column)
        Return True
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub clear()
        v.clear()
        new_row()
        s = 0
    End Sub
End Class
