
Partial Public Class big_int
    Public Function even() As Boolean
        Return d.even()
    End Function

    Public Function odd() As Boolean
        Return d.odd()
    End Function

    Public Function power_of_2() As Boolean
        Return positive() AndAlso d.power_of_2()
    End Function
End Class
