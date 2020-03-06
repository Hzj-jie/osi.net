﻿
Option Explicit On
Option Infer Off
Option Strict On

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
        Dim n As big_uint = Nothing
        n = Me.n * that.d + Me.d * that.n
        Dim d As big_uint = Nothing
        d = Me.d * that.d
        replace_by(n, d)
        reduce_fraction()
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
        reduce_fraction()
    End Sub

    Public Function [sub](ByVal that As big_udec, ByRef overflow As Boolean) As big_udec
        If that Is Nothing OrElse that.is_zero() Then
            Return Me
        End If
        If is_zero() Then
            [sub](big_uint.zero(), that.n, that.d, overflow)
            Return Me
        End If

        [sub](Me.n * that.d, that.n * Me.d, Me.d * that.d, overflow)
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
        If that Is Nothing Then
            Return Me
        End If
        If is_zero() Then
            Return Me
        End If
        If that.is_zero() Then
            set_zero()
            Return Me
        End If

        Dim n As big_uint = Nothing
        Dim d As big_uint = Nothing
        n = Me.n * that.n
        d = Me.d * that.d
        assert(replace_by(n, d))
        reduce_fraction()
        Return Me
    End Function

    Public Function divide(ByVal that As big_udec, ByRef divide_by_zero As Boolean) As big_udec
        divide_by_zero = False
        If that Is Nothing OrElse that.is_zero() Then
            divide_by_zero = True
            Return Me
        End If
        If is_zero() Then
            Return Me
        End If

        assert(replace_by(Me.n * that.d, Me.d * that.n))
        reduce_fraction()
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

        Dim n As big_uint = Nothing
        Dim d As big_uint = Nothing
        n = Me.n ^ that
        d = Me.d ^ that
        replace_by(n, d)
        Return Me
    End Function

    Public Function extract(ByVal that As big_uint, ByRef overflow As Boolean) As big_udec
        Return extract(that, constants.extract_power_base, overflow)
    End Function

    Public Function extract(ByVal that As big_uint,
                            ByVal extract_power_base As UInt32,
                            ByRef overflow As Boolean) As big_udec
        assert(extract_power_base > 0)
        If that Is Nothing OrElse that.is_zero() Then
            overflow = True
            set_zero()
            Return Me
        End If
        overflow = False
        If that.is_one() Then
            Return Me
        End If
        Dim p As big_uint = Nothing
        p = Me.n * that * extract_power_base \ Me.d
        If p.is_zero_or_one() Then
            p = New big_uint(CUInt(2))
        End If
        Dim n As big_uint = Nothing
        n = ((Me.n ^ (that * p + uint32_1)) * (Me.d ^ (that * p - uint32_1))).assert_extract(that)
        Dim d As big_uint = Nothing
        d = ((Me.n * Me.d) ^ p)
        replace_by(n, d)
        reduce_fraction()
        Return Me
    End Function

    Public Function extract(ByVal that As big_uint) As big_udec
        Return extract(that, constants.extract_power_base)
    End Function

    Public Function extract(ByVal that As big_uint, ByVal extract_power_base As UInt32) As big_udec
        Dim overflow As Boolean = False
        Dim r As big_udec = Nothing
        r = extract(that, extract_power_base, overflow)
        If overflow Then
            throws.overflow()
        End If
        Return r
    End Function

    Public Function assert_extract(ByVal that As big_uint) As big_udec
        Return assert_extract(that, constants.extract_power_base)
    End Function

    Public Function assert_extract(ByVal that As big_uint, ByVal extract_power_base As UInt32) As big_udec
        Dim overflow As Boolean = False
        Dim r As big_udec = Nothing
        r = extract(that, extract_power_base, overflow)
        assert(Not overflow)
        Return r
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
End Class