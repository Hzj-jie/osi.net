
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.template
Imports lock_t = osi.root.lock.slimlock.monitorlock

Public Class hashmapless(Of KEY_T As IComparable(Of KEY_T), VALUE_T)
    Inherits hashmapless(Of KEY_T, VALUE_T, default_to_uint32(Of KEY_T))
    Implements ICloneable(Of hashmapless(Of KEY_T, VALUE_T)), ICloneable

    Public Sub New(ByVal hash_size As UInt32)
        MyBase.New(hash_size)
    End Sub

    <copy_constructor()>
    Protected Sub New(ByVal hash_size As UInt32, ByVal data() As map(Of KEY_T, VALUE_T))
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

    Private Shared ReadOnly _KEY_TO_INDEX As KEY_TO_INDEX = Nothing
    Private ReadOnly hash_size As UInt32 = 0
    Private ReadOnly data() As map(Of KEY_T, VALUE_T)
    Private ReadOnly _lock() As lock_t

    Private Sub lock(ByVal index As UInt32)
        _lock(CInt(index)).wait()
    End Sub

    Private Sub unlock(ByVal index As UInt32)
        _lock(CInt(index)).release()
    End Sub

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
            data(i) = New map(Of KEY_T, VALUE_T)()
        Next
    End Sub

    Shared Sub New()
        _KEY_TO_INDEX = alloc(Of KEY_TO_INDEX)()
    End Sub

    Public Sub New(ByVal hash_size As UInt32)
        Me.New(hash_size, New map(Of KEY_T, VALUE_T)(CInt(hash_size) - 1) {})
    End Sub

    <copy_constructor()>
    Protected Sub New(ByVal hash_size As UInt32, ByVal data() As map(Of KEY_T, VALUE_T))
        assert(hash_size > 0)
        assert(hash_size <= max_int32)
        Me.hash_size = hash_size
        ReDim _lock(CInt(hash_size) - 1)
        assert(array_size(data) = hash_size)
        Me.data = data
        clear()
    End Sub

    Private Function index_of_key(ByVal k As KEY_T) As UInt32
        Return _KEY_TO_INDEX(k) Mod hash_size
    End Function

    Public Function [get](ByVal k As KEY_T, ByRef o As VALUE_T) As Boolean
        Dim work As map(Of KEY_T, VALUE_T).iterator = Nothing
        Dim rtn As Boolean = False
        Dim index As UInt32 = 0
        index = index_of_key(k)
        lock(index)
        work = data(CInt(index)).find(k)
        If work <> data(CInt(index)).end() Then
            o = (+work).second
            rtn = True
        Else
            rtn = False
        End If
        unlock(index)
        Return rtn
    End Function

    Default Public Property _D(ByVal k As KEY_T) As VALUE_T
        Get
            Dim o As VALUE_T = Nothing
            If [get](k, o) Then
                Return o
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As VALUE_T)
            Dim index As UInt32 = 0
            index = index_of_key(k)
            lock(index)
            data(CInt(index)).insert(k, value)
            unlock(index)
        End Set
    End Property

    Public Sub insert(ByVal k As KEY_T, ByVal v As VALUE_T)
        Dim index As UInt32 = 0
        index = index_of_key(k)
        lock(index)
        data(CInt(index)).insert(k, v)
        unlock(index)
    End Sub

    Public Function unique_insert(ByVal k As KEY_T, ByVal v As VALUE_T) As Boolean
        Dim index As UInt32 = 0
        index = index_of_key(k)
        Dim rtn As Boolean = False
        lock(index)
        If data(CInt(index)).find(k) = data(CInt(index)).end() Then
            data(CInt(index)).insert(k, v)
            rtn = True
        Else
            rtn = False
        End If
        unlock(index)
        Return rtn
    End Function

    Public Function insert(ByVal other As hashmapless(Of KEY_T, VALUE_T, KEY_TO_INDEX)) As Boolean
        If other Is Nothing Then
            Return False
        End If
        If hash_size = other.hash_size Then
            For i As UInt32 = 0 To hash_size - uint32_1
                lock(i)
                other.lock(i)
                data(CInt(i)).insert(other.data(CInt(i)))
                other.unlock(i)
                unlock(i)
            Next
        Else
            For i As UInt32 = 0 To other.hash_size - uint32_1
                other.lock(i)
                Dim it As map(Of KEY_T, VALUE_T).iterator = Nothing
                it = other.data(CInt(i)).begin()
                While it <> other.data(CInt(i)).end()
                    insert((+it).first, (+it).second)
                    it += 1
                End While
                other.unlock(i)
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
        Dim rtn As Boolean = False
        index = index_of_key(k)
        lock(index)
        rtn = data(CInt(index)).erase(k)
        unlock(index)
        Return rtn
    End Function

    Public Function CloneT() As hashmapless(Of KEY_T, VALUE_T, KEY_TO_INDEX) _
                              Implements ICloneable(Of hashmapless(Of KEY_T, VALUE_T, KEY_TO_INDEX)).Clone
        Return clone(Of hashmapless(Of KEY_T, VALUE_T, KEY_TO_INDEX))()
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Protected Function clone(Of R As hashmapless(Of KEY_T, VALUE_T, KEY_TO_INDEX))() As R
        Dim data() As map(Of KEY_T, VALUE_T) = Nothing
        ReDim data(CInt(hash_size) - 1)
        For i As UInt32 = 0 To hash_size - uint32_1
            lock(i)
            copy(data(CInt(i)), Me.data(CInt(i)))
            unlock(i)
        Next

        Return copy_constructor(Of R).invoke(hash_size, data)
    End Function
End Class
