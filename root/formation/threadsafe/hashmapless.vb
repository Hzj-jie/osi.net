
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.template
Imports lock_t = osi.root.lock.slimlock.monitorlock

Public NotInheritable Class hashmapless(Of KEY_T As IComparable(Of KEY_T), VALUE_T)
    Inherits hashmapless(Of KEY_T, VALUE_T, default_to_uint32(Of KEY_T))
    Implements ICloneable(Of hashmapless(Of KEY_T, VALUE_T)), ICloneable

    Public Shared Function tiny() As hashmapless(Of KEY_T, VALUE_T)
        Return New hashmapless(Of KEY_T, VALUE_T)(15)
    End Function

    Public Sub New(ByVal hash_size As UInt32)
        MyBase.New(hash_size)
    End Sub

    <copy_constructor()>
    Protected Sub New(ByVal hash_size As UInt32, ByVal data() As unordered_map(Of KEY_T, VALUE_T))
        MyBase.New(hash_size, data)
    End Sub

    Public Shadows Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Shadows Function CloneT() As hashmapless(Of KEY_T, VALUE_T) _
                                     Implements ICloneable(Of hashmapless(Of KEY_T, VALUE_T)).Clone
        Return MyBase.clone(Of hashmapless(Of KEY_T, VALUE_T))()
    End Function
End Class

