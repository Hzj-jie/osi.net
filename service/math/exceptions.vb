
Public Class ImaginaryNumberException
    Inherits Exception

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal inner_exception As Exception)
        MyBase.New(message, inner_exception)
    End Sub

    Public Sub New(ByVal info As Runtime.Serialization.SerializationInfo,
                   ByVal context As Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub
End Class

Public Module _exceptions
    Public Function divide_by_zero() As DivideByZeroException
        Return New DivideByZeroException()
    End Function

    Public Function overflow() As OverflowException
        Return New OverflowException()
    End Function

    Public Function imaginary_number() As Exception
        Return New ImaginaryNumberException()
    End Function
End Module
