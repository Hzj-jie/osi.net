
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

Partial Public Class hashmap(Of KEY_T As IComparable(Of KEY_T),
                                VALUE_T,
                                _KEY_TO_INDEX As _to_uint32(Of KEY_T))
    Public Class iterator
        Implements IComparable(Of iterator), IComparable

        Private ReadOnly _container As hashmap(Of KEY_T, VALUE_T, _KEY_TO_INDEX) = Nothing
        Private ReadOnly it As map(Of KEY_T, VALUE_T).iterator
        Private ReadOnly _index As UInt32

        Friend Function index() As UInt32
            Return _index
        End Function

        Friend Function container() As hashmap(Of KEY_T, VALUE_T, _KEY_TO_INDEX)
            Return _container
        End Function

        Friend Function iterator() As map(Of KEY_T, VALUE_T).iterator
            Return it
        End Function

        Public Function key() As KEY_T
            Return (+it).first
        End Function

        Public Function value() As VALUE_T
            Return (+it).second
        End Function

        Private Function move_next(ByVal index As UInt32, ByVal it As map(Of KEY_T, VALUE_T).iterator) As iterator
            assert(Not container() Is Nothing)
            assert(Not it.is_null())
            it += 1
            If it = container().data(index, False).end() Then
                index = container().next_index(index)
                If Not container().valid_index(index) Then
                    Return container().end()
                End If
                it = container().data(index, False).begin()
            End If
            Return New iterator(container, index, it)
        End Function

        Private Function move_prev(ByVal index As UInt32, ByVal it As map(Of KEY_T, VALUE_T).iterator) As iterator
            assert(Not container() Is Nothing)
            assert(Not it.is_null())
            it -= 1
            If it = container().data(index, False).end() Then
                index = container().prev_index(index)
                If Not container().valid_index(index) Then
                    Return container().end()
                End If
                it = container().data(index, False).begin()
            End If
            Return New iterator(container, index, it)
        End Function

        Private Function move_next(ByVal that As UInt32) As iterator
            assert(that > 0)
            Dim r As iterator = Nothing
            r = Me
            For i As UInt32 = 0 To that - uint32_1
                r = move_next(r.index(), r.iterator())
            Next
            Return r
        End Function

        Private Function move_prev(ByVal that As UInt32) As iterator
            assert(that > 0)
            Dim r As iterator = Nothing
            r = Me
            For i As UInt32 = 0 To that - uint32_1
                r = move_prev(r.index(), r.iterator())
            Next
            Return r
        End Function

        Public Shared Operator +(ByVal this As iterator, ByVal that As Int32) As iterator
            If this Is Nothing OrElse this = this.container().end() OrElse that = 0 Then
                Return this
            End If
            If that > 0 Then
                Return this.move_next(CUInt(that))
            End If
            assert(that < 0)
            Return this.move_prev(CUInt(-that))
        End Operator

        Public Shared Operator -(ByVal this As iterator, ByVal that As Int32) As iterator
            If this Is Nothing OrElse this = this.container().end() OrElse that = 0 Then
                Return this
            End If
            If that > 0 Then
                Return this.move_prev(CUInt(that))
            End If
            assert(that < 0)
            Return this.move_next(CUInt(-that))
        End Operator

        Public Shared Operator +(ByVal this As iterator) As first_const_pair(Of KEY_T, VALUE_T)
            If this Is Nothing Then
                Return Nothing
            End If
            Return +(this.it)
        End Operator

        Public Shared Operator =(ByVal this As iterator, ByVal that As iterator) As Boolean
            Dim c As Int32 = 0
            c = object_compare(this, that)
            If c = object_compare_undetermined Then
                assert(Not this Is Nothing)
                assert(Not that Is Nothing)
                Return (this.is_end() AndAlso that.is_end()) OrElse
                       (object_compare(this.container(), that.container()) = 0 AndAlso
                        this.index() = that.index() AndAlso
                        this.iterator() = that.iterator())
            End If
            Return (c = 0) OrElse
                   (this Is Nothing AndAlso that.is_end()) OrElse
                   (that Is Nothing AndAlso this.is_end())
        End Operator

        Public Shared Operator <>(ByVal this As iterator, ByVal that As iterator) As Boolean
            Return Not (this = that)
        End Operator

        Public Function is_end() As Boolean
            Return container() Is Nothing OrElse container().invalid_index(index()) OrElse iterator().is_end()
        End Function

        Public Function is_not_end() As Boolean
            Return Not is_end()
        End Function

        Friend Sub New(ByVal container As hashmap(Of KEY_T, VALUE_T, _KEY_TO_INDEX),
                       ByVal index As UInt32,
                       ByVal that As map(Of KEY_T, VALUE_T).iterator)
            _container = container
            _index = index
            it = that
        End Sub

        Public Function CompareTo(ByVal other As iterator) As Int32 Implements IComparable(Of iterator).CompareTo
            Return If(Me = other, 0, -1)
        End Function

        Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
            Return CompareTo(cast(Of iterator)(obj, False))
        End Function
    End Class
End Class
