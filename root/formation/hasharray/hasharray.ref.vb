
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.template

Partial Public Class hasharray(Of T,
                                  _UNIQUE As _boolean,
                                  _HASHER As _to_uint32(Of T),
                                  _EQUALER As _equaler(Of T))
    Friend Class ref
        Public ReadOnly owner As hasharray(Of T, _UNIQUE, _HASHER, _EQUALER)
        Public ReadOnly column As UInt32
        Public ReadOnly row As UInt32

        Public Sub New(ByVal owner As hasharray(Of T, _UNIQUE, _HASHER, _EQUALER),
                       ByVal column As UInt32,
                       ByVal row As UInt32)
            assert(Not owner Is Nothing)
            Me.owner = owner
            assert(column < column_count())
            assert(row < row_count(column))
            Me.column = column
            Me.row = row
        End Sub

        Public Function ref_at(ByVal column As UInt32, ByVal row As UInt32) As ref
            Return owner.ref_at(column, row)
        End Function

        Public Function column_count() As UInt32
            Return owner.column_count()
        End Function

        Public Function row_count(ByVal column As UInt32) As UInt32
            Return owner.row_count(column)
        End Function

        Public Function empty() As Boolean
            Return owner.cell_is_empty(column, row)
        End Function

        Public Function is_equal_to(ByVal that As ref) As Boolean
            If that Is Nothing Then
                Return False
            End If
            Return object_compare(owner, that.owner) = 0 AndAlso
                   column = that.column AndAlso
                   row = that.row
        End Function

        Public Shared Operator +(ByVal this As ref) As T
            If this Is Nothing OrElse this.empty() Then
                Return Nothing
            End If

            Return +this.owner.v(this.column)(this.row)
        End Operator
    End Class
End Class
