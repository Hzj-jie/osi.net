
Imports osi.root.template
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Public Class hashmap(Of KEY_T As IComparable(Of KEY_T), VALUE_T)
    Inherits hashmap(Of KEY_T, VALUE_T, _1023)
    Implements ICloneable(Of hashmap(Of KEY_T, VALUE_T))

    Public Shadows Function CloneT() As hashmap(Of KEY_T, VALUE_T) _
                                     Implements ICloneable(Of hashmap(Of KEY_T, VALUE_T)).Clone
        Return direct_cast(Of hashmap(Of KEY_T, VALUE_T))(MyBase.CloneT())
    End Function
End Class

Public Class hashmap(Of KEY_T As IComparable(Of KEY_T), VALUE_T, HASH_SIZE As _int64)
    Inherits hashmap(Of KEY_T, VALUE_T, HASH_SIZE, default_to_uint32(Of KEY_T))
    Implements ICloneable(Of hashmap(Of KEY_T, VALUE_T, HASH_SIZE))

    Public Shadows Function CloneT() As hashmap(Of KEY_T, VALUE_T, HASH_SIZE) _
                                     Implements ICloneable(Of hashmap(Of KEY_T, VALUE_T, HASH_SIZE)).Clone
        Return direct_cast(Of hashmap(Of KEY_T, VALUE_T, HASH_SIZE))(MyBase.CloneT())
    End Function
End Class

