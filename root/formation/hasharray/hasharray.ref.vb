﻿
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

        Public Sub New(ByVal owner As hasharray(Of T, _UNIQUE, _HASHER, _EQUALER), ByVal column As UInt32)
            assert(Not owner Is Nothing)
            assert(column < owner.column_count())
            Me.owner = owner
            Me.column = column
        End Sub

        Public Function ref_at(ByVal column As UInt32) As ref
            Return owner.ref_at(column)
        End Function

        Public Function column_count() As UInt32
            Return owner.column_count()
        End Function

        Public Function empty() As Boolean
            Return owner.v(column) Is Nothing
        End Function

        Public Function is_equal_to(ByVal that As ref) As Boolean
            If that Is Nothing Then
                Return False
            End If
            Return object_compare(owner, that.owner) = 0 AndAlso
                   column = that.column
        End Function

        Public Shared Operator +(ByVal this As ref) As T
            If this Is Nothing OrElse this.empty() Then
                Return Nothing
            End If

            Return +this.owner.v(this.column)
        End Operator
    End Class
End Class
