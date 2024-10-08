
'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with uint_binary_calculator.vbp ----------
'so change uint_binary_calculator.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with binary_calculator.vbp ----------
'so change binary_calculator.vbp instead of this file


Public Class uint_binary_calculator
    Implements ibinary_calculator(Of UInt32)

    Public Shared ReadOnly instance As uint_binary_calculator

    Shared Sub New()
        instance = New uint_binary_calculator()
    End Sub

    Private Sub New()
    End Sub

    Public Function decrement(ByRef this As UInt32,
                              ByVal that As UInt32) As calculator_error _
                             Implements ibinary_calculator(Of UInt32).decrement
        Try
            this = this - that
            Return Nothing
        Catch ex As OverflowException
            Return calculator_error.overflow_error
        End Try
    End Function

    Public Function divide(ByRef this As UInt32,
                           ByVal that As UInt32,
                           ByRef remainder As UInt32) As calculator_error _
                          Implements ibinary_calculator(Of UInt32).divide
        Try
            remainder = this Mod that
            this = this \ that
            Return Nothing
        Catch ex As DivideByZeroException
            Return calculator_error.divide_by_zero_error
        End Try
    End Function

    Public Function equal(ByRef this As UInt32,
                          ByVal that As UInt32) As calculator_error _
                         Implements ibinary_calculator(Of UInt32).equal
        this = (this = that)
        Return Nothing
    End Function

    Public Function extract(ByRef this As UInt32,
                            ByVal that As UInt32,
                            ByRef remainder As UInt32) As calculator_error _
                           Implements ibinary_calculator(Of UInt32).extract
        If this < 0 AndAlso that.even() Then
            'the overflow check has been disabled, which means we need to detect it ourselves.
            Return calculator_error.imaginary_number_error
        End If
        Try
            Dim r As UInt32 = 0
            r = (1 \ that)  'trigger divide by zero exception
            r = this ^ (1 / that)
            remainder = this - (r ^ that)
            this = r
            Return Nothing
        Catch ex As OverflowException
            Return calculator_error.imaginary_number_error
        Catch ex As DivideByZeroException
            Return calculator_error.divide_by_zero_error
        End Try
    End Function

    Public Function increment(ByRef this As UInt32,
                              ByVal that As UInt32) As calculator_error _
                             Implements ibinary_calculator(Of UInt32).increment
        Try
            this += that
            Return Nothing
        Catch ex As OverflowException
            Return calculator_error.overflow_error
        End Try
    End Function

    Public Function left_shift(ByRef this As UInt32,
                               ByVal that As UInt32) As calculator_error _
                              Implements ibinary_calculator(Of UInt32).left_shift
        this <<= that
        Return Nothing
    End Function

    Public Function less(ByRef this As UInt32,
                         ByVal that As UInt32) As calculator_error _
                        Implements ibinary_calculator(Of UInt32).less
        this = (this < that)
        Return Nothing
    End Function

    Public Function less_or_equal(ByRef this As UInt32,
                                  ByVal that As UInt32) As calculator_error _
                                 Implements ibinary_calculator(Of UInt32).less_or_equal
        this = (this <= that)
        Return Nothing
    End Function

    Public Function [mod](ByRef this As UInt32,
                          ByVal that As UInt32) As calculator_error _
                         Implements ibinary_calculator(Of UInt32).mod
        Try
            this = this Mod that
            Return Nothing
        Catch ex As DivideByZeroException
            Return calculator_error.divide_by_zero_error
        End Try
    End Function

    Public Function more(ByRef this As UInt32,
                         ByVal that As UInt32) As calculator_error _
                        Implements ibinary_calculator(Of UInt32).more
        this = (this > that)
        Return Nothing
    End Function

    Public Function more_or_equal(ByRef this As UInt32,
                                  ByVal that As UInt32) As calculator_error _
                                 Implements ibinary_calculator(Of UInt32).more_or_equal
        this = (this >= that)
        Return Nothing
    End Function

    Public Function multiply(ByRef this As UInt32,
                             ByVal that As UInt32) As calculator_error _
                            Implements ibinary_calculator(Of UInt32).multiply
        Try
            this *= that
            Return Nothing
        Catch ex As OverflowException
            Return calculator_error.overflow_error
        End Try
    End Function

    Public Function not_equal(ByRef this As UInt32,
                              ByVal that As UInt32) As calculator_error _
                             Implements ibinary_calculator(Of UInt32).not_equal
        this = (this <> that)
        Return Nothing
    End Function

    Public Function power(ByRef this As UInt32,
                          ByVal that As UInt32) As calculator_error _
                         Implements ibinary_calculator(Of UInt32).power
        Try
            this ^= that
            Return Nothing
        Catch ex As OverflowException
            Return calculator_error.overflow_error
        End Try
    End Function

    Public Function right_shift(ByRef this As UInt32,
                                ByVal that As UInt32) As calculator_error _
                               Implements ibinary_calculator(Of UInt32).right_shift
        this >>= that
        Return Nothing
    End Function
End Class
'finish binary_calculator.vbp --------
'finish uint_binary_calculator.vbp --------
