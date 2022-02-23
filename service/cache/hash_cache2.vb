
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.template
Imports osi.root.utils
Imports mhc = osi.service.cache.constants.mapheap_cache

Public Class hash_cache2(Of KEY_T As IComparable(Of KEY_T), VALUE_T)
    Inherits hash_cache2(Of KEY_T, VALUE_T, _63)

    Public Sub New(Optional ByVal max_size As UInt64 = mhc.default_max_size,
                   Optional ByVal retire_ticks As UInt64 = mhc.default_retire_ticks)
        MyBase.New(max_size, retire_ticks)
    End Sub

    Public Sub New(ByVal d As Func(Of icache2(Of KEY_T, VALUE_T)))
        MyBase.New(d)
    End Sub
End Class

Public Class hash_cache2(Of KEY_T As IComparable(Of KEY_T), VALUE_T, HASH_SIZE As _int64)
    Inherits hash_based(Of KEY_T, HASH_SIZE, icache2(Of KEY_T, VALUE_T))
    Implements icache2(Of KEY_T, VALUE_T)

    Public Sub New(Optional ByVal max_size As UInt64 = mhc.default_max_size,
                   Optional ByVal retire_ticks As UInt64 = mhc.default_retire_ticks)
        MyBase.New(Function() _cache2.mapheap_cache2(Of KEY_T, VALUE_T)(max_size, retire_ticks))
    End Sub

    Public Sub New(ByVal d As Func(Of icache2(Of KEY_T, VALUE_T)))
        MyBase.New(d)
    End Sub

    Private Function foreach(ByVal d As _do(Of icache2(Of KEY_T, VALUE_T), event_comb)) As event_comb
        assert(d IsNot Nothing)
        Dim i As Int32 = 0
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If i > 0 Then
                                      assert(ec IsNot Nothing)
                                      If Not ec.end_result() Then
                                          Return False
                                      End If
                                  End If
                                  If i = hash_size() Then
                                      Return goto_end()
                                  Else
                                      ec = d([select](i))
                                      i += 1
                                      Return waitfor(ec)
                                  End If
                              End Function)
    End Function

    Public Function clear() As event_comb Implements icache2(Of KEY_T, VALUE_T).clear
        Return foreach(Function(ByRef x) x.clear())
    End Function

    Public Function empty() As event_comb Implements icache2(Of KEY_T, VALUE_T).empty
        Return foreach(Function(ByRef x) x.empty())
    End Function

    Public Function [erase](ByVal key As KEY_T) As event_comb Implements icache2(Of KEY_T, VALUE_T).erase
        Return [select](key).erase(key)
    End Function

    Public Function [get](ByVal key As KEY_T, ByVal value As ref(Of VALUE_T)) As event_comb _
                         Implements icache2(Of KEY_T, VALUE_T).get
        Return [select](key).get(key, value)
    End Function

    Public Function have(ByVal key As KEY_T) As event_comb Implements icache2(Of KEY_T, VALUE_T).have
        Return [select](key).have(key)
    End Function

    Public Function [set](ByVal key As KEY_T, ByVal value As VALUE_T) As event_comb _
                         Implements icache2(Of KEY_T, VALUE_T).set
        Return [select](key).set(key, value)
    End Function

    Public Function size(ByVal value As ref(Of Int64)) As event_comb Implements icache2(Of KEY_T, VALUE_T).size
        Dim r As Int64 = 0
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = foreach(Function(ByRef x As icache2(Of KEY_T, VALUE_T)) As event_comb
                                                   Dim ec2 As event_comb = Nothing
                                                   Dim p As ref(Of Int64) = Nothing
                                                   Dim i As icache2(Of KEY_T, VALUE_T) = Nothing
                                                   i = x
                                                   Return New event_comb(Function() As Boolean
                                                                             p = New ref(Of Int64)()
                                                                             ec2 = i.size(p)
                                                                             Return waitfor(ec2) AndAlso
                                                                                    goto_next()
                                                                         End Function,
                                                                         Function() As Boolean
                                                                             If ec2.end_result() Then
                                                                                 r += (+p)
                                                                                 Return goto_end()
                                                                             Else
                                                                                 Return False
                                                                             End If
                                                                         End Function)
                                               End Function)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return eva(value, r) AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
