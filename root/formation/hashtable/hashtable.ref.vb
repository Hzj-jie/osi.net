
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
    Friend Class ref
        Public ReadOnly owner As hashtable(Of T, _UNIQUE, _HASHER, _EQUALER)
        Public ReadOnly row As UInt32
        Public ReadOnly column As UInt32

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub New(ByVal owner As hashtable(Of T, _UNIQUE, _HASHER, _EQUALER),
                       ByVal row As UInt32,
                       ByVal column As UInt32)
            assert(Not owner Is Nothing)
            assert(row < owner.row_count())
            assert(column < owner.column_count())
            Me.owner = owner
            Me.row = row
            Me.column = column
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function cell_id() As UInt32
            Return owner.cell_id(row, column)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function ref_at(ByVal id As UInt32) As ref
            Return owner.ref_at(id)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function cell_count() As UInt32
            Return owner.cell_count()
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function cell(ByVal id As UInt32) As hasher_node(Of T)
            Return owner.cell(id)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function cell() As hasher_node(Of T)
            Return owner.cell(row, column)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function empty() As Boolean
            Return owner.cell_is_empty(row, column)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function is_equal_to(ByVal that As ref) As Boolean
            If that Is Nothing Then
                Return False
            End If
            Return object_compare(owner, that.owner) = 0 AndAlso
                   row = that.row AndAlso
                   column = that.column
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Shared Operator +(ByVal this As ref) As T
            If this Is Nothing OrElse this.empty() Then
                Return Nothing
            End If
            Return +this.owner.cell(this.row, this.column)
        End Operator
    End Class
End Class
