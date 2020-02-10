
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class big_int
    Public Shared Function support_base(ByVal b As Byte) As Boolean
        Return big_uint.support_base(b)
    End Function
End Class
