
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.lock
Imports osi.root.utils
Imports mhc = osi.service.cache.constants.mapheap_cache
Imports monitorlock = osi.root.lock.slimlock.monitorlock

Public Module _cache
    Private Function wrapper(Of KEY_T As IComparable(Of KEY_T), VALUE_T) _
                            (ByVal i As islimcache2(Of KEY_T, VALUE_T)) As icache(Of KEY_T, VALUE_T)
        assert(Not i Is Nothing)
        Return New cache(Of KEY_T, VALUE_T)(i)
    End Function

    Public Function hashmap_cache(Of KEY_T As IComparable(Of KEY_T), VALUE_T)(ByVal hash_size As UInt32) _
                                 As icache(Of KEY_T, VALUE_T)
        Return wrapper(New hashmap_cache(Of KEY_T, VALUE_T)(hash_size))
    End Function

    Public Function hashmap_cache(Of KEY_T As IComparable(Of KEY_T), VALUE_T)() As icache(Of KEY_T, VALUE_T)
        Return wrapper(New hashmap_cache(Of KEY_T, VALUE_T)())
    End Function

    Public Function map_cache(Of KEY_T As IComparable(Of KEY_T), VALUE_T)() As icache(Of KEY_T, VALUE_T)
        Return wrapper(New map_cache(Of KEY_T, VALUE_T)())
    End Function

    Public Function mapheap_slimecache2(Of KEY_T As IComparable(Of KEY_T), VALUE_T) _
                                       (Optional ByVal max_size As UInt64 = mhc.default_max_size,
                                        Optional ByVal retire_ticks As UInt64 = mhc.default_retire_ticks,
                                        Optional ByVal update_ticks_when_refer As Boolean = _
                                                            mhc.default_update_ticks_when_refer) _
                                       As islimcache2(Of KEY_T, VALUE_T)
        Return New mapheap_cache(Of KEY_T, VALUE_T)(max_size, retire_ticks, update_ticks_when_refer)
    End Function

    Public Function mapheap_cache(Of KEY_T As IComparable(Of KEY_T), VALUE_T) _
                                 (Optional ByVal max_size As UInt64 = mhc.default_max_size,
                                  Optional ByVal retire_ticks As UInt64 = mhc.default_retire_ticks,
                                  Optional ByVal update_ticks_when_refer As Boolean = _
                                                    mhc.default_update_ticks_when_refer) _
                                 As icache(Of KEY_T, VALUE_T)
        Return wrapper(mapheap_slimecache2(Of KEY_T, VALUE_T)(max_size, retire_ticks, update_ticks_when_refer))
    End Function

    Public Sub mapheap_slimecache2(Of KEY_T As IComparable(Of KEY_T), VALUE_T) _
                                  (ByRef i As islimcache2(Of KEY_T, VALUE_T),
                                   Optional ByVal max_size As UInt64 = mhc.default_max_size,
                                   Optional ByVal retire_ticks As UInt64 = mhc.default_retire_ticks,
                                   Optional ByVal update_ticks_when_refer As Boolean = _
                                                        mhc.default_update_ticks_when_refer)
        i = mapheap_slimecache2(Of KEY_T, VALUE_T)(max_size, retire_ticks, update_ticks_when_refer)
    End Sub

    Public Sub mapheap_cache(Of KEY_T As IComparable(Of KEY_T), VALUE_T) _
                            (ByRef i As icache(Of KEY_T, VALUE_T),
                             Optional ByVal max_size As UInt64 = mhc.default_max_size,
                             Optional ByVal retire_ticks As UInt64 = mhc.default_retire_ticks,
                             Optional ByVal update_ticks_when_refer As Boolean = _
                                                mhc.default_update_ticks_when_refer)
        i = mapheap_cache(Of KEY_T, VALUE_T)(max_size, retire_ticks, update_ticks_when_refer)
    End Sub

    Public Function map_map_cache(Of KEY_T As IComparable(Of KEY_T), VALUE_T) _
                                 (Optional ByVal max_size As UInt64 = mhc.default_max_size,
                                  Optional ByVal retire_ticks As UInt64 = mhc.default_retire_ticks,
                                  Optional ByVal update_ticks_when_refer As Boolean = _
                                                    mhc.default_update_ticks_when_refer) _
                                 As icache(Of KEY_T, VALUE_T)
        Return wrapper(New map_map_cache(Of KEY_T, VALUE_T)(max_size, retire_ticks, update_ticks_when_refer))
    End Function
End Module

Public Class cache(Of KEY_T As IComparable(Of KEY_T), VALUE_T)
    Implements icache(Of KEY_T, VALUE_T)

    Private ReadOnly c As islimcache2(Of KEY_T, VALUE_T)
    Private l As monitorlock

    Public Sub New(ByVal c As islimcache(Of KEY_T, VALUE_T))
        Me.New(New slimcache2(Of KEY_T, VALUE_T)(c))
    End Sub

    Public Sub New(ByVal c As islimcache2(Of KEY_T, VALUE_T))
        assert(Not c Is Nothing)
        assert(Not TypeOf c Is icache(Of KEY_T, VALUE_T))
        Me.c = c
    End Sub

    Public Function empty() As Boolean Implements icache(Of KEY_T, VALUE_T).empty
        Return l.locked(Function() c.empty())
    End Function

    Public Function foreach(ByVal d As _do(Of KEY_T, VALUE_T, Boolean)) As Boolean _
                           Implements icache(Of KEY_T, VALUE_T).foreach
        Return utils.foreach(AddressOf foreach, d)
    End Function

    Public Function foreach(ByVal d As void(Of KEY_T, VALUE_T)) As Boolean _
                           Implements icache(Of KEY_T, VALUE_T).foreach
        Return utils.foreach(AddressOf foreach, d)
    End Function

    Public Function [get](ByVal key As KEY_T) As VALUE_T Implements icache(Of KEY_T, VALUE_T).get
        Dim r As VALUE_T = Nothing
        If [get](key, r) Then
            Return r
        Else
            Return Nothing
        End If
    End Function

    Public Function have(ByVal key As KEY_T) As Boolean Implements icache(Of KEY_T, VALUE_T).have
        Return l.locked(Function() c.have(key))
    End Function

    Public Sub clear() Implements icache(Of KEY_T, VALUE_T).clear
        l.locked(Sub() c.clear())
    End Sub

    Public Function [erase](ByVal key As KEY_T) As Boolean Implements icache(Of KEY_T, VALUE_T).erase
        Return l.locked(Function() c.erase(key))
    End Function

    Public Function foreach(ByVal d As _do(Of KEY_T, VALUE_T, Boolean, Boolean)) As Boolean _
                           Implements icache(Of KEY_T, VALUE_T).foreach
        Return l.locked(Function() c.foreach(d))
    End Function

    Public Function [get](ByVal key As KEY_T, ByRef value As VALUE_T) As Boolean _
                         Implements icache(Of KEY_T, VALUE_T).get
        Dim r As VALUE_T = Nothing
        Return l.locked(Function() c.get(key, r)) AndAlso eva(value, r)
    End Function

    Public Sub [set](ByVal key As KEY_T, ByVal value As VALUE_T) Implements icache(Of KEY_T, VALUE_T).set
        l.locked(Sub() c.set(key, value))
    End Sub

    Public Function size() As Int64 Implements icache(Of KEY_T, VALUE_T).size
        Return l.locked(Function() c.size())
    End Function
End Class
