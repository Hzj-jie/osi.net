
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class big_int
    Public Sub replace_by(ByVal i As Boolean)
        d.replace_by(i)
        set_signal(Not i)
    End Sub

    Public Shared Widening Operator CType(ByVal this As Boolean) As big_int
        Dim r As big_int = Nothing
        r = New big_int()
        r.replace_by(this)
        Return r
    End Operator

    Public Function power_of_2() As Boolean
        Return positive() AndAlso d.power_of_2()
    End Function

    Public Function as_uint64(ByRef overflow As Boolean) As UInt64
        If negative() Then
            overflow = True
            Return 0
        End If
        Return d.as_uint64(overflow)
    End Function

    Public Function as_uint32(ByRef overflow As Boolean) As UInt32
        If negative() Then
            overflow = True
            Return 0
        End If
        Return d.as_uint32(overflow)
    End Function

    Public Function as_int32(ByRef overflow As Boolean) As Int32
        Dim v As Int32 = 0
        v = d.as_int32(overflow)
        Return If(negative(), -v, v)
    End Function

    Private Sub divide(ByVal that As big_int, ByRef remainder As big_int, ByRef divide_by_zero As Boolean)
        If that Is Nothing OrElse that.is_zero() Then
            divide_by_zero = True
            Return
        End If
        Dim n As Boolean = False
        n = negative()
        Dim r As big_uint = Nothing
        d.divide(that.d, divide_by_zero, r)
        assert(Not divide_by_zero)
        remainder = share(r)
        confirm_signal()
        If n Then
            remainder.set_negative()
        End If
        If n = that.negative() Then
            set_positive()
        Else
            set_negative()
        End If
    End Sub

    Private Sub extract(ByVal that As big_int,
                        ByRef remainder As big_int,
                        ByRef divide_by_zero As Boolean,
                        ByRef imaginary_number As Boolean)
        divide_by_zero = False
        imaginary_number = False
        If that Is Nothing OrElse that.is_zero() Then
            If is_one() Then
                divide_by_zero = False
                remainder = zero()
            Else
                divide_by_zero = True
            End If
            Return
        End If
        imaginary_number = (negative() AndAlso that.even())
        If imaginary_number Then
            Return
        End If
        remainder = New big_int()
        If is_zero_or_one_or_negative_one() Then
            Return
        End If
        If that.negative() Then
            remainder.replace_by(Me)
            set_zero()
        Else
            Dim r As big_uint = Nothing
            d.extract(that.d, divide_by_zero, r)
            remainder.replace_by(r)
            remainder.set_signal(signal())
        End If
        confirm_signal()
    End Sub

    Public Function divide(ByVal that As big_uint,
                           ByRef divide_by_zero As Boolean,
                           Optional ByRef remainder As big_int = Nothing) As big_int
        Return divide(share(that), divide_by_zero, remainder)
    End Function

    Public Function divide(ByVal that As big_int,
                           ByRef divide_by_zero As Boolean,
                           Optional ByRef remainder As big_int = Nothing) As big_int
        divide(that, remainder, divide_by_zero)
        Return Me
    End Function

    Public Function divide(ByVal that As big_uint,
                           Optional ByRef remainder As big_int = Nothing) As big_int
        Return divide(share(that), remainder)
    End Function

    Public Function divide(ByVal that As big_int,
                           Optional ByRef remainder As big_int = Nothing) As big_int
        Dim r As Boolean = False
        divide(that, remainder, r)
        If r Then
            Throw divide_by_zero()
        End If
        Return Me
    End Function

    Public Function assert_divide(ByVal that As big_uint,
                                  Optional ByRef remainder As big_int = Nothing) As big_int
        Return assert_divide(share(that), remainder)
    End Function

    Public Function assert_divide(ByVal that As big_int,
                                  Optional ByRef remainder As big_int = Nothing) As big_int
        Dim r As Boolean = False
        divide(that, remainder, r)
        assert(Not r)
        Return Me
    End Function

    Public Function modulus(ByVal that As big_uint,
                            ByRef divide_by_zero As Boolean) As big_int
        Return modulus(share(that), divide_by_zero)
    End Function

    Public Function modulus(ByVal that As big_int,
                            ByRef divide_by_zero As Boolean) As big_int
        If that Is Nothing OrElse that.is_zero() Then
            divide_by_zero = True
            Return Me
        End If
        d.modulus(that.d, divide_by_zero)
        assert(Not divide_by_zero)
        confirm_signal()
        Return Me
    End Function

    Public Function modulus(ByVal that As big_uint) As big_int
        Return modulus(share(that))
    End Function

    Public Function modulus(ByVal that As big_int) As big_int
        Dim r As Boolean = False
        modulus(that, r)
        If r Then
            Throw divide_by_zero()
        End If
        Return Me
    End Function

    Public Function assert_modulus(ByVal that As big_uint) As big_int
        Return assert_modulus(share(that))
    End Function

    Public Function assert_modulus(ByVal that As big_int) As big_int
        Dim r As Boolean = False
        modulus(that, r)
        assert(Not r)
        Return Me
    End Function

    Public Function extract(ByVal that As big_uint,
                            ByRef divide_by_zero As Boolean,
                            ByRef imaginary_number As Boolean,
                            Optional ByRef remainder As big_int = Nothing) As big_int
        Return extract(share(that), divide_by_zero, imaginary_number, remainder)
    End Function

    Public Function extract(ByVal that As big_int,
                            ByRef divide_by_zero As Boolean,
                            ByRef imaginary_number As Boolean,
                            Optional ByRef remainder As big_int = Nothing) As big_int
        extract(that, remainder, divide_by_zero, imaginary_number)
        Return Me
    End Function

    Public Function extract(ByVal that As big_uint,
                            Optional ByRef remainder As big_int = Nothing) As big_int
        Return extract(share(that), remainder)
    End Function

    Public Function extract(ByVal that As big_int,
                            Optional ByRef remainder As big_int = Nothing) As big_int
        Dim d As Boolean = False
        Dim i As Boolean = False
        extract(that, remainder, d, i)
        If d Then
            Throw divide_by_zero()
        End If
        If i Then
            Throw imaginary_number()
        End If
        Return Me
    End Function

    Public Function assert_extract(ByVal that As big_uint,
                                   Optional ByRef remainder As big_int = Nothing) As big_int
        Return assert_extract(share(that), remainder)
    End Function

    Public Function assert_extract(ByVal that As big_int,
                                   Optional ByRef remainder As big_int = Nothing) As big_int
        Dim d As Boolean = False
        Dim i As Boolean = False
        extract(that, remainder, d, i)
        assert(Not d)
        assert(Not i)
        Return Me
    End Function

    Public Shared Operator /(ByVal this As big_int, ByVal that As big_int) As pair(Of big_int, big_int)
        Dim q As big_int = Nothing
        Dim r As big_int = Nothing
        q = New big_int(this)
        q.divide(that, r)
        Return pair.emplace_of(q, r)
    End Operator

    Public Shared Operator Mod(ByVal this As big_int, ByVal that As big_int) As big_int
        Dim q As big_int = Nothing
        q = New big_int(this)
        q.modulus(that)
        Return q
    End Operator
End Class
