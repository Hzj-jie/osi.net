
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class big_udec
    Public Shared Function random(Optional ByVal length As UInt32 = 0) As big_udec
        Dim n As big_uint = Nothing
        Dim d As big_uint = Nothing
        n = big_uint.random(length)
        Do
            d = big_uint.random(length)
        Loop While d.is_zero()
        Return New big_udec(n, d)
    End Function
End Class
