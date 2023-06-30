
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class comparer(Of T, T2)
    Private Shared Function comparer_object(Of IT, IT2) _
                                           (ByVal c As Func(Of IT, IT2, Int32)) As Func(Of Object, Object, Int32)
        assert(Not c Is Nothing)
        Return Function(ByVal i As Object, ByVal j As Object) As Int32
                   Return c(direct_cast(Of IT)(i), direct_cast(Of IT2)(j))
               End Function
    End Function
End Class