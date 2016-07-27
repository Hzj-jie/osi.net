
Imports osi.root.delegates
Imports osi.root.connector

Friend Class slimcache2(Of KEY_T As IComparable(Of KEY_T), VALUE_T)
    Implements islimcache2(Of KEY_T, VALUE_T)

    Private ReadOnly s As islimcache(Of KEY_T, VALUE_T)

    Public Sub New(ByVal s As islimcache(Of KEY_T, VALUE_T))
        assert(Not s Is Nothing)
        assert(Not TypeOf s Is islimcache2(Of KEY_T, VALUE_T))
        assert(Not TypeOf s Is icache(Of KEY_T, VALUE_T))
        Me.s = s
    End Sub

    Public Sub clear() Implements islimcache2(Of KEY_T, VALUE_T).clear
        s.clear()
    End Sub

    Public Function [erase](ByVal key As KEY_T) As Boolean Implements islimcache2(Of KEY_T, VALUE_T).erase
        Return s.erase(key)
    End Function

    Public Function foreach(ByVal d As _do(Of KEY_T, VALUE_T, Boolean, Boolean)) As Boolean _
                           Implements islimcache2(Of KEY_T, VALUE_T).foreach
        Return s.foreach(d)
    End Function

    Public Function [get](ByVal key As KEY_T, ByRef value As VALUE_T) As Boolean _
                         Implements islimcache2(Of KEY_T, VALUE_T).get
        Return s.get(key, value)
    End Function

    Public Sub [set](ByVal key As KEY_T, ByVal value As VALUE_T) Implements islimcache2(Of KEY_T, VALUE_T).set
        s.set(key, value)
    End Sub

    Public Function size() As Int64 Implements islimcache2(Of KEY_T, VALUE_T).size
        Return s.size()
    End Function

    Public Function empty() As Boolean Implements islimcache2(Of KEY_T, VALUE_T).empty
        Return size() = 0
    End Function

    Public Function have(ByVal key As KEY_T) As Boolean Implements islimcache2(Of KEY_T, VALUE_T).have
        Return [get](key, Nothing)
    End Function
End Class
