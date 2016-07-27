
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.template

Public Class unique_weak_map(Of KEY_T As IComparable(Of KEY_T), VALUE_T, HASH_SIZE As _int64)
    Inherits unique_map(Of KEY_T, weak_pointer(Of VALUE_T), VALUE_T, HASH_SIZE)

    Protected NotOverridable Overrides Function store_value(ByVal i As weak_pointer(Of VALUE_T),
                                                            ByRef o As VALUE_T) As Boolean
        assert(Not i Is Nothing)
        Return i.get(o)
    End Function

    Protected NotOverridable Overrides Function value_store(ByVal i As VALUE_T) As weak_pointer(Of VALUE_T)
        Return New weak_pointer(Of VALUE_T)(i)
    End Function
End Class

Public Class unique_weak_map(Of KEY_T As IComparable(Of KEY_T), VALUE_T)
    Inherits unique_weak_map(Of KEY_T, VALUE_T, _1023)
End Class