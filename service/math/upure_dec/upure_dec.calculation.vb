
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class upure_dec
    Public Function add(ByVal that As upure_dec, ByRef overflow As Boolean) As upure_dec
        overflow = False
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
        If n >= d Then
            n -= d
            overflow = True
        End If
        replace_by(n, d)
        reduct_of_fraction()
        Return Me
    End Function

    Public Function add(ByVal that As upure_dec) As upure_dec
        Dim overflow As Boolean = False
        Dim r As upure_dec = Nothing
        r = add(that, overflow)
        If overflow Then
            throws.overflow()
        End If
        Return r
    End Function

    Public Function assert_add(ByVal that As upure_dec) As upure_dec
        Dim overflow As Boolean = False
        Dim r As upure_dec = Nothing
        r = add(that, overflow)
        assert(Not overflow)
        Return r
    End Function

    Public Function [sub](ByVal that As upure_dec, ByRef overflow As Boolean) As upure_dec
        If that Is Nothing OrElse that.is_zero() Then
            Return Me
        End If
        If is_zero() Then
            overflow = True
            replace_by(that.d - that.n, that.d)
            Return Me
        End If

        Dim d As big_uint = Nothing
        d = Me.d * that.d
        Dim n As big_uint = Nothing
        If less(that) Then
            overflow = True
            n = d - Me.n * that.d + Me.d * that.n
        Else
            overflow = False
            n = Me.n * that.d - Me.d * that.n
        End If
        assert(Not n Is Nothing)
        replace_by(n, d)
        reduct_of_fraction()
        Return Me
    End Function

    Public Function [sub](ByVal that As upure_dec) As upure_dec
        Dim overflow As Boolean = False
        Dim r As upure_dec = Nothing
        r = [sub](that, overflow)
        If overflow Then
            throws.overflow()
        End If
        Return r
    End Function

    Public Function assert_sub(ByVal that As upure_dec) As upure_dec
        Dim overflow As Boolean = False
        Dim r As upure_dec = Nothing
        r = [sub](that, overflow)
        assert(Not overflow)
        Return r
    End Function

    Public Function multiply(ByVal that As upure_dec) As upure_dec
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
        replace_by(n, d)
        reduct_of_fraction()
        Return Me
    End Function

    Public Function divide(ByVal that As upure_dec,
                           ByRef divide_by_zero As Boolean,
                           ByRef overflow As Boolean) As upure_dec
        divide_by_zero = False
        overflow = False
        If that Is Nothing OrElse that.is_zero() Then
            divide_by_zero = True
            Return Me
        End If
        If is_zero() Then
            Return Me
        End If

        Dim n As big_uint = Nothing
        Dim d As big_uint = Nothing
        n = Me.n * that.d
        d = Me.d * that.n
        If n >= d Then
            n -= d
            overflow = True
        End If
        replace_by(n, d)
        reduct_of_fraction()
        Return Me
    End Function

    Public Function divide(ByVal that As upure_dec) As upure_dec
        Dim divide_by_zero As Boolean = False
        Dim overflow As Boolean = False
        Dim r As upure_dec = Nothing
        r = divide(that, divide_by_zero, overflow)
        If divide_by_zero Then
            throws.divide_by_zero()
        End If
        If overflow Then
            throws.overflow()
        End If
        Return r
    End Function

    Public Function assert_divide(ByVal that As upure_dec) As upure_dec
        Dim divide_by_zero As Boolean = False
        Dim overflow As Boolean = False
        Dim r As upure_dec = Nothing
        r = divide(that, divide_by_zero, overflow)
        assert(Not divide_by_zero)
        assert(Not overflow)
        Return r
    End Function

    Public Function power(ByVal that As big_uint) As upure_dec
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

    Public Function extract(ByVal that As big_uint, ByRef overflow As Boolean) As upure_dec
        If that Is Nothing OrElse that.is_zero() Then
            overflow = True
            set_zero()
            Return Me
        End If
        overflow = False
        If that.is_one() Then
            Return Me
        End If
        Dim n As big_uint = Nothing
        n = ((Me.n ^ (that + uint32_1)) * (Me.d ^ (that - uint32_1))).assert_extract(that)
        Dim d As big_uint = Nothing
        d = Me.n * Me.d
        replace_by(n, d)
        reduct_of_fraction()
        Return Me
    End Function

    Public Function extract(ByVal that As big_uint) As upure_dec
        Dim overflow As Boolean = False
        Dim r As upure_dec = Nothing
        r = extract(that, overflow)
        If overflow Then
            throws.overflow()
        End If
        Return r
    End Function

    Public Function assert_extract(ByVal that As big_uint) As upure_dec
        Dim overflow As Boolean = False
        Dim r As upure_dec = Nothing
        r = extract(that, overflow)
        assert(Not overflow)
        Return r
    End Function

    Public Function power(ByVal that As upure_dec, ByRef overflow As Boolean) As upure_dec
        overflow = False
        If that Is Nothing OrElse that.is_zero() Then
            overflow = True
            set_zero()
            Return Me
        End If

        Return power(that.n).assert_extract(that.d)
    End Function

    Public Function extract(ByVal that As upure_dec, ByRef divide_by_zero As Boolean) As upure_dec
        divide_by_zero = False
        If that Is Nothing OrElse that.is_zero() Then
            divide_by_zero = True
            Return Me
        End If

        Return power(that.d).assert_extract(that.n)
    End Function
End Class