Public Class hashmapless(Of KEY_T As IComparable(Of KEY_T),
                            VALUE_T,
                            KEY_TO_INDEX As _to_uint32(Of KEY_T))
    Implements ICloneable, ICloneable(Of hashmapless(Of KEY_T, VALUE_T, KEY_TO_INDEX))

    Private Shared ReadOnly _KEY_TO_INDEX As KEY_TO_INDEX = alloc(Of KEY_TO_INDEX)()
    Private ReadOnly hash_size As UInt32 = 0
    Private ReadOnly data() As unordered_map(Of KEY_T, VALUE_T)
    Private ReadOnly _lock() As lock_t

    Private Function scoped_lock(ByVal index As UInt32) As IDisposable
        _lock(CInt(index)).wait()
        Return defer.to(Sub()
                                _lock(CInt(index)).release()
                            End Sub)
    End Function

    Private Function valid_index(ByVal index As UInt32) As Boolean
        Return index < hash_size
    End Function

    Private Function invalid_index(ByVal index As UInt32) As Boolean
        Return Not valid_index(index)
    End Function

    Private Function empty(ByVal i As UInt32) As Boolean
        assert(valid_index(i))
        Return data(CInt(i)).empty()
    End Function

    Public Sub clear()
        For i As Int32 = 0 To CInt(hash_size) - 1
            data(i).clear()
        Next
    End Sub

    Public Sub New(ByVal hash_size As UInt32)
        Me.New(hash_size, const_array.alloc_of(Function() As unordered_map(Of KEY_T, VALUE_T)
                                                   Return New unordered_map(Of KEY_T, VALUE_T)()
                                               End Function,
                                               hash_size).as_array())
    End Sub

    <copy_constructor()>
    Protected Sub New(ByVal hash_size As UInt32, ByVal data() As unordered_map(Of KEY_T, VALUE_T))
        assert(hash_size > 0)
        assert(hash_size <= max_int32)
        Me.hash_size = hash_size
        ReDim _lock(CInt(hash_size) - 1)
        assert(array_size(data) = hash_size)
        Me.data = data
        For i As Int32 = 0 To array_size_i(data) - 1
            assert(Not data(i) Is Nothing)
        Next
    End Sub

    Private Function index_of_key(ByVal k As KEY_T) As UInt32
        Return _KEY_TO_INDEX(k) Mod hash_size
    End Function

    Public Function [get](ByVal k As KEY_T, ByRef o As VALUE_T) As Boolean
        Dim index As UInt32 = 0
        index = index_of_key(k)
        Dim d As unordered_map(Of KEY_T, VALUE_T) = Nothing
        d = data(CInt(index))
        Using scoped_lock(index)
            Dim work As unordered_map(Of KEY_T, VALUE_T).iterator = Nothing
            work = d.find(k)
            If work <> d.end() Then
                o = (+work).second
                Return True
            End If
            Return False
        End Using
    End Function

    Default Public Property _D(ByVal k As KEY_T) As VALUE_T
        Get
            Return insert(k, AddressOf alloc(Of VALUE_T)).first
        End Get
        Set(ByVal value As VALUE_T)
            Dim index As UInt32 = 0
            index = index_of_key(k)
            Using scoped_lock(index)
                data(CInt(index))(k) = value
            End Using
        End Set
    End Property

    ' Returning iterator is not reasonable, as it may be invalidated by operations in other threads.
    Public Function insert(ByVal k As KEY_T, ByVal f As Func(Of VALUE_T)) As pair(Of VALUE_T, Boolean)
        assert(Not f Is Nothing)
        Dim index As UInt32 = 0
        index = index_of_key(k)
        Dim d As unordered_map(Of KEY_T, VALUE_T) = Nothing
        d = data(CInt(index))
        While False
            Using scoped_lock(index)
                Dim work As unordered_map(Of KEY_T, VALUE_T).iterator = Nothing
                work = d.find(k)
                If work = d.end() Then
                    Exit While
                End If
                Dim r As pair(Of VALUE_T, Boolean) = Nothing
                r = pair.emplace_of((+work).second, False)
                Return r
            End Using
        End While
        Return emplace(k, f())
    End Function

    ' Insert() with function always uses emplace to avoid unnecessary copy.
    Public Function emplace(ByVal k As KEY_T, ByVal f As Func(Of VALUE_T)) As pair(Of VALUE_T, Boolean)
        Return insert(k, f)
    End Function

    Public Function emplace(ByVal k As KEY_T, ByVal v As VALUE_T) As pair(Of VALUE_T, Boolean)
        Dim index As UInt32 = 0
        index = index_of_key(k)
        Using scoped_lock(index)
            Dim w As tuple(Of unordered_map(Of KEY_T, VALUE_T).iterator, Boolean) = Nothing
            w = data(CInt(index)).emplace(k, v)
            Return pair.emplace_of((+w.first).second, w.second)
        End Using
    End Function

    Public Function insert(ByVal k As KEY_T, ByVal v As VALUE_T) As pair(Of VALUE_T, Boolean)
        Return emplace(copy_no_error(k), copy_no_error(v))
    End Function

    Public Function insert(ByVal other As hashmapless(Of KEY_T, VALUE_T, KEY_TO_INDEX)) As Boolean
        If other Is Nothing Then
            Return False
        End If
        If hash_size = other.hash_size Then
            For i As UInt32 = 0 To hash_size - uint32_1
                Using scoped_lock(i)
                    Using other.scoped_lock(i)
                        assert(data(CInt(i)).insert(other.data(CInt(i))))
                    End Using
                End Using
            Next
        Else
            For i As UInt32 = 0 To other.hash_size - uint32_1
                Using other.scoped_lock(i)
                    Dim it As unordered_map(Of KEY_T, VALUE_T).iterator = Nothing
                    it = other.data(CInt(i)).begin()
                    While it <> other.data(CInt(i)).end()
                        insert((+it).first, (+it).second)
                        it += 1
                    End While
                End Using
            Next
        End If
        Return True
    End Function

    Public Function empty() As Boolean
        For i As UInt32 = 0 To hash_size - uint32_1
            If Not empty(i) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Function [erase](ByVal k As KEY_T) As Boolean
        Dim index As UInt32 = 0
        index = index_of_key(k)
        Using scoped_lock(index)
            Return data(CInt(index)).erase(k)
        End Using
    End Function

    Public Function foreach(ByVal f As Func(Of KEY_T, VALUE_T, Boolean)) As Int32
        If f Is Nothing Then
            Return npos
        End If
        Dim r As Int32 = 0
        For i As UInt32 = 0 To hash_size - uint32_1
            Dim d As unordered_map(Of KEY_T, VALUE_T) = Nothing
            d = data(CInt(i))
            Using scoped_lock(i)
                Dim it As unordered_map(Of KEY_T, VALUE_T).iterator = Nothing
                it = d.begin()
                While it <> d.end()
                    If Not f((+it).first, (+it).second) Then
                        Return r
                    End If
                    it += 1
                    r += 1
                End While
            End Using
        Next
        Return r
    End Function

    Public Function foreach(ByVal f As Action(Of KEY_T, VALUE_T)) As Int32
        If f Is Nothing Then
            Return npos
        End If
        Return foreach(f.true_())
    End Function

    Public Function CloneT() As hashmapless(Of KEY_T, VALUE_T, KEY_TO_INDEX) _
                              Implements ICloneable(Of hashmapless(Of KEY_T, VALUE_T, KEY_TO_INDEX)).Clone
        Return clone(Of hashmapless(Of KEY_T, VALUE_T, KEY_TO_INDEX))()
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Protected Function clone(Of R As hashmapless(Of KEY_T, VALUE_T, KEY_TO_INDEX))() As R
        Dim data() As unordered_map(Of KEY_T, VALUE_T) = Nothing
        ReDim data(CInt(hash_size) - 1)
        For i As UInt32 = 0 To hash_size - uint32_1
            Using scoped_lock(i)
                copy(data(CInt(i)), Me.data(CInt(i)))
            End Using
        Next

        Return copy_constructor(Of R).invoke(hash_size, data)
    End Function
End Class
