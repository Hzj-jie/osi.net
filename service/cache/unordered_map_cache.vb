
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Friend NotInheritable Class unordered_map_cache(Of KEY_T As IComparable(Of KEY_T), VALUE_T)
    Implements islimcache2(Of KEY_T, VALUE_T)

    Private ReadOnly m As unordered_map(Of KEY_T, VALUE_T)

    Public Sub New()
        m = New unordered_map(Of KEY_T, VALUE_T)()
    End Sub

    Public Sub clear() Implements islimcache2(Of KEY_T, VALUE_T).clear
        m.clear()
    End Sub

    Public Function [erase](ByVal key As KEY_T) As Boolean Implements islimcache2(Of KEY_T, VALUE_T).erase
        Return m.erase(key)
    End Function

    Public Function [get](ByVal key As KEY_T, ByRef value As VALUE_T) As Boolean _
                         Implements islimcache2(Of KEY_T, VALUE_T).get
        Dim i As unordered_map(Of KEY_T, VALUE_T).iterator = Nothing
        i = m.find(key)
        If i = m.end() Then
            Return False
        End If
        copy(value, (+i).second)
        Return True
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
