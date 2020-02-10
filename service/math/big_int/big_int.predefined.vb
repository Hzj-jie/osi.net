
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Partial Public NotInheritable Class big_int
    Public Shared Function zero() As big_int
        Return New big_int()
    End Function

    Public Shared Function _0() As big_int
        Return zero()
    End Function

    Public Shared Function one() As big_int
        Return New big_int(uint32_1)
    End Function

    Public Shared Function _1() As big_int
        Return one()
    End Function

    Public Shared Function negative_one() As big_int
        Return New big_int(-1)
    End Function

    Public Shared Function n1() As big_int
        Return negative_one()
    End Function
End Class
