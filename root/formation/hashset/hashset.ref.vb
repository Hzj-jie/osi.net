
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.template

Partial Public Class hashset(Of T,
                                _UNIQUE As _boolean,
                                _HASHER As _to_uint32(Of T),
                                _EQUALER As _equaler(Of T))
    Friend Class ref
        Public ReadOnly owner As hashset(Of T, _UNIQUE, _HASHER, _EQUALER)
        Public ReadOnly row As UInt32
        Public ReadOnly column As [set](Of T).iterator

        Public Sub New(ByVal owner As hashset(Of T, _UNIQUE, _HASHER, _EQUALER),
                       ByVal row As UInt32,
                       ByVal column As [set](Of T).iterator)
            assert(Not owner Is Nothing)
            assert(row < owner.row_count())
            Me.owner = owner
            Me.row = row
            Me.column = column
        End Sub

        Public Function ref_at(ByVal row As UInt32, ByVal column As [set](Of T).iterator) As ref
            Return owner.ref_at(row, column)
        End Function

        Public Function empty() As Boolean
            Return column.is_end()
        End Function

        Public Function is_equal_to(ByVal that As ref) As Boolean
            If that Is Nothing Then
                Return False
            End If
            Return object_compare(owner, that.owner) = 0 AndAlso
                   row = that.row AndAlso
                   column = that.column
        End Function

        Public Function row_set(ByVal r As UInt32) As [set](Of T)
            Return owner.v(r)
        End Function

        Public Function row_count() As UInt32
            Return owner.row_count()
        End Function

        Public Function last_row() As UInt32
            Return owner.last_row()
        End Function

        Public Shared Operator +(ByVal this As ref) As T
            If this Is Nothing OrElse this.empty() Then
                Return Nothing
            End If
            Return +this.column
        End Operator
    End Class
End Class
