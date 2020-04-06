
Option Explicit On
Option Infer Off
Option Strict On

#Const GCD_USE_SUCCESSIVE_DIVISION = False
#Const USE_MODULUS_BIT = False
#Const USE_DIVIDE_BIT = False

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class big_uint
    Public Function add(ByVal that As big_uint) As big_uint
        If that Is Nothing OrElse that.is_zero() Then
            Return Me
        End If
        If is_zero() Then
            assert(replace_by(that))
            Return Me
        End If
        assert(v.size() > 0 AndAlso that.v.size() > 0)
        Dim i As UInt32 = 0
        Dim c As UInt32 = 0
        For i = 0 To min(v.size(), that.v.size()) - uint32_1
            c = add(that.v.get(i), c, i)
#If DEBUG Then
            assert(c = 0 OrElse c = 1)
#End If
        Next

        If v.size() < that.v.size() Then
            v.resize(that.v.size())
            memcpy(v.data(), i, that.v.data(), i, that.v.size() - i)
        End If
        recursive_add(c, i)
        Return Me
    End Function

    Public Function [sub](ByVal that As big_uint) As big_uint
        Dim o As Boolean = False
        [sub](that, o)
        If o Then
            Throw overflow()
        End If
        Return Me
    End Function

    Public Function assert_sub(ByVal that As big_uint) As big_uint
        Dim o As Boolean = False
        [sub](that, o)
        assert(Not o)
        Return Me
    End Function

    Public Function [sub](ByVal that As big_uint, ByRef overflow As Boolean) As big_uint
        If that Is Nothing OrElse that.is_zero() Then
            overflow = False
            Return Me
        End If
        If that.v.size() > v.size() Then
            v.resize(that.v.size())
        End If
        assert(v.size() > 0 AndAlso that.v.size() > 0)
        Dim i As UInt32 = 0
        Dim c As UInt32 = 0
        For i = 0 To that.v.size() - uint32_1
            c = [sub](that.v.get(i), c, i)
        Next
        overflow = recursive_sub(c, i)
        remove_extra_blank()
        Return Me
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function multiply(ByVal that As big_uint) As big_uint
        multiply(move(Me), that)
        Return Me
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function multiply(ByVal that As UInt32) As big_uint
        If that = 0 Then
            set_zero()
            Return Me
        End If
        If that = 1 OrElse is_zero() Then
            Return Me
        End If
        If is_one() Then
            replace_by(that)
        End If
        Dim c As UInt32 = 0
        For i As UInt32 = 0 To v.size() - uint32_1
            Dim t As UInt64 = 0
            t = v.get(i)
            t *= that
            t += c
            v.set(i, CUInt(t And max_uint32))
            c = CUInt(t >> bit_count_in_uint32)
        Next
        If c <> 0 Then
            v.push_back(c)
        End If
        remove_last_blank()
        Return Me
    End Function

    Public Function divide(ByVal that As big_uint,
                           ByRef divide_by_zero As Boolean,
                           Optional ByRef remainder As big_uint = Nothing) As big_uint
        If that Is Nothing OrElse that.is_zero() Then
            divide_by_zero = True
            Return Me
        End If
        divide_by_zero = False
        If is_zero() OrElse that.is_one() Then
            remainder = big_uint.zero()
            Return Me
        End If
        If is_one() Then
            remainder = big_uint.one()
            set_zero()
            Return Me
        End If
        If that.power_of_2() Then
            Dim l As UInt64 = 0
            l = that.bit_count() - uint64_1
            remainder = Me.CloneT().[and](that - uint32_1)
            right_shift(l)
            Return Me
        End If
        If that.fit_uint32() Then
            Dim r As UInt32 = 0
            divide(that.as_uint32(), r, divide_by_zero)
            assert(Not divide_by_zero)
            remainder = New big_uint(r)
            Return Me
        End If
        assert(Not that.is_zero_or_one())
        remainder = move(Me)
        set_zero()
        If remainder.uint32_size() < that.uint32_size() Then
            Return Me
        End If

#If DEBUG Then
        assert(remainder.bit_count() >= that.bit_count())
#End If

        'make sure the that will not be impacted during the calculation
#If DEBUG Then
        Dim original_that As big_uint = Nothing
        original_that = that
        that = that.CloneT()
#End If

#If USE_DIVIDE_BIT Then
        divide_bit(that, remainder)
#Else
        divide_uint(that, remainder)
#End If
        remove_last_blank()

#If DEBUG Then
        assert(remainder.less(original_that))
        assert(that.equal(original_that))
