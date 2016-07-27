
Imports osi.root.constants

Partial Public Class big_uint
    Public Shared Function zero() As big_uint
        Return New big_uint()
    End Function

    Public Shared Function _0() As big_uint
        Return zero()
    End Function

    Public Shared Function one() As big_uint
        Return New big_uint(uint32_1)
    End Function

    Public Shared Function _1() As big_uint
        Return one()
    End Function
End Class
