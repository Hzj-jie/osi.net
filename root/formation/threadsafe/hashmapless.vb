
Imports osi.root.template
Imports osi.root.connector
Imports lock_t = osi.root.lock.slimlock.monitorlock

Public Class hashmapless(Of keyT As IComparable(Of keyT), valueT, HASH_SIZE As _int64)
    Inherits hashmapless(Of keyT, valueT, HASH_SIZE, default_to_uint32(Of keyT))
End Class

Public Class hashmapless(Of keyT As IComparable(Of keyT), valueT,
                            HASH_SIZE As _int64, KEY_TO_INDEX As _to_uint32(Of keyT))
    Implements ICloneable

    Private Shared ReadOnly hashSize As UInt32 = 0
    Private Shared ReadOnly _keyToIndex As KEY_TO_INDEX = Nothing
    Private data() As map(Of keyT, valueT)
    Private _lock() As lock_t

    Private Sub lock(ByVal index As UInt32)
        _lock(index).wait()
    End Sub

    Private Sub unlock(ByVal index As UInt32)
        _lock(index).release()
    End Sub

    Private Shared Function valid_index(ByVal index As UInt32) As Boolean
        Return index < hashSize
    End Function

    Private Shared Function invalid_index(ByVal index As UInt32) As Boolean
        Return Not valid_index(index)
    End Function

    Private Function empty(ByVal i As Int64) As Boolean
        assert(valid_index(i))
        Return data(i).empty()
    End Function

    Public Sub clear()
        ReDim data(hashSize - 1)
        For i As Int64 = 0 To hashSize - 1
            data(i) = New map(Of keyT, valueT)()
        Next
    End Sub

    Shared Sub New()
        _keyToIndex = alloc(Of KEY_TO_INDEX)()
        hashSize = +(alloc(Of HASH_SIZE)())
    End Sub

    Private Shared Function keyToIndex(ByVal k As keyT) As UInt32
        Return _keyToIndex(k) Mod hashSize
    End Function

    Public Sub New()
        ReDim _lock(hashSize - 1)
        clear()
    End Sub

    Public Function [get](ByVal k As keyT, ByRef o As valueT) As Boolean
        Dim work As map(Of keyT, valueT).iterator = Nothing
        Dim rtn As Boolean = False
        Dim index As UInt32 = keyToIndex(k)
        lock(index)
        work = data(index).find(k)
        If work <> data(index).end() Then
            o = (+work).second
            rtn = True
        Else
            rtn = False
        End If
        unlock(index)
        Return rtn
    End Function

    Default Public Property _D(ByVal k As keyT) As valueT
        Get
            Dim o As valueT = Nothing
            If [get](k, o) Then
                Return o
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As valueT)
            Dim index As UInt32 = keyToIndex(k)
            lock(index)
            data(index).insert(k, value)
            unlock(index)
        End Set
    End Property

    Public Sub insert(ByVal k As keyT, ByVal v As valueT)
        Dim index As UInt32 = 0
        index = keyToIndex(k)
        lock(index)
        data(index).insert(k, v)
        unlock(index)
    End Sub

    Public Function unique_insert(ByVal k As keyT, ByVal v As valueT) As Boolean
        Dim index As UInt32 = 0
        index = keyToIndex(k)
        Dim rtn As Boolean = False
        lock(index)
        If data(index).find(k) = data(index).end() Then
            data(index).insert(k, v)
            rtn = True
        Else
            rtn = False
        End If
        unlock(index)
        Return rtn
    End Function

    Public Function insert(ByVal other As hashmapless(Of keyT, valueT, HASH_SIZE, KEY_TO_INDEX)) As Boolean
        If other Is Nothing Then
            Return False
        Else
            For i As Int64 = 0 To hashSize - 1
                lock(i)
                other.lock(i)
                data(i).insert(other.data(i))
                other.unlock(i)
                unlock(i)
            Next
            Return True
        End If
    End Function

    Public Function empty() As Boolean
        For i As Int64 = 0 To hashSize - 1
            If Not empty(i) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Function [erase](ByVal k As keyT) As Boolean
        Dim index As UInt32 = 0
        Dim rtn As Boolean = False
        index = keyToIndex(k)
        lock(index)
        rtn = data(index).erase(k)
        unlock(index)
        Return rtn
    End Function

    Public Function Clone() As Object Implements System.ICloneable.Clone
        Dim rtn As hashmapless(Of keyT, valueT, HASH_SIZE, KEY_TO_INDEX) = Nothing
        rtn = alloc(Me)
        Dim i As Int32
        For i = 0 To hashSize - 1
            lock(i)
            copy(rtn.data(i), data(i))
            unlock(i)
        Next

        Return rtn
    End Function
End Class
