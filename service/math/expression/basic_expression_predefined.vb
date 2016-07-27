
Public Class int_basic_expression
    Inherits basic_expression(Of Int32)

    Public Sub New()
        MyBase.New(New int_calculator(), int_parser.instance, int_outputter.instance)
    End Sub
End Class

Public Class uint_basic_expression
    Inherits basic_expression(Of UInt32)

    Public Sub New()
        MyBase.New(New uint_calculator(), uint_parser.instance, uint_outputter.instance)
    End Sub
End Class

Public Class big_int_basic_expression
    Inherits basic_expression(Of big_int)

    Public Sub New()
        MyBase.New(New big_int_calculator(), big_int_parser.instance, big_int_outputter.instance)
    End Sub
End Class

Public Class big_uint_basic_expression
    Inherits basic_expression(Of big_uint)

    Public Sub New()
        MyBase.New(New big_uint_calculator(), big_uint_parser.instance, big_uint_outputter.instance)
    End Sub
End Class
