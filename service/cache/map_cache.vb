
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation

'for test only
Friend Class map_cache(Of KEY_T As IComparable(Of KEY_T), VALUE_T)
    Implements islimcache2(Of KEY_T, VALUE_T)

    Private ReadOnly m As map(Of KEY_T, VALUE_T)

    Public Sub New()
        m = New map(Of KEY_T, VALUE_T)()
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
        Dim it As map(Of KEY_T, VALUE_T).iterator = Nothing
        it = m.find(key)
        If it = m.end() Then
            Return False
        Else
            copy(value, (+it).second)
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
