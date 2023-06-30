
Option Explicit On
Option Infer Off
Option Strict On

#Const REDUCE_FRACTION_WHEN_CALCULATING = False

Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class big_udec
    Public Function add(ByVal that As big_udec) As big_udec
        If that Is Nothing OrElse that.is_zero() Then
            Return Me
        End If
        If is_zero() Then
            replace_by(that)
            Return Me
        End If

        If Me.d.equal(that.d) Then
            replace_by(Me.n + that.n, Me.d)
#If REDUCE_FRACTION_WHEN_CALCULATING Then
            reduce_fraction()
#End If
        Else
#If REDUCE_FRACTION_WHEN_CALCULATING Then
            Dim g As big_uint = Nothing
            g = big_uint.gcd(Me.d, that.d)
            Dim c As big_uint = Nothing
            Me.d.assert_divide(g, c)
            assert(c.is_zero())
            replace_by((that.d \ g) * Me.n + that.n * Me.d, that.d * Me.d)
#Else
            replace_by(that.d * Me.n + that.n * Me.d, that.d * Me.d)
#End If
        End If

#If Not REDUCE_FRACTION_WHEN_CALCULATING Then
        increase_fraction_dirty_rate()
#End If
        Return Me
    End Function

    ' Replace by (x - y) / d
    Private Sub [sub](ByVal x As big_uint, ByVal y As big_uint, ByVal d As big_uint, ByRef overflow As Boolean)
        assert(Not x Is Nothing)
        assert(Not y Is Nothing)
        assert(Not d Is Nothing)
        overflow = False
        If y.is_zero() Then
            assert(replace_by(x, d))
        ElseIf y.less_or_equal(x) Then
            assert(replace_by(x - y, d))
        Else
            overflow = True
            y -= x
            Dim r As big_uint = Nothing
            y.assert_divide(d, r)
            If r.is_zero() Then
                set_zero()
            Else
                assert(replace_by(d - r, d))
            End If
        End If
    End Sub

    Public Function [sub](ByVal that As big_udec, ByRef overflow As Boolean) As big_udec
        If that Is Nothing OrElse that.is_zero() Then
            overflow = False
            Return Me
        End If
        If is_zero() Then
            [sub](big_uint.zero(), that.n, that.d, overflow)
            Return Me
        End If

        If Me.d.equal(that.d) Then
            [sub](Me.n, that.n.CloneT(), Me.d, overflow)
#If REDUCE_FRACTION_WHEN_CALCULATING Then
            reduce_fraction()
#End If
        Else
#If REDUCE_FRACTION_WHEN_CALCULATING Then
            Dim g As big_uint = Nothing
            g = big_uint.gcd(Me.d, that.d)
            Dim c As big_uint = Nothing
            Me.d.assert_divide(g, c)
            assert(c.is_zero())
            [sub]((that.d \ g) * Me.n, Me.d * that.n, Me.d * that.d, overflow)
#Else
            [sub](that.d * Me.n, Me.d * that.n, Me.d * that.d, overflow)
#End If
        End If

#If Not REDUCE_FRACTION_WHEN_CALCULATING Then
        increase_fraction_dirty_rate()
#End If
        Return Me
    End Function

    Public Function [sub](ByVal that As big_udec) As big_udec
        Dim overflow As Boolean = False
        Dim r As big_udec = Nothing
        r = [sub](that, overflow)
        If overflow Then
            throws.overflow()
        End If
        Return r
    End Function

    Public Function assert_sub(ByVal that As big_udec) As big_udec
        Dim overflow As Boolean = False
        Dim r As big_udec = Nothing
        r = [sub](that, overflow)
        assert(Not overflow)
        Return r
    End Function

    Public Function multiply(ByVal that As big_udec) As big_udec
        If that Is Nothing OrElse that.is_zero() Then
            set_zero()
            Return Me
        End If
        If is_zero() Then
            Return Me
        End If

        Dim n1 As big_uint = Nothing
        Dim n2 As big_uint = Nothing
        Dim d1 As big_uint = Nothing
        Dim d2 As big_uint = Nothing
        n1 = Me.n.CloneT()
        n2 = that.n.CloneT()
        d1 = Me.d.CloneT()
        d2 = that.d.CloneT()
#If REDUCE_FRACTION_WHEN_CALCULATING Then
        reduce_fraction(n1, d2)
        reduce_fraction(n2, d1)
#Else
        fast_reduce_fraction(n1, d2)
        fast_reduce_fraction(n2, d1)
#End If
        replace_by(n1.multiply(n2), d1.multiply(d2))

#If Not REDUCE_FRACTION_WHEN_CALCULATING Then
        increase_fraction_dirty_rate()
#End If
        Return Me
    End Function

    Public Function divide(ByVal that As big_udec, ByRef divide_by_zero As Boolean) As big_udec
        If that Is Nothing OrElse that.is_zero() Then
            divide_by_zero = True
            Return Me
        End If
        divide_by_zero = False
        If is_zero() Then
            Return Me
        End If

        Dim n1 As big_uint = Nothing
        Dim n2 As big_uint = Nothing
        Dim d1 As big_uint = Nothing
        Dim d2 As big_uint = Nothing
        n1 = Me.n.CloneT()
        n2 = that.n.CloneT()
        d1 = Me.d.CloneT()
        d2 = that.d.CloneT()
