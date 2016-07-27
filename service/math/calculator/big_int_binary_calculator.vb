
Public Class big_int_binary_calculator
    Implements ibinary_calculator(Of big_int)

    Public Shared ReadOnly instance As big_int_binary_calculator

    Shared Sub New()
        instance = New big_int_binary_calculator()
    End Sub

    Private Sub New()
    End Sub

    Public Function decrement(ByRef this As big_int,
                              ByVal that As big_int) As calculator_error _
                             Implements ibinary_calculator(Of big_int).decrement
        this.sub(that)
        Return Nothing
    End Function

    Public Function divide(ByRef this As big_int,
                           ByVal that As big_int,
                           ByRef remainder As big_int) As calculator_error _
                          Implements ibinary_calculator(Of big_int).divide
        Dim divide_by_zero As Boolean = False
        this.divide(that, divide_by_zero, remainder)
        Return If(divide_by_zero, calculator_error.divide_by_zero_error, Nothing)
    End Function

    Public Function equal(ByRef this As big_int,
                          ByVal that As big_int) As calculator_error _
                         Implements ibinary_calculator(Of big_int).equal
        this.replace_by(this.equal(that))
        Return Nothing
    End Function

    Public Function extract(ByRef this As big_int,
                            ByVal that As big_int,
                            ByRef remainder As big_int) As calculator_error _
                           Implements ibinary_calculator(Of big_int).extract
        Dim divide_by_zero As Boolean = False
        Dim imaginary_number As Boolean = False
        this.extract(that, divide_by_zero, imaginary_number, remainder)
        Return If(divide_by_zero,
                  calculator_error.divide_by_zero_error,
                  If(imaginary_number,
                     calculator_error.imaginary_number_error,
                     Nothing))
    End Function

    Public Function increment(ByRef this As big_int,
                              ByVal that As big_int) As calculator_error _
                             Implements ibinary_calculator(Of big_int).increment
        this.add(that)
        Return Nothing
    End Function

    Public Function left_shift(ByRef this As big_int,
                               ByVal that As big_int) As calculator_error _
                              Implements ibinary_calculator(Of big_int).left_shift
        Dim overflow As Boolean = False
        this.left_shift(that, overflow)
        Return If(overflow, calculator_error.overflow_error, Nothing)
    End Function

    Public Function less(ByRef this As big_int,
                         ByVal that As big_int) As calculator_error _
                        Implements ibinary_calculator(Of big_int).less
        this.replace_by(this.less(that))
        Return Nothing
    End Function

    Public Function less_or_equal(ByRef this As big_int,
                                  ByVal that As big_int) As calculator_error _
                                 Implements ibinary_calculator(Of big_int).less_or_equal
        this.replace_by(this.less_or_equal(that))
        Return Nothing
    End Function

    Public Function [mod](ByRef this As big_int,
                          ByVal that As big_int) As calculator_error _
                         Implements ibinary_calculator(Of big_int).mod
        Dim divide_by_zero As Boolean
        Dim r As big_int = Nothing
        this.divide(that, divide_by_zero, r)
        If divide_by_zero Then
            Return calculator_error.divide_by_zero_error
        Else
            this.replace_by(r)
            Return Nothing
        End If
    End Function

    Public Function more(ByRef this As big_int,
                         ByVal that As big_int) As calculator_error _
                        Implements ibinary_calculator(Of big_int).more
        this.replace_by(that.less(this))
        Return Nothing
    End Function

    Public Function more_or_equal(ByRef this As big_int,
                                  ByVal that As big_int) As calculator_error _
                                 Implements ibinary_calculator(Of big_int).more_or_equal
        this.replace_by(that.less_or_equal(this))
        Return Nothing
    End Function

    Public Function multiply(ByRef this As big_int,
                             ByVal that As big_int) As calculator_error _
                            Implements ibinary_calculator(Of big_int).multiply
        this.multiply(that)
        Return Nothing
    End Function

    Public Function not_equal(ByRef this As big_int,
                              ByVal that As big_int) As calculator_error _
                             Implements ibinary_calculator(Of big_int).not_equal
        this.replace_by(Not this.equal(that))
        Return Nothing
    End Function

    Public Function power(ByRef this As big_int,
                          ByVal that As big_int) As calculator_error _
                         Implements ibinary_calculator(Of big_int).power
        this.power(that)
        Return Nothing
    End Function

    Public Function right_shift(ByRef this As big_int,
                                ByVal that As big_int) As calculator_error _
                               Implements ibinary_calculator(Of big_int).right_shift
        Dim overflow As Boolean = False
        this.right_shift(that, overflow)
        Return If(overflow, calculator_error.overflow_error, Nothing)
    End Function
End Class
