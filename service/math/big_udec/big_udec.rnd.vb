
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class big_udec
    Public Shared Function random(Optional ByVal length As UInt32 = 0) As big_udec
        If length = 0 Then
            length = rnd_uint(100, 1000)
        End If
        Dim n As big_uint = Nothing
        Dim d As big_uint = Nothing
        n = big_uint.random(CUInt(length * rnd_double(0.9, 1.5)))
        Do
            d = big_uint.random(length)
        Loop While d.is_zero()
        Return New big_udec(n, d)
    End Function
End Class
