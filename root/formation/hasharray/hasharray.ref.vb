
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
    Friend Structure ref
        Public ReadOnly owner As hasharray(Of T, _UNIQUE, _HASHER, _EQUALER)
        Public ReadOnly column As UInt32
        Public ReadOnly row As UInt32

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub New(ByVal owner As hasharray(Of T, _UNIQUE, _HASHER, _EQUALER),
                       ByVal column As UInt32,
                       ByVal row As UInt32)
#If DEBUG Then
            assert(Not owner Is Nothing)
#End If
            Me.owner = owner
#If DEBUG Then
            assert(column < column_count())
#End If
            Me.column = column
#If DEBUG Then
            assert(row < row_count(column))
#End If
            Me.row = row
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function ref_at(ByVal column As UInt32, ByVal row As UInt32) As ref
            Return owner.ref_at(column, row)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function column_count() As UInt32
            Return owner.column_count()
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function row_count(ByVal column As UInt32) As UInt32
            Return owner.row_count(column)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function empty() As Boolean
            Return owner.cell_is_empty(column, row)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function is_equal_to(ByVal that As ref) As Boolean
            Return object_compare(owner, that.owner) = 0 AndAlso
                   column = that.column AndAlso
                   row = that.row
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function is_end() As Boolean
            Return owner Is Nothing
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Shared Operator +(ByVal this As ref) As T
            Return +this.owner.v(this.column)(this.row)
        End Operator
    End Structure
End Class
