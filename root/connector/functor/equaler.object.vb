
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class equaler(Of T, T2)
    Private Shared Function equaler_object(Of IT, IT2) _
                                          (ByVal c As Func(Of IT, IT2, Boolean)) As Func(Of Object, Object, Boolean)
        assert(c IsNot Nothing)
        Return Function(ByVal i As Object, ByVal j As Object) As Boolean
                   Return c(direct_cast(Of IT)(i), direct_cast(Of IT2)(j))
               End Function
    End Function
End Class
