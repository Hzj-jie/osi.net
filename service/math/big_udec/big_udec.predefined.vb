
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class big_udec
    Public Shared Function zero() As big_udec
        Return New big_udec()
    End Function

    Public Shared Function one() As big_udec
        Return New big_udec(big_uint.one(), big_uint.one())
    End Function
End Class