#End If
        Return Me
    End Function

    Public Function divide(ByVal that As big_uint, Optional ByRef remainder As big_uint = Nothing) As big_uint
        Dim r As Boolean = False
        divide(that, r, remainder)
        If r Then
            Throw divide_by_zero()
        End If
        Return Me
    End Function

    Public Function assert_divide(ByVal that As big_uint, Optional ByRef remainder As big_uint = Nothing) As big_uint
        Dim r As Boolean = False
        divide(that, r, remainder)
        assert(Not r)
        Return Me
    End Function

    Public Function divide(ByVal that As UInt32,
                           ByRef divide_by_zero As Boolean,
                           Optional ByRef remainder As UInt32 = 0) As big_uint
        divide(that, remainder, divide_by_zero)
        Return Me
    End Function

    Public Function divide(ByVal that As UInt32,
                           Optional ByRef remainder As UInt32 = 0) As big_uint
        Dim r As Boolean = False
        divide(that, remainder, r)
        If r Then
            Throw divide_by_zero()
        End If
        Return Me
    End Function

    Public Function assert_divide(ByVal that As UInt32,
                                  Optional ByRef remainder As UInt32 = 0) As big_uint
        Dim r As Boolean = False
        divide(that, remainder, r)
        assert(Not r)
        Return Me
    End Function

    Public Function divide(ByVal that As UInt16,
                           ByRef divide_by_zero As Boolean,
                           Optional ByRef remainder As UInt32 = 0) As big_uint
        Return divide(CUInt(that), divide_by_zero, remainder)
    End Function

    Public Function divide(ByVal that As UInt16,
                           Optional ByRef remainder As UInt32 = 0) As big_uint
        Return divide(CUInt(that), remainder)
    End Function

    Public Function assert_divide(ByVal that As UInt16,
                                  Optional ByRef remainder As UInt32 = 0) As big_uint
        Return assert_divide(CUInt(that), remainder)
    End Function

    Public Function divide(ByVal that As Byte,
                           ByRef divide_by_zero As Boolean,
                           Optional ByRef remainder As UInt32 = 0) As big_uint
        Return divide(CUInt(that), divide_by_zero, remainder)
    End Function

    Public Function divide(ByVal that As Byte,
                           Optional ByRef remainder As UInt32 = 0) As big_uint
        Return divide(CUInt(that), remainder)
    End Function

    Public Function assert_divide(ByVal that As Byte,
                                  Optional ByRef remainder As UInt32 = 0) As big_uint
        Return assert_divide(CUInt(that), remainder)
    End Function

    Public Function modulus(ByVal that As UInt32, ByRef divide_by_zero As Boolean) As big_uint
        If that = 0 Then
            divide_by_zero = True
            Return Me
        End If
        divide_by_zero = False
        If is_zero() Then
            Return Me
        End If
        If that = 1 Then
            set_zero()
            Return Me
        End If
        If that = 2 Then
            If even() Then
                set_zero()
            Else
                set_one()
            End If
            Return Me
        End If
        If is_one() Then
            Return Me
        End If
        If fit_uint32() Then
            If as_uint32() < that Then
                Return Me
            End If
            If as_uint32() = that Then
                set_zero()
                Return Me
            End If
        End If
        Dim r As UInt32 = 0
        Dim i As UInt32 = 0
        i = uint32_size() - uint32_1
        While True
            If r > 0 OrElse v.get(i) > 0 Then
                Dim t As UInt64 = 0
                t = r
                t <<= bit_count_in_uint32
                t = t Or v.get(i)
                r = CUInt(t Mod that)
            End If
            If i = 0 Then
                Exit While
            End If
            i -= uint32_1
        End While
        replace_by(r)
        Return Me
    End Function

    Public Function modulus(ByVal that As UInt32) As big_uint
        Dim r As Boolean = False
        modulus(that, r)
        If r Then
            Throw divide_by_zero()
        End If
        Return Me
    End Function

    Public Function assert_modulus(ByVal that As UInt32) As big_uint
        Dim r As Boolean = False
        modulus(that, r)
        assert(Not r)
        Return Me
    End Function

    Public Function modulus(ByVal that As big_uint, ByRef divide_by_zero As Boolean) As big_uint
        If that Is Nothing OrElse that.is_zero() Then
            divide_by_zero = True
            Return Me
        End If
        divide_by_zero = False
        If that.power_of_2() Then
            [and](that - uint32_1)
            Return Me
        End If
        If that.fit_uint32() Then
            modulus(that.as_uint32(), divide_by_zero)
            assert(Not divide_by_zero)
            Return Me
        End If
        If is_zero_or_one() Then
            Return Me
        End If
        If less(that) Then
            Return Me
        End If

