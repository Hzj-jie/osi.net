
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

' Convert an icache implementation to icache2; this is typically for tests only.
Public Class icache_icache2_adapter(Of KEY_T As IComparable(Of KEY_T), VALUE_T)
    Implements icache2(Of KEY_T, VALUE_T)

    Private ReadOnly i As icache(Of KEY_T, VALUE_T)

    Public Sub New(ByVal i As icache(Of KEY_T, VALUE_T))
        assert(Not i Is Nothing)
        Me.i = i
    End Sub

    Public Function clear() As event_comb Implements icache2(Of KEY_T, VALUE_T).clear
        Return sync_async(Sub()
                              i.clear()
                          End Sub)
    End Function

    Public Function empty() As event_comb Implements icache2(Of KEY_T, VALUE_T).empty
        Return sync_async(Function() As Boolean
                              Return i.empty()
                          End Function)
    End Function

    Public Function [erase](ByVal key As KEY_T) As event_comb Implements icache2(Of KEY_T, VALUE_T).erase
        Return sync_async(Function() As Boolean
                              Return i.erase(key)
                          End Function)
    End Function

    Public Function foreach(ByVal d As _do(Of KEY_T, VALUE_T, Boolean)) As event_comb _
                           Implements icache2(Of KEY_T, VALUE_T).foreach
        Return sync_async(Function() As Boolean
                              Return i.foreach(d)
                          End Function)
    End Function

    Public Function foreach(ByVal d As void(Of KEY_T, VALUE_T)) As event_comb _
                           Implements icache2(Of KEY_T, VALUE_T).foreach
        Return sync_async(Function() As Boolean
                              Return i.foreach(d)
                          End Function)
    End Function

    Public Function foreach(ByVal d As _do(Of KEY_T, VALUE_T, Boolean, Boolean)) As event_comb _
                           Implements icache2(Of KEY_T, VALUE_T).foreach
        Return sync_async(Function() As Boolean
                              Return i.foreach(d)
                          End Function)
    End Function

    Public Function [get](ByVal key As KEY_T, ByVal value As pointer(Of VALUE_T)) As event_comb _
                         Implements icache2(Of KEY_T, VALUE_T).get
        Dim r As VALUE_T = Nothing
        Return sync_async(Function() As Boolean
                              Return i.get(key, r) AndAlso
                                     eva(value, r)
                          End Function)
    End Function

    Public Function have(ByVal key As KEY_T) As event_comb Implements icache2(Of KEY_T, VALUE_T).have
        Return sync_async(Function() As Boolean
                              Return i.have(key)
                          End Function)
    End Function

    Public Function [set](ByVal key As KEY_T, ByVal value As VALUE_T) As event_comb _
                         Implements icache2(Of KEY_T, VALUE_T).set
        Return sync_async(Sub()
                              i.set(key, value)
                          End Sub)
    End Function

    Public Function size(ByVal value As pointer(Of Int64)) As event_comb Implements icache2(Of KEY_T, VALUE_T).size
        Return sync_async(Sub()
                              eva(value, i.size())
                          End Sub)
    End Function
End Class
