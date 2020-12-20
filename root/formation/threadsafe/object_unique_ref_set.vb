
Imports osi.root.connector
Imports osi.root.template
Imports osi.root.formation

Public Class object_unique_ref_set(Of T As Class, threadsafe As _boolean)
    Inherits object_unique_set(Of ref(Of T), threadsafe)

    Protected Overrides Function object_same(ByVal i As ref(Of T), ByVal j As ref(Of T)) As Boolean
        Return object_compare(+i, +j) = 0
    End Function

    Public Overloads Function insert(ByVal i As T) As Boolean
        Return MyBase.insert(New ref(Of T)(i))
    End Function

    Public Overloads Function [erase](ByVal i As T) As Boolean
        Return MyBase.erase(New ref(Of T)(i))
    End Function
End Class

Public Class object_unique_ref_set(Of T As Class)
    Inherits object_unique_ref_set(Of T, _true)
End Class