#If USE_MODULUS_BIT Then
        modulus_bit(that)
#Else
        modulus_uint(that)
#End If

        Return Me
    End Function

    Public Function modulus(ByVal that As big_uint) As big_uint
        Dim r As Boolean = False
        modulus(that, r)
        If r Then
            Throw divide_by_zero()
        End If
        Return Me
    End Function

    Public Function assert_modulus(ByVal that As big_uint) As big_uint
        Dim r As Boolean = False
        modulus(that, r)
        assert(Not r)
        Return Me
    End Function

    Public Function power_2() As big_uint
        Dim s As big_uint = Nothing
        s = move(Me)
        multiply(s, s)
        Return Me
    End Function

    Public Function power(ByVal that As big_uint) As big_uint
        If that Is Nothing Then
            Return Me
        End If
        If that.is_zero() Then
            '0 ^ 0 = 1
            set_one()
            Return Me
        End If
        If is_zero_or_one() OrElse that.is_one() Then
            Return Me
        End If
        Dim c As big_uint = Nothing
        If that.getrbit(0) Then
            c = New big_uint(Me)
        Else
            c = move(Me)
            set_one()
        End If
        assert(that.bit_count() > 0)
        Dim last As UInt64 = 0
        For i As UInt64 = 1 To that.bit_count() - uint64_1
            If that.getrbit(i) Then
                While last < i
                    c.power_2()
                    last += uint64_1
                End While
                multiply(c)
            End If
        Next
        Return Me
    End Function

    Public Function extract(ByVal that As big_uint,
                            ByRef divide_by_zero As Boolean,
                            Optional ByRef remainder As big_uint = Nothing) As big_uint
        extract(that, remainder, divide_by_zero)
        Return Me
    End Function

    Public Function extract(ByVal that As big_uint, Optional ByRef remainder As big_uint = Nothing) As big_uint
        Dim r As Boolean = False
        extract(that, remainder, r)
        If r Then
            Throw divide_by_zero()
        End If
        Return Me
    End Function

    Public Function assert_extract(ByVal that As big_uint, Optional ByRef remainder As big_uint = Nothing) As big_uint
        Dim r As Boolean = False
        extract(that, remainder, r)
        assert(Not r)
        Return Me
    End Function

    Public Function factorial() As big_uint
        If is_zero_or_one() Then
            set_one()
        Else
            Dim i As big_uint = Nothing
            i = move(Me)
            set_one()
            While Not i.is_one()
                Me.multiply(i)
                i.assert_sub(uint32_1)
            End While
        End If
        Return Me
    End Function

    Public Shared Function gcd(ByVal a As big_uint, ByVal b As big_uint) As big_uint
        assert(Not a Is Nothing)
        assert(Not b Is Nothing)
        If a.is_zero() OrElse b.is_zero() Then
            Return zero()
        End If
        If a.is_one() OrElse b.is_one() Then
            Return one()
        End If
        If a.equal(b) Then
            Return a.CloneT()
        End If
#If GCD_USE_SUCCESSIVE_DIVISION Then
        Dim c As big_uint = Nothing
        If a.less(b) Then
            c = a
            a = b
            b = c
        End If
        c = a.CloneT().assert_modulus(b)
        While Not c.is_zero()
            a = b.CloneT()
            b = c
            c = a.CloneT().assert_modulus(b)
        End While
        Return b
#Else
        a = a.CloneT()
        b = b.CloneT()
        Dim shift As UInt32 = 0
        Dim az As UInt32 = 0
        Dim bz As UInt32 = 0
        az = a.binary_trailing_zero_count()
        bz = b.binary_trailing_zero_count()
        a.right_shift(az)
        b.right_shift(bz)
        shift = min(az, bz)

        While True
            assert(Not a.is_zero())
            assert(Not b.is_zero())
            a.remove_binary_trailing_zeros()
            b.remove_binary_trailing_zeros()

            Dim cmp As Int32 = 0
            cmp = a.compare(b)
            If cmp = 0 Then
                Return a.left_shift(shift)
            End If
            If cmp < 0 Then
                b.assert_sub(a)
            Else
                assert(cmp > 0)
                a.assert_sub(b)
            End If
        End While
        assert(False)
        Return Nothing
#End If
    End Function
End Class
