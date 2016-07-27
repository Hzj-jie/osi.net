
Imports osi.root.delegates
Imports osi.root.connector
Imports osi.root.template
Imports osi.root.formation

Friend Class hashmap_cache(Of KEY_T As IComparable(Of KEY_T), VALUE_T, _SIZE As _int64)
    Implements islimcache2(Of KEY_T, VALUE_T)

    Private ReadOnly m As hashmap(Of KEY_T, VALUE_T, _SIZE)

    Public Sub New()
        m = New hashmap(Of KEY_T, VALUE_T, _SIZE)
    End Sub

    Public Sub clear() Implements islimcache2(Of KEY_T, VALUE_T).clear
        m.clear()
    End Sub

    Public Function [erase](ByVal key As KEY_T) As Boolean Implements islimcache2(Of KEY_T, VALUE_T).erase
        Return m.erase(key)
    End Function

    Public Function foreach(ByVal d As _do(Of KEY_T, VALUE_T, Boolean, Boolean)) As Boolean _
                           Implements islimcache2(Of KEY_T, VALUE_T).foreach
        Return m.foreach(d)
    End Function

    Public Function [get](ByVal key As KEY_T, ByRef value As VALUE_T) As Boolean _
                         Implements islimcache2(Of KEY_T, VALUE_T).get
        Dim i As hashmap(Of KEY_T, VALUE_T, _SIZE).iterator = Nothing
        i = m.find(key)
        If i = m.end() Then
            Return False
        Else
            copy(value, (+i).second)
            Return True
        End If
    End Function

    Public Sub [set](ByVal key As KEY_T, ByVal value As VALUE_T) Implements islimcache2(Of KEY_T, VALUE_T).set
        m(key) = value
    End Sub

    Public Function size() As Int64 Implements islimcache2(Of KEY_T, VALUE_T).size
        Return m.size()
    End Function

    Public Function empty() As Boolean Implements islimcache2(Of KEY_T, VALUE_T).empty
        Return m.empty()
    End Function

    Public Function have(ByVal key As KEY_T) As Boolean Implements islimcache2(Of KEY_T, VALUE_T).have
        Return m.find(key) <> m.end()
    End Function
End Class

Friend Class hashmap_cache(Of KEY_T As IComparable(Of KEY_T), VALUE_T)
    Inherits hashmap_cache(Of KEY_T, VALUE_T, _1023)
End Class
