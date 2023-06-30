
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.cache.constants.mapheap_cache

'for test only
Friend NotInheritable Class map_map_cache(Of KEY_T As IComparable(Of KEY_T), VALUE_T)
    Implements islimcache2(Of KEY_T, VALUE_T)

    Private ReadOnly kv As map(Of KEY_T, VALUE_T)
    Private ReadOnly kt As map(Of KEY_T, Int64)
    Private ReadOnly max_size As UInt64
    Private ReadOnly retire_ticks As UInt64
    Private ReadOnly update_ticks_when_refer As Boolean

    Public Sub New(Optional ByVal max_size As UInt64 = default_max_size,
                   Optional ByVal retire_ticks As UInt64 = default_retire_ticks,
                   Optional ByVal update_ticks_when_refer As Boolean = default_update_ticks_when_refer)
        assert(max_size > 0)
        Me.max_size = max_size
        Me.retire_ticks = retire_ticks
        Me.update_ticks_when_refer = update_ticks_when_refer
        kv = New map(Of KEY_T, VALUE_T)()
        kt = New map(Of KEY_T, Int64)()
    End Sub

    Private Function no_retire() As Boolean
        Return retire_ticks = no_retire_ticks
    End Function

    Private Function retired(ByVal d As Int64) As Boolean
        Return Not no_retire() AndAlso nowadays.ticks() - d >= retire_ticks
    End Function

    Private Function retired(ByVal i As map(Of KEY_T, Int64).iterator) As Boolean
        assert(Not i.is_null())
        assert(i <> kt.end())
        Return retired((+i).second)
    End Function

    Private Sub update_refer_ticks(ByVal k As KEY_T)
        assert(kt.insert(k, nowadays.ticks()) <> kt.end())
    End Sub

    Public Sub clear() Implements islimcache(Of KEY_T, VALUE_T).clear
        kv.clear()
        kt.clear()
    End Sub

    Public Function [erase](ByVal key As KEY_T) As Boolean Implements islimcache(Of KEY_T, VALUE_T).erase
        If kv.erase(key) Then
            assert(kt.erase(key))
            Return True
        End If
        Return False
    End Function

    Public Function [get](ByVal key As KEY_T, ByRef value As VALUE_T) As Boolean _
                         Implements islimcache(Of KEY_T, VALUE_T).get
        Dim i As map(Of KEY_T, VALUE_T).iterator = Nothing
        i = kv.find(key)
        If i = kv.end() Then
            If isdebugmode() Then
                assert(kt.find(key) = kt.end())
            End If
            Return False
        End If
        Dim j As map(Of KEY_T, Int64).iterator = Nothing
        j = kt.find(key)
        assert(j <> kt.end())
        If retired(j) Then
            assert([erase](key))
            Return False
        End If
        copy(value, (+i).second)
        If update_ticks_when_refer Then
            update_refer_ticks(key)
        End If
        Return True
    End Function

    Public Sub [set](ByVal key As KEY_T, ByVal value As VALUE_T) Implements islimcache(Of KEY_T, VALUE_T).set
        kv.insert(key, value)
        update_refer_ticks(key)
        If size() > max_size Then
            Dim old_key As KEY_T = Nothing
            Dim old As Int64 = max_int64
            kt.stream().foreach(kt.on_pair(Sub(ByVal k As KEY_T, ByVal v As Int64)
                                               If v < old Then
                                                   old = v
                                                   old_key = k
                                               End If
                                           End Sub))
            assert(Not old_key Is Nothing)
            assert([erase](old_key))
            assert(size() = max_size)
        End If
    End Sub

    Public Function size() As Int64 Implements islimcache(Of KEY_T, VALUE_T).size
        If isdebugmode() Then
            assert(kt.size() = kv.size())
        End If
        Return kv.size()
    End Function

    Public Function empty() As Boolean Implements islimcache2(Of KEY_T, VALUE_T).empty
        If isdebugmode() Then
            assert(kt.empty() = kv.empty())
        End If
        Return kv.empty()
    End Function

    Public Function have(ByVal key As KEY_T) As Boolean Implements islimcache2(Of KEY_T, VALUE_T).have
        If isdebugmode() Then
            assert((kt.find(key) = kt.end()) = (kv.find(key) = kv.end()))
        End If
        Return kv.find(key) <> kv.end()
    End Function
End Class