Public Class hashmap(Of KEY_T As IComparable(Of KEY_T),
                        VALUE_T,
                        _HASH_SIZE As _int64,
                        _KEY_TO_INDEX As _to_uint32(Of KEY_T))
    Implements ICloneable, ICloneable(Of hashmap(Of KEY_T, VALUE_T, _HASH_SIZE, _KEY_TO_INDEX))

    Private Shared ReadOnly _end As iterator = Nothing
    Private Shared ReadOnly hash_size As UInt32 = 0
    Private Shared ReadOnly key_to_index As _KEY_TO_INDEX = Nothing
    Private _data() As map(Of KEY_T, VALUE_T)

    Public Class iterator
        Implements IComparable(Of iterator), IComparable

        Private ReadOnly _container As hashmap(Of KEY_T, VALUE_T, _HASH_SIZE, _KEY_TO_INDEX) = Nothing
        Private ReadOnly it As map(Of KEY_T, VALUE_T).iterator
        Private ReadOnly _index As UInt32

        Friend Function index() As UInt32
            Return _index
        End Function

        Friend Function container() As hashmap(Of KEY_T, VALUE_T, _HASH_SIZE, _KEY_TO_INDEX)
            Return _container
        End Function

        Friend Function iterator() As map(Of KEY_T, VALUE_T).iterator
            Return it
        End Function

        Public Function key() As KEY_T
            Dim rtn As KEY_T = Nothing
            copy(rtn, (+it).first)
            Return rtn
        End Function

        Public Function value() As VALUE_T
            Dim rtn As VALUE_T = Nothing
            copy(rtn, (+it).second)
            Return rtn
        End Function

        Private Shared Function move_next(ByVal container As hashmap(Of KEY_T, VALUE_T, _HASH_SIZE, _KEY_TO_INDEX),
                                          ByVal index As Int64,
                                          ByVal it As map(Of KEY_T, VALUE_T).iterator) As iterator
            assert(Not container Is Nothing)
            assert(Not it Is Nothing)
            it += 1
            If it = container.data(index, False).end() Then
                index = container.next_index(index)
                If valid_index(index) Then
                    it = container.data(index, False).begin()
                Else
                    Return container.end()
                End If
            End If
            Return New iterator(container, index, it)
        End Function

        Private Shared Function move_prev(ByVal container As hashmap(Of KEY_T, VALUE_T, _HASH_SIZE, _KEY_TO_INDEX),
                                          ByVal index As Int64,
                                          ByVal it As map(Of KEY_T, VALUE_T).iterator) As iterator
            assert(Not container Is Nothing)
            assert(Not it Is Nothing)
            it -= 1
            If it = container.data(index, False).end() Then
                index = container.prev_index(index)
                If valid_index(index) Then
                    it = container.data(index, False).begin()
                Else
                    Return container.end()
                End If
            End If
            Return New iterator(container, index, it)
        End Function

        Private Function move_next(ByVal that As UInt32) As iterator
            assert(that > 0)
            Dim r As iterator = Nothing
            r = Me
            For i As UInt32 = 0 To that - uint32_1
                r = move_next(r.container(), r.index(), r.iterator())
            Next
            Return r
        End Function

        Private Function move_prev(ByVal that As UInt32) As iterator
            assert(that > 0)
            Dim r As iterator = Nothing
            r = Me
            For i As UInt32 = 0 To that - uint32_1
                r = move_prev(r.container(), r.index(), r.iterator())
            Next
            Return r
        End Function

        Public Shared Operator +(ByVal this As iterator, ByVal that As Int32) As iterator
            If this Is Nothing OrElse this = this.container().end() OrElse that = 0 Then
                Return this
            ElseIf that > 0 Then
                Return this.move_next(that)
            Else
                assert(that < 0)
                Return this.move_prev(-that)
            End If
        End Operator

        Public Shared Operator -(ByVal this As iterator, ByVal that As Int32) As iterator
            If this Is Nothing OrElse this = this.container().end() OrElse that = 0 Then
                Return this
            ElseIf that > 0 Then
                Return this.move_prev(that)
            Else
                assert(that < 0)
                Return this.move_next(-that)
            End If
        End Operator

        Public Shared Operator +(ByVal this As iterator) As first_const_pair(Of KEY_T, VALUE_T)
            If this Is Nothing Then
                Return Nothing
            Else
                Return +(this.it)
            End If
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
            Else
                Return (c = 0) OrElse
                       (this Is Nothing AndAlso that.is_end()) OrElse
                       (that Is Nothing AndAlso this.is_end())
            End If
        End Operator

        Public Shared Operator <>(ByVal this As iterator, ByVal that As iterator) As Boolean
            Return Not (this = that)
        End Operator

        Public Function is_end() As Boolean
            Return invalid_index(index()) OrElse iterator().is_end()
        End Function

        Public Function is_not_end() As Boolean
            Return Not is_end()
        End Function

        Friend Sub New(ByVal container As hashmap(Of KEY_T, VALUE_T, _HASH_SIZE, _KEY_TO_INDEX),
                       ByVal index As Int64,
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

    Private Shared Function valid_index(ByVal index As UInt32) As Boolean
        Return index < hash_size
    End Function

    Private Shared Function invalid_index(ByVal index As UInt32) As Boolean
        Return Not valid_index(index)
    End Function

    Private Function data(ByVal index As Int64, Optional ByVal autoInsert As Boolean = True) As map(Of KEY_T, VALUE_T)
        If _data(index) Is Nothing AndAlso autoInsert Then
            _data(index) = New map(Of KEY_T, VALUE_T)()
        End If
        Return _data(index)
    End Function

    Private Function empty(ByVal i As Int64) As Boolean
        assert(valid_index(i))
        Return data(i, False) Is Nothing OrElse data(i).empty()
    End Function

    Private Function size(ByVal i As Int64) As UInt32
        assert(valid_index(i))
        Dim r As map(Of KEY_T, VALUE_T) = Nothing
        r = data(i, False)
        Return If(r Is Nothing, 0, r.size())
    End Function

    Private Function prev_index(ByVal this As UInt32) As UInt32
        Do
            If this = 0 Then
                this = max_uint32
            Else
                this -= 1
            End If
            If invalid_index(this) Then
                Exit Do
            End If
        Loop Until Not empty(this)

        Return this
    End Function

    Private Function next_index(ByVal this As Int32) As Int32
        Do
            this += 1
            If invalid_index(this) Then
                Exit Do
            End If
        Loop Until Not empty(this)

        Return this
    End Function

    Public Function rbegin() As iterator
        Dim rtn As iterator = Nothing
        Dim index As Int32 = prev_index(hash_size)
        If valid_index(index) Then
            Return New iterator(Me, index, data(index).rbegin())
        End If
        Return rend()
    End Function

    Public Function begin() As iterator
        Dim rtn As iterator = Nothing
        Dim index As Int32 = next_index(-1)
        If valid_index(index) Then
            Return New iterator(Me, index, data(index).begin())
        End If
        Return [end]()
    End Function

    Public Function [end]() As iterator
        Return _end
    End Function

    Public Function [rend]() As iterator
        Return _end
    End Function

    Public Sub clear()
        ReDim _data(hash_size - uint32_1)
    End Sub

    Shared Sub New()
        _end = New iterator(Nothing, max_uint32, map(Of KEY_T, VALUE_T).iterator.end)
        key_to_index = alloc(Of _KEY_TO_INDEX)()
        hash_size = (alloc(Of _HASH_SIZE)()).as_uint32()
        assert(hash_size > 0)
    End Sub

    Private Shared Function key_index(ByVal k As KEY_T) As UInt32
        Return key_to_index(k) Mod hash_size
    End Function

    Public Sub New()
        clear()
    End Sub

    Default Public Property _D(ByVal k As KEY_T) As VALUE_T
        Get
            Return data(key_index(k))(k)
        End Get
        Set(ByVal value As VALUE_T)
            data(key_index(k)).insert(k, value)
        End Set
    End Property

    Public Function insert(ByVal k As KEY_T, ByVal v As VALUE_T) As iterator
        Dim index As Int32 = npos
        index = key_index(k)
        Return New iterator(Me, index, data(index).insert(k, v))
    End Function

    Public Function insert(ByVal other As hashmap(Of KEY_T, VALUE_T, _HASH_SIZE, _KEY_TO_INDEX)) As Boolean
        If other Is Nothing Then
            Return False
        Else
            For i As UInt32 = 0 To hash_size - uint32_1
                If Not other.data(i, False) Is Nothing Then
                    Dim it As map(Of KEY_T, VALUE_T).iterator = Nothing
                    data(i).insert(other.data(i))
                End If
            Next
            Return True
        End If
    End Function

    Public Function find(ByVal k As KEY_T) As iterator
        Dim index As Int32 = npos
        index = key_index(k)
        If data(index, False) Is Nothing Then
            Return [end]()
        Else
            Return New iterator(Me, index, data(index).find(k))
        End If
    End Function

    Public Function empty() As Boolean
        For i As UInt32 = 0 To hash_size - uint32_1
            If Not empty(i) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Function size() As UInt32
        Dim r As UInt32 = 0
        For i As UInt32 = 0 To hash_size - uint32_1
            r += size(i)
        Next
        Return r
    End Function

    Public Function [erase](ByVal it As iterator) As Boolean
        If it Is Nothing OrElse it = [end]() Then
            Return False
        Else
            assert(object_compare(it.container(), Me) = 0)
            Return data(it.index()).erase(it.iterator())
        End If
    End Function

    Public Function [erase](ByVal k As KEY_T) As Boolean
        Return [erase](find(k))
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CloneT() As hashmap(Of KEY_T, VALUE_T, _HASH_SIZE, _KEY_TO_INDEX) _
                             Implements ICloneable(Of hashmap(Of KEY_T, VALUE_T, _HASH_SIZE, _KEY_TO_INDEX)).Clone
        Dim rtn As hashmap(Of KEY_T, VALUE_T, _HASH_SIZE, _KEY_TO_INDEX) = Nothing
        rtn = alloc(Me)
        For i As UInt32 = 0 To hash_size - uint32_1
            If Not _data(i) Is Nothing Then
                copy(rtn._data(i), _data(i))
            End If
        Next

        Return rtn
    End Function
End Class
