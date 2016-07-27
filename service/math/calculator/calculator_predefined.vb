
Public Class int_calculator
    Inherits calculator(Of Int32)

    Public Sub New()
        MyBase.New(int_binary_calculator.instance)
    End Sub
End Class

Public Class uint_calculator
    Inherits calculator(Of UInt32)

    Public Sub New()
        MyBase.New(uint_binary_calculator.instance)
    End Sub
End Class

Public Class big_uint_calculator
    Inherits calculator(Of big_uint)

    Public Sub New()
        MyBase.New(big_uint_binary_calculator.instance)
    End Sub
End Class

Public Class big_int_calculator
    Inherits calculator(Of big_int)

    Public Sub New()
        MyBase.New(big_int_binary_calculator.instance)
    End Sub
End Class
