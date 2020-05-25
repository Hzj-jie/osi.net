
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.utils
Imports mhc = osi.service.cache.constants.mapheap_cache

Public Module _cache2
    Private Function wrapper(Of KEY_T As IComparable(Of KEY_T), VALUE_T) _
                            (ByVal i As islimcache2(Of KEY_T, VALUE_T)) As icache2(Of KEY_T, VALUE_T)
        assert(Not i Is Nothing)
        Return New cache2(Of KEY_T, VALUE_T)(i)
    End Function

    Public Function unordered_map_cache2(Of KEY_T As IComparable(Of KEY_T), VALUE_T)() As icache2(Of KEY_T, VALUE_T)
        Return wrapper(New unordered_map_cache(Of KEY_T, VALUE_T)())
    End Function

    Public Function map_cache2(Of KEY_T As IComparable(Of KEY_T), VALUE_T)() As icache2(Of KEY_T, VALUE_T)
        Return wrapper(New map_cache(Of KEY_T, VALUE_T)())
    End Function

    Public Function mapheap_cache2(Of KEY_T As IComparable(Of KEY_T), VALUE_T) _
                                  (Optional ByVal max_size As UInt64 = mhc.default_max_size,
                                   Optional ByVal retire_ticks As UInt64 = mhc.default_retire_ticks,
                                   Optional ByVal update_ticks_when_refer As Boolean = _
                                                        mhc.default_update_ticks_when_refer) _
                                  As icache2(Of KEY_T, VALUE_T)
        Return wrapper(New mapheap_cache(Of KEY_T, VALUE_T)(max_size, retire_ticks, update_ticks_when_refer))
    End Function

    Public Sub mapheap_cache2(Of KEY_T As IComparable(Of KEY_T), VALUE_T) _
                             (ByRef i As icache2(Of KEY_T, VALUE_T),
                              Optional ByVal max_size As UInt64 = mhc.default_max_size,
                              Optional ByVal retire_ticks As UInt64 = mhc.default_retire_ticks,
                              Optional ByVal update_ticks_when_refer As Boolean = _
                                                    mhc.default_update_ticks_when_refer)
        i = mapheap_cache2(Of KEY_T, VALUE_T)(max_size, retire_ticks, update_ticks_when_refer)
    End Sub

    Public Function map_map_cache2(Of KEY_T As IComparable(Of KEY_T), VALUE_T) _
                                  (Optional ByVal max_size As UInt64 = mhc.default_max_size,
                                   Optional ByVal retire_ticks As UInt64 = mhc.default_retire_ticks,
                                   Optional ByVal update_ticks_when_refer As Boolean = _
                                                        mhc.default_update_ticks_when_refer) _
                                  As icache2(Of KEY_T, VALUE_T)
        Return wrapper(New map_map_cache(Of KEY_T, VALUE_T)(max_size, retire_ticks, update_ticks_when_refer))
    End Function
End Module

Public Class cache2(Of KEY_T As IComparable(Of KEY_T), VALUE_T)
    Implements icache2(Of KEY_T, VALUE_T)

    Private ReadOnly c As islimcache2(Of KEY_T, VALUE_T)
    Private ReadOnly l As ref(Of event_comb_lock)

    Public Sub New(ByVal c As islimcache2(Of KEY_T, VALUE_T))
        assert(Not c Is Nothing)
        assert(Not TypeOf c Is icache(Of KEY_T, VALUE_T))
        Me.c = c
        l = New ref(Of event_comb_lock)()
    End Sub

    Public Sub New(ByVal c As islimcache(Of KEY_T, VALUE_T))
        Me.New(New slimcache2(Of KEY_T, VALUE_T)(c))
    End Sub

    Public Function clear() As event_comb Implements icache2(Of KEY_T, VALUE_T).clear
        Return l.locked(Sub() c.clear())
    End Function

    Public Function empty() As event_comb Implements icache2(Of KEY_T, VALUE_T).empty
        Return l.locked(Function() c.empty())
    End Function

    Public Function [erase](ByVal key As KEY_T) As event_comb Implements icache2(Of KEY_T, VALUE_T).erase
        Return l.locked(Function() c.erase(key))
    End Function

    Public Function [get](ByVal key As KEY_T, ByVal value As pointer(Of VALUE_T)) As event_comb _
                         Implements icache2(Of KEY_T, VALUE_T).get
        Return l.locked(Function() As Boolean
                            Dim v As VALUE_T = Nothing
                            Return Not value Is Nothing AndAlso
                                   eva(value, DirectCast(Nothing, VALUE_T)) AndAlso
                                   c.get(key, v) AndAlso
                                   eva(value, v)
                        End Function)
    End Function

    Public Function have(ByVal key As KEY_T) As event_comb Implements icache2(Of KEY_T, VALUE_T).have
        Return l.locked(Function() c.have(key))
    End Function

    Public Function [set](ByVal key As KEY_T, ByVal value As VALUE_T) As event_comb _
                         Implements icache2(Of KEY_T, VALUE_T).set
        Return l.locked(Sub() c.set(key, value))
    End Function

    Public Function size(ByVal value As pointer(Of Int64)) As event_comb _
                        Implements icache2(Of KEY_T, VALUE_T).size
        Return l.locked(Function() As Boolean
                            Return Not value Is Nothing AndAlso
                                   eva(value, c.size())
                        End Function)
    End Function
End Class
