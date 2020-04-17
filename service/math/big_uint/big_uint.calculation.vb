
Option Explicit On
Option Infer Off
Option Strict On

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
            arrays.copy(v.data(), i, that.v.data(), i, that.v.size() - i)
        End If
        recursive_add(c, i)
        Return Me
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function [sub](ByVal that As big_uint) As big_uint
        Dim o As Boolean = False
        [sub](that, o)
        If o Then
            Throw overflow()
        End If
        Return Me
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Function assert_sub(ByVal that As big_uint, ByVal offset As UInt32) As big_uint
        Dim o As Boolean = False
        [sub](that, offset, o)
        assert(Not o)
        Return Me
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function assert_sub(ByVal that As big_uint) As big_uint
        Dim o As Boolean = False
        [sub](that, o)
        assert(Not o)
        Return Me
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Function [sub](ByVal that As big_uint, ByVal offset As UInt32, ByRef overflow As Boolean) As big_uint
        If that Is Nothing OrElse that.is_zero() Then
            overflow = False
            Return Me
        End If
        If that.v.size() + offset > v.size() Then
            v.resize(that.v.size() + offset)
        End If
        assert(v.size() > 0 AndAlso that.v.size() > 0)
        Dim i As UInt32 = 0
        Dim c As UInt32 = 0
        For i = 0 To that.v.size() - uint32_1
            c = [sub](that.v.get(i), c, i + offset)
        Next
        overflow = recursive_sub(c, i + offset)
        remove_extra_blank()
        Return Me
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function [sub](ByVal that As big_uint, ByRef overflow As Boolean) As big_uint
        Return [sub](that, 0, overflow)
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function multiply(ByVal that As big_uint) As big_uint
        multiply(move(Me), that)
        Return Me
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
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

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function divide(ByVal that As big_uint, Optional ByRef remainder As big_uint = Nothing) As big_uint
        Dim r As Boolean = False
        divide(that, r, remainder)
        If r Then
            Throw divide_by_zero()
        End If
        Return Me
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function assert_divide(ByVal that As big_uint, Optional ByRef remainder As big_uint = Nothing) As big_uint
        Dim r As Boolean = False
        divide(that, r, remainder)
        assert(Not r)
        Return Me
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function divide(ByVal that As UInt32,
                           Optional ByRef remainder As UInt32 = 0) As big_uint
        Dim r As Boolean = False
        divide(that, r, remainder)
        If r Then
            Throw divide_by_zero()
        End If
        Return Me
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function assert_divide(ByVal that As UInt32,
                                  Optional ByRef remainder As UInt32 = 0) As big_uint
        Dim r As Boolean = False
        divide(that, r, remainder)
        assert(Not r)
        Return Me
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function divide(ByVal that As UInt16,
                           ByRef divide_by_zero As Boolean,
                           Optional ByRef remainder As UInt32 = 0) As big_uint
        Return divide(CUInt(that), divide_by_zero, remainder)
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function divide(ByVal that As UInt16,
                           Optional ByRef remainder As UInt32 = 0) As big_uint
        Return divide(CUInt(that), remainder)
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function assert_divide(ByVal that As UInt16,
                                  Optional ByRef remainder As UInt32 = 0) As big_uint
        Return assert_divide(CUInt(that), remainder)
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function divide(ByVal that As Byte,
                           ByRef divide_by_zero As Boolean,
                           Optional ByRef remainder As UInt32 = 0) As big_uint
        Return divide(CUInt(that), divide_by_zero, remainder)
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function divide(ByVal that As Byte,
                           Optional ByRef remainder As UInt32 = 0) As big_uint
        Return divide(CUInt(that), remainder)
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
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

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function modulus(ByVal that As UInt32) As big_uint
        Dim r As Boolean = False
        modulus(that, r)
        If r Then
            Throw divide_by_zero()
        End If
        Return Me
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
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

#If DEBUG Then
        Dim original_that As big_uint = Nothing
        original_that = that.CloneT()
#End If

#If USE_MODULUS_BIT Then
        modulus_bit(that)
#Else
        modulus_uint(that)
#End If

#If DEBUG Then
        assert(that.equal(original_that))
#End If

        Return Me
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function modulus(ByVal that As big_uint) As big_uint
        Dim r As Boolean = False
        modulus(that, r)
        If r Then
            Throw divide_by_zero()
        End If
        Return Me
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function assert_modulus(ByVal that As big_uint) As big_uint
        Dim r As Boolean = False
        modulus(that, r)
        assert(Not r)
        Return Me
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function power_2() As big_uint
        Dim s As big_uint = Nothing
        s = move(Me)
        multiply(s, s)
        Return Me
    End Function

    Public Function power(ByVal that As big_uint) As big_uint
        If that Is Nothing OrElse that.is_zero() Then
            '0 ^ 0 = 1
            set_one()
            Return Me
        End If
        If is_zero_or_one() OrElse that.is_one() Then
            Return Me
        End If
        that = that.CloneT()
        For i As UInt32 = 1 To that.remove_trailing_binary_zeros()
            power_2()
        Next
        Dim c As big_uint = Nothing
        c = Me.CloneT()
        assert(that.bit_count() > 0)
        Dim last As UInt64 = 0
        For i As UInt64 = 1 To that.bit_count() - uint64_1
            If Not that.getrbit(i) Then
                Continue For
            End If
            While last < i
                c.power_2()
                last += uint64_1
            End While
            multiply(c)
        Next
        Return Me
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function extract(ByVal that As big_uint,
                            ByRef divide_by_zero As Boolean,
                            Optional ByRef remainder As big_uint = Nothing) As big_uint
        extract(that, remainder, divide_by_zero)
        Return Me
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function extract(ByVal that As big_uint, Optional ByRef remainder As big_uint = Nothing) As big_uint
        Dim r As Boolean = False
        extract(that, remainder, r)
        If r Then
            Throw divide_by_zero()
        End If
        Return Me
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
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
End Class
