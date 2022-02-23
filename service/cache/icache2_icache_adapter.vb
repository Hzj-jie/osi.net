
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure

' Convert an icache2 implementation to icache; this is typically for tests only.
Public NotInheritable Class icache2_icache_adapter(Of KEY_T As IComparable(Of KEY_T), VALUE_T)
    Implements icache(Of KEY_T, VALUE_T)

    Private ReadOnly i As icache2(Of KEY_T, VALUE_T)

    Public Sub New(ByVal i As icache2(Of KEY_T, VALUE_T))
        assert(i IsNot Nothing)
        Me.i = i
    End Sub

    Public Sub clear() Implements islimcache(Of KEY_T, VALUE_T).clear
        async_sync(i.clear())
    End Sub

    Public Sub [set](ByVal key As KEY_T, ByVal value As VALUE_T) Implements islimcache(Of KEY_T, VALUE_T).set
        async_sync(i.set(key, value))
    End Sub

    Public Function empty() As Boolean Implements islimcache2(Of KEY_T, VALUE_T).empty
        Return async_sync(i.empty())
    End Function

    Public Function [erase](ByVal key As KEY_T) As Boolean Implements islimcache(Of KEY_T, VALUE_T).erase
        Return async_sync(i.erase(key))
    End Function

    Public Function [get](ByVal key As KEY_T) As VALUE_T Implements icache(Of KEY_T, VALUE_T).get
        Dim r As ref(Of VALUE_T) = Nothing
        r = New ref(Of VALUE_T)()
        If async_sync(i.get(key, r)) Then
            Return +r
        End If
        Return Nothing
    End Function

    Public Function [get](ByVal key As KEY_T, ByRef value As VALUE_T) As Boolean Implements islimcache(Of KEY_T, VALUE_T).get
        Dim r As ref(Of VALUE_T) = Nothing
        r = New ref(Of VALUE_T)()
        If async_sync(i.get(key, r)) Then
            value = (+r)
            Return True
        End If
        Return False
    End Function

    Public Function have(ByVal key As KEY_T) As Boolean Implements islimcache2(Of KEY_T, VALUE_T).have
        Return async_sync(i.have(key))
    End Function

    Public Function size() As Int64 Implements islimcache(Of KEY_T, VALUE_T).size
        Dim r As ref(Of Int64) = Nothing
        r = New ref(Of Int64)()
        If async_sync(i.size(r)) Then
            Return +r
        End If
        Return 0
    End Function
End Class
