
Public Class big_uint_binary_calculator
    Implements ibinary_calculator(Of big_uint)

    Public Shared ReadOnly instance As big_uint_binary_calculator

    Shared Sub New()
        instance = New big_uint_binary_calculator()
    End Sub

    Private Sub New()
    End Sub

    Public Function decrement(ByRef this As big_uint,
                              ByVal that As big_uint) As calculator_error _
                             Implements ibinary_calculator(Of big_uint).decrement
        Dim overflow As Boolean = False
        this.sub(that, overflow)
        Return If(overflow, calculator_error.overflow_error, Nothing)
    End Function

    Public Function divide(ByRef this As big_uint,
                           ByVal that As big_uint,
                           ByRef remainder As big_uint) As calculator_error _
                          Implements ibinary_calculator(Of big_uint).divide
        Dim divide_by_zero As Boolean = False
        this.divide(that, divide_by_zero, remainder)
        Return If(divide_by_zero, calculator_error.divide_by_zero_error, Nothing)
    End Function

    Public Function equal(ByRef this As big_uint,
                          ByVal that As big_uint) As calculator_error _
                         Implements ibinary_calculator(Of big_uint).equal
        this.replace_by(this.equal(that))
        Return Nothing
    End Function

    Public Function extract(ByRef this As big_uint,
                            ByVal that As big_uint,
                            ByRef remainder As big_uint) As calculator_error _
                           Implements ibinary_calculator(Of big_uint).extract
        Dim divide_by_zero As Boolean = False
        this.extract(that, divide_by_zero, remainder)
        Return If(divide_by_zero, calculator_error.divide_by_zero_error, Nothing)
    End Function

    Public Function increment(ByRef this As big_uint,
                              ByVal that As big_uint) As calculator_error _
                             Implements ibinary_calculator(Of big_uint).increment
        this.add(that)
        Return Nothing
    End Function

    Public Function left_shift(ByRef this As big_uint,
                               ByVal that As big_uint) As calculator_error _
                              Implements ibinary_calculator(Of big_uint).left_shift
        Dim overflow As Boolean = False
        this.left_shift(that, overflow)
        Return If(overflow, calculator_error.overflow_error, Nothing)
    End Function

    Public Function less(ByRef this As big_uint,
                         ByVal that As big_uint) As calculator_error _
                        Implements ibinary_calculator(Of big_uint).less
        this.replace_by(this.less(that))
        Return Nothing
    End Function

    Public Function less_or_equal(ByRef this As big_uint,
                                  ByVal that As big_uint) As calculator_error _
                                 Implements ibinary_calculator(Of big_uint).less_or_equal
        this.replace_by(this.less_or_equal(that))
        Return Nothing
    End Function

    Public Function [mod](ByRef this As big_uint,
                          ByVal that As big_uint) As calculator_error _
                         Implements ibinary_calculator(Of big_uint).mod
        Dim r As big_uint = Nothing
        Dim divide_by_zero As Boolean = False
        this.divide(that, divide_by_zero, r)
        If divide_by_zero Then
            Return calculator_error.divide_by_zero_error
        Else
            this.replace_by(r)
            Return Nothing
        End If
    End Function

    Public Function more(ByRef this As big_uint,
                         ByVal that As big_uint) As calculator_error _
                        Implements ibinary_calculator(Of big_uint).more
        this.replace_by(that.less(this))
        Return Nothing
    End Function

    Public Function more_or_equal(ByRef this As big_uint,
                                  ByVal that As big_uint) As calculator_error _
                                 Implements ibinary_calculator(Of big_uint).more_or_equal
        this.replace_by(that.less_or_equal(this))
        Return Nothing
    End Function

    Public Function multiply(ByRef this As big_uint,
                             ByVal that As big_uint) As calculator_error _
                            Implements ibinary_calculator(Of big_uint).multiply
        this.multiply(that)
        Return Nothing
    End Function

    Public Function not_equal(ByRef this As big_uint,
                              ByVal that As big_uint) As calculator_error _
                             Implements ibinary_calculator(Of big_uint).not_equal
        this.replace_by(Not this.equal(that))
        Return Nothing
    End Function

    Public Function power(ByRef this As big_uint,
                          ByVal that As big_uint) As calculator_error _
                         Implements ibinary_calculator(Of big_uint).power
        this.power(that)
        Return Nothing
    End Function

    Public Function right_shift(ByRef this As big_uint,
                                ByVal that As big_uint) As calculator_error _
                               Implements ibinary_calculator(Of big_uint).right_shift
        Dim overflow As Boolean = False
        this.right_shift(that, overflow)
        Return If(overflow, calculator_error.overflow_error, Nothing)
    End Function
End Class