#If REDUCE_FRACTION_WHEN_CALCULATING Then
        reduce_fraction(n1, n2)
        reduce_fraction(d1, d2)
#Else
        fast_reduce_fraction(n1, n2)
        fast_reduce_fraction(d1, d2)
#End If
        replace_by(n1.multiply(d2), d1.multiply(n2))

#If Not REDUCE_FRACTION_WHEN_CALCULATING Then
        increase_fraction_dirty_rate()
#End If
        Return Me
    End Function

    Public Function divide(ByVal that As big_udec) As big_udec
        Dim divide_by_zero As Boolean = False
        Dim r As big_udec = Nothing
        r = divide(that, divide_by_zero)
        If divide_by_zero Then
            throws.divide_by_zero()
        End If
        Return r
    End Function

    Public Function assert_divide(ByVal that As big_udec) As big_udec
        Dim divide_by_zero As Boolean = False
        Dim r As big_udec = Nothing
        r = divide(that, divide_by_zero)
        assert(Not divide_by_zero)
        Return r
    End Function

    Public Function power(ByVal that As big_uint) As big_udec
        If that Is Nothing OrElse that.is_zero() Then
            set_zero()
            Return Me
        End If
        If that.is_one() Then
            Return Me
        End If

        replace_by(Me.n ^ that, Me.d ^ that)
        Return Me
    End Function

    Public Function extract(ByVal that As big_uint, ByRef divide_by_zero As Boolean) As big_udec
        Return extract(that, constants.extract_power_base, divide_by_zero)
    End Function

    Public Function extract(ByVal that As big_uint,
                            ByVal extract_power_base As UInt32,
                            ByRef divide_by_zero As Boolean) As big_udec
        assert(extract_power_base > 0)
        If that Is Nothing OrElse that.is_zero() Then
            divide_by_zero = True
            set_zero()
            Return Me
        End If
        divide_by_zero = False
        If is_zero() OrElse is_one() OrElse that.is_one() Then
            Return Me
        End If
        Dim p As big_uint = Nothing
        p = Me.n * extract_power_base \ Me.d \ that
        If p.is_zero_or_one() Then
            p = New big_uint(CUInt(2))
        End If
        replace_by(((Me.n ^ (that * p + uint32_1)) * (Me.d ^ (that * p - uint32_1))).assert_extract(that),
                   (Me.n * Me.d) ^ p)
        Return Me
    End Function

    Public Function extract(ByVal that As big_uint) As big_udec
        Return extract(that, constants.extract_power_base)
    End Function

    Public Function extract(ByVal that As big_uint, ByVal extract_power_base As UInt32) As big_udec
        Dim divide_by_zero As Boolean = False
        Dim r As big_udec = Nothing
        r = extract(that, extract_power_base, divide_by_zero)
        If divide_by_zero Then
            throws.divide_by_zero()
        End If
        Return r
    End Function

    Public Function assert_extract(ByVal that As big_uint) As big_udec
        Return assert_extract(that, constants.extract_power_base)
    End Function

    Public Function assert_extract(ByVal that As big_uint, ByVal extract_power_base As UInt32) As big_udec
        Dim divide_by_zero As Boolean = False
        Dim r As big_udec = Nothing
        r = extract(that, extract_power_base, divide_by_zero)
        assert(Not divide_by_zero)
        Return r
    End Function

    Public Function power_2() As big_udec
        Return multiply(Me)
    End Function

    Public Function power(ByVal that As big_udec) As big_udec
        If that Is Nothing OrElse that.is_zero() Then
            set_one()
            Return Me
        End If

        Return power(that.n).assert_extract(that.d)
    End Function

    Public Function extract(ByVal that As big_udec, ByRef divide_by_zero As Boolean) As big_udec
        divide_by_zero = False
        If that Is Nothing OrElse that.is_zero() Then
            divide_by_zero = True
            Return Me
        End If

        Return power(that.d).assert_extract(that.n)
    End Function

    Public Function extract(ByVal that As big_udec) As big_udec
        Dim divide_by_zero As Boolean = False
        Dim r As big_udec = Nothing
        r = extract(that, divide_by_zero)
        If divide_by_zero Then
            throws.divide_by_zero()
        End If
        Return r
    End Function

    Public Function assert_extract(ByVal that As big_udec) As big_udec
        Dim divide_by_zero As Boolean = False
        Dim r As big_udec = Nothing
        r = extract(that, divide_by_zero)
        assert(Not divide_by_zero)
        Return r
    End Function

    Public Function reciprocal(ByRef divide_by_zero As Boolean) As big_udec
        If is_zero() Then
            divide_by_zero = True
            Return CloneT()
        End If
        divide_by_zero = False
        Return New big_udec(d.CloneT(), n.CloneT())
    End Function

    Public Function reciprocal() As big_udec
        Dim d As Boolean = False
        Dim r As big_udec = Nothing
        r = reciprocal(d)
        If d Then
            throws.divide_by_zero()
        End If
        Return r
    End Function

    Public Function assert_reciprocal() As big_udec
        Dim d As Boolean = False
        Dim r As big_udec = Nothing
        r = reciprocal(d)
        assert(Not d)
        Return r
    End Function
End Class
