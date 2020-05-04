
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.template

Partial Public Class hashmap(Of KEY_T As IComparable(Of KEY_T),
                                VALUE_T,
                                _KEY_TO_INDEX As _to_uint32(Of KEY_T))
    Implements ICloneable, ICloneable(Of hashmap(Of KEY_T, VALUE_T, _KEY_TO_INDEX))

    Private Shared ReadOnly _end As iterator
    Private Shared ReadOnly key_to_index As _KEY_TO_INDEX
    Private ReadOnly hash_size As UInt32
    Private ReadOnly _data() As map(Of KEY_T, VALUE_T)

    Shared Sub New()
        _end = New iterator(Nothing, max_uint32, map(Of KEY_T, VALUE_T).iterator.end)
        key_to_index = alloc(Of _KEY_TO_INDEX)()
    End Sub

    Public Sub New(ByVal hash_size As UInt32)
        Me.New(hash_size, New map(Of KEY_T, VALUE_T)(CInt(hash_size - uint32_1)) {})
    End Sub

    Public Sub New()
        Me.New(3371)
    End Sub

    <copy_constructor>
    Protected Sub New(ByVal hash_size As UInt32, ByVal data() As map(Of KEY_T, VALUE_T))
        assert(hash_size > 0)
        assert(hash_size <= max_int32)
        Me.hash_size = hash_size
        assert(array_size(data) = hash_size)
        Me._data = data
    End Sub

    Public Function rbegin() As iterator
        Dim rtn As iterator = Nothing
        Dim index As UInt32 = prev_index(hash_size)
        If valid_index(index) Then
            Return New iterator(Me, index, data(index).rbegin())
        End If
        Return rend()
    End Function

    Public Function begin() As iterator
        Dim rtn As iterator = Nothing
        Dim index As UInt32 = 0
        index = first_index()
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
        arrays.clear(_data)
    End Sub

    Default Public Property _D(ByVal k As KEY_T) As VALUE_T
        Get
            Return data(key_index(k))(k)
        End Get
        Set(ByVal value As VALUE_T)
            Dim r As pair(Of iterator, Boolean) = Nothing
            r = insert(k, value)
            If Not r.second Then
                copy(r.first.iterator().value().second, value)
            End If
        End Set
    End Property

    Public Function insert(ByVal k As KEY_T, ByVal v As VALUE_T) As pair(Of iterator, Boolean)
        Return emplace(k, copy_no_error(v))
    End Function

    Public Function emplace(ByVal k As KEY_T, ByVal v As VALUE_T) As pair(Of iterator, Boolean)
        Dim index As UInt32 = 0
        index = key_index(k)
        Dim p As pair(Of map(Of KEY_T, VALUE_T).iterator, Boolean) = Nothing
        p = data(index).emplace(k, v)
        Return pair.emplace_of(New iterator(Me, index, p.first), p.second)
    End Function

    Public Function insert(ByVal other As hashmap(Of KEY_T, VALUE_T, _KEY_TO_INDEX)) As Boolean
        If other Is Nothing Then
            Return False
        End If
        For i As UInt32 = 0 To hash_size - uint32_1
            If Not other.data(i, False) Is Nothing Then
                Dim it As map(Of KEY_T, VALUE_T).iterator = Nothing
                data(i).insert(other.data(i))
            End If
        Next
        Return True
    End Function

    Public Function find(ByVal k As KEY_T) As iterator
        Dim index As UInt32 = 0
        index = key_index(k)
        If data(index, False) Is Nothing Then
            Return [end]()
        End If
        Return New iterator(Me, index, data(index).find(k))
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
        If it Is Nothing OrElse it = [end]() OrElse object_compare(it.container(), Me) <> 0 Then
            Return False
        End If
        Return data(it.index()).erase(it.iterator())
    End Function

    Public Function [erase](ByVal k As KEY_T) As Boolean
        Return [erase](find(k))
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CloneT() As hashmap(Of KEY_T, VALUE_T, _KEY_TO_INDEX) _
                             Implements ICloneable(Of hashmap(Of KEY_T, VALUE_T, _KEY_TO_INDEX)).Clone
        Return clone(Of hashmap(Of KEY_T, VALUE_T, _KEY_TO_INDEX))()
    End Function

    Protected Function clone(Of R As hashmap(Of KEY_T, VALUE_T, _KEY_TO_INDEX))() As R
        Return copy_constructor(Of R).invoke(hash_size, _data.deep_clone())
    End Function
End Class
