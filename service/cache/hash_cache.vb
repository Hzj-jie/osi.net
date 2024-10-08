
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.template
Imports mhc = osi.service.cache.constants.mapheap_cache

Public Class hash_cache(Of KEY_T As IComparable(Of KEY_T), VALUE_T)
    Inherits hash_cache(Of KEY_T, VALUE_T, _63)

    Public Sub New(Optional ByVal max_size As UInt64 = mhc.default_max_size,
                   Optional ByVal retire_ticks As UInt64 = mhc.default_retire_ticks)
        MyBase.New(max_size, retire_ticks)
    End Sub

    Public Sub New(ByVal d As Func(Of icache(Of KEY_T, VALUE_T)))
        MyBase.New(d)
    End Sub
End Class

Public Class hash_cache(Of KEY_T As IComparable(Of KEY_T), VALUE_T, HASH_SIZE As _int64)
    Inherits hash_based(Of KEY_T, HASH_SIZE, icache(Of KEY_T, VALUE_T))
    Implements icache(Of KEY_T, VALUE_T)

    Public Sub New(Optional ByVal max_size As UInt64 = mhc.default_max_size,
                   Optional ByVal retire_ticks As UInt64 = mhc.default_retire_ticks)
        MyBase.New(Function() _cache.mapheap_cache(Of KEY_T, VALUE_T)(max_size, retire_ticks))
    End Sub

    Public Sub New(ByVal d As Func(Of icache(Of KEY_T, VALUE_T)))
        MyBase.New(d)
    End Sub

    Public Function [get](ByVal key As KEY_T) As VALUE_T Implements icache(Of KEY_T, VALUE_T).get
        Return [select](key).get(key)
    End Function

    Public Sub clear() Implements icache(Of KEY_T, VALUE_T).clear
        For i As Int32 = 0 To hash_size() - 1
            [select](i).clear()
        Next
    End Sub

    Public Function [erase](ByVal key As KEY_T) As Boolean Implements icache(Of KEY_T, VALUE_T).erase
        Return [select](key).erase(key)
    End Function

    Public Function [get](ByVal key As KEY_T, ByRef value As VALUE_T) As Boolean _
                         Implements icache(Of KEY_T, VALUE_T).get
        Return [select](key).get(key, value)
    End Function

    Public Sub [set](ByVal key As KEY_T, ByVal value As VALUE_T) Implements icache(Of KEY_T, VALUE_T).set
        [select](key).set(key, value)
    End Sub

    Public Function size() As Int64 Implements icache(Of KEY_T, VALUE_T).size
        Dim r As Int64 = 0
        For i As Int32 = 0 To hash_size() - 1
            r += [select](i).size()
        Next
        Return r
    End Function

    Public Function empty() As Boolean Implements icache(Of KEY_T, VALUE_T).empty
        Dim r As Int64 = 0
        For i As Int32 = 0 To hash_size() - 1
            If Not [select](i).empty() Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Function have(ByVal key As KEY_T) As Boolean Implements icache(Of KEY_T, VALUE_T).have
        Return [select](key).have(key)
    End Function
End Class
