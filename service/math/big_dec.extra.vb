
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class big_dec
    Public Function divide(ByVal that As big_udec,
                           ByRef divide_by_zero As Boolean) As big_dec
        Return divide(share(that), divide_by_zero)
    End Function

    Public Function divide(ByVal that As big_dec,
                           ByRef divide_by_zero As Boolean) As big_dec
        If that Is Nothing OrElse that.is_zero() Then
            divide_by_zero = True
            Return Me
        End If
        Dim n As Boolean = False
        n = negative()
        d.divide(that.d, divide_by_zero)
        confirm_signal()
        If n = that.negative() Then
            set_positive()
        Else
            set_negative()
        End If
        Return Me
    End Function

    Public Function divide(ByVal that As big_udec) As big_dec
        Return divide(share(that))
    End Function

    Public Function divide(ByVal that As big_dec) As big_dec
        Dim r As Boolean = False
        divide(that, r)
        If r Then
            Throw divide_by_zero()
        End If
        Return Me
    End Function

    Public Function assert_divide(ByVal that As big_udec) As big_dec
        Return assert_divide(share(that))
    End Function

    Public Function assert_divide(ByVal that As big_dec) As big_dec
        Dim r As Boolean = False
        divide(that, r)
        assert(Not r)
        Return Me
    End Function

    Public Function extract(ByVal that As big_udec,
                            ByRef divide_by_zero As Boolean,
                            ByRef imaginary_number As Boolean) As big_dec
        Return extract(share(that), divide_by_zero, imaginary_number)
    End Function

    Public Function extract(ByVal that As big_dec,
                            ByRef divide_by_zero As Boolean,
                            ByRef imaginary_number As Boolean) As big_dec
        divide_by_zero = False
        imaginary_number = False
        If that Is Nothing OrElse that.is_zero() Then
            divide_by_zero = Not is_one()
            Return Me
        End If
        imaginary_number = (negative() AndAlso that.even())
        If imaginary_number Then
            Return Me
        End If
        If is_zero_or_one_or_negative_one() Then
            Return Me
        End If
        If that.negative() Then
            d.extract(that.d.assert_reciprocal(), divide_by_zero)
        Else
            d.extract(that.d, divide_by_zero)
        End If
        confirm_signal()
        Return Me
    End Function

    Public Function extract(ByVal that As big_udec) As big_dec
        Return extract(share(that))
    End Function

    Public Function extract(ByVal that As big_dec) As big_dec
        Dim d As Boolean = False
        Dim i As Boolean = False
        extract(that, d, i)
        If d Then
            Throw divide_by_zero()
        End If
        If i Then
            Throw imaginary_number()
        End If
        Return Me
    End Function

    Public Function assert_extract(ByVal that As big_udec) As big_dec
        Return assert_extract(share(that))
    End Function

    Public Function assert_extract(ByVal that As big_dec) As big_dec
        Dim d As Boolean = False
        Dim i As Boolean = False
        extract(that, d, i)
        assert(Not d)
        assert(Not i)
        Return Me
    End Function

    Public Shared Operator /(ByVal this As big_dec, ByVal that As big_dec) As big_dec
        Return this.CloneT().divide(that)
    End Operator
End Class
