
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public NotInheritable Class unique_weak_map(Of KEY_T As IComparable(Of KEY_T), VALUE_T)
    Inherits unique_map(Of KEY_T, weak_ref(Of VALUE_T), VALUE_T)

    Protected Overrides Function store_value(ByVal i As weak_ref(Of VALUE_T), ByRef o As VALUE_T) As Boolean
        assert(Not i Is Nothing)
        Return i.get(o)
    End Function

    Protected Overrides Function value_store(ByVal i As VALUE_T) As weak_ref(Of VALUE_T)
        Return New weak_ref(Of VALUE_T)(i)
    End Function
End Class
