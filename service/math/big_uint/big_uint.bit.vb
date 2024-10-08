
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class big_uint
    <MethodImpl(math_debug.aggressive_inlining)>
    Private Sub set_bit_count(ByVal s As UInt64)
        set_zero()
        Dim l As UInt32 = 0
#If DEBUG Then
        l = assert_which.of(s >> bit_count_in_uint32_shift).can_cast_to_uint32()
#Else
        l = CUInt(s >> bit_count_in_uint32_shift)
#End If
        If (s And bit_count_in_uint32_mask) > 0 Then
            l += uint32_1
        End If
        v.resize(l)
    End Sub

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Sub bit_pos(ByVal pos As UInt64, ByRef v_index As UInt32, ByRef b_index As Byte)
        bit_rpos(pos, v_index, b_index)
        v_index = v.size() - uint32_1 - v_index
    End Sub

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Sub bit_rpos(ByVal pos As UInt64,
                         ByRef v_index As UInt32,
                         ByRef b_rindex As Byte)
        bit_rpos(pos, v_index, b_rindex, Nothing, False, True)
    End Sub

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Sub bit_rpos(ByVal pos As UInt64,
                         ByRef v_index As UInt32,
                         ByRef b_rindex As Byte,
                         ByRef overflow As Boolean,
                         ByVal fill As Boolean,
                         ByVal require_assert As Boolean)
        overflow = (pos >= bit_count())
        If require_assert Then
            assert(Not overflow)
        End If
#If DEBUG Then
        v_index = assert_which.of(pos >> bit_count_in_uint32_shift).can_cast_to_uint32()
        b_rindex = assert_which.of(pos And bit_count_in_uint32_mask).can_cast_to_byte()
#Else
        v_index = CUInt(pos >> bit_count_in_uint32_shift)
        b_rindex = CByte(pos And bit_count_in_uint32_mask)
#End If
        If fill AndAlso overflow AndAlso v_index >= v.size() Then
            v.resize(v_index + uint32_1)
        End If
    End Sub

    'the bitwise operators are following the rules
    '1. if that is longer than me, ignore the overlength bits
    '2. if that is shorter than me, treat all the bits left as 0
    Private Function bit_wise_operation(ByVal that As big_uint, ByVal op As bit_wise_operator) As big_uint
        If that Is Nothing Then
            Return Me
        End If
        For i As UInt32 = 0 To v.size() - uint32_1
            Dim t As UInt32 = 0
            If i < that.v.size() Then
                t = that.v.get(i)
            Else
                t = 0
            End If
            Select Case op
                Case bit_wise_operator.and
                    v.set(i, v.get(i) And t)
                Case bit_wise_operator.or
                    v.set(i, v.get(i) Or t)
                Case bit_wise_operator.xor
                    v.set(i, v.get(i) Xor t)
                Case Else
                    assert(False)
            End Select
        Next
        If op = bit_wise_operator.and OrElse
           op = bit_wise_operator.xor Then
            remove_extra_blank()
        End If
        Return Me
    End Function

    Public Function [not]() As big_uint
        If Not is_zero() Then
            assert(v.size() > 0)
            For i As UInt32 = 0 To v.size() - uint32_1
                v.set(i, Not v.get(i))
            Next
        End If
        Return Me
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function bit_count() As UInt64
        If is_zero() Then
            Return uint64_0
        End If
        If is_one() Then
            Return uint64_1
        End If
        Return ((v.size() - uint64_1) << bit_count_in_uint32_shift) + v.back().bit_count()
    End Function

    Public Function [and](ByVal that As big_uint) As big_uint
        Return bit_wise_operation(that, bit_wise_operator.and)
    End Function

    Public Function [or](ByVal that As big_uint) As big_uint
        Return bit_wise_operation(that, bit_wise_operator.or)
    End Function

    Public Function [xor](ByVal that As big_uint) As big_uint
        Return bit_wise_operation(that, bit_wise_operator.xor)
    End Function

    Public Function _1count() As UInt64
        If is_zero() Then
            Return 0
        End If
        Dim r As UInt64 = 0
        For i As UInt32 = 0 To v.size() - uint32_1
            r += v.get(i)._1count()
        Next
        Return r
    End Function

    Public Function onecount() As UInt64
        Return _1count()
    End Function

    Public Function power_of_2() As Boolean
        If is_zero() Then
            Return False
        End If
        Dim r As UInt64 = 0
        Dim i As UInt32 = 0
        i = v.size() - uint32_1
        While True
            r += v.get(i)._1count()
            If r > 1 Then
                Return False
            End If
            If i = 0 Then
                Exit While
            End If
            i -= uint32_1
        End While
        assert(r = 1)
        Return True
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Sub setbit(ByVal pos As UInt64, Optional ByVal value As Boolean = True)
        Dim vi As UInt32 = 0
        Dim bi As Byte = 0
        bit_pos(pos, vi, bi)
        v.set(vi, v.get(vi).setbit(bi, value))
    End Sub

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function getbit(ByVal pos As UInt64) As Boolean
        Dim vi As UInt32 = 0
        Dim bi As Byte = 0
        bit_pos(pos, vi, bi)
        Return v.get(vi).getbit(bi)
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Sub setrbit(ByVal pos As UInt64, Optional ByVal value As Boolean = True)
        Dim vi As UInt32 = 0
        Dim bi As Byte = 0
        Dim ov As Boolean = False
        bit_rpos(pos, vi, bi, ov, value, False)
        If Not ov OrElse value Then
            v.set(vi, v.get(vi).setrbit(bi, value))
        End If
    End Sub

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function getrbit(ByVal pos As UInt64) As Boolean
        Dim vi As UInt32 = 0
        Dim bi As Byte = 0
        bit_rpos(pos, vi, bi)
        Return v.get(vi).getrbit(bi)
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function even() As Boolean
        Return Not odd()
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function odd() As Boolean
        '0 is even
        Return bit_count() > 0 AndAlso getrbit(0)
    End Function

    Public Function trailing_binary_zero_count() As UInt32
        assert(Not is_zero())
        Dim i As UInt32 = 0
        While i < uint32_size()
            If v.get(i) <> 0 Then
                Return (i << bit_count_in_uint32_shift) + v.get(i).trailing_binary_zero_count()
            End If
            i += uint32_1
        End While
        assert(False)
        Return uint32_0
    End Function

    Public Function trailing_uint32_zero_count() As UInt32
        Dim i As UInt32 = 0
        While i < uint32_size()
            If v.get(i) <> 0 Then
                Return i
            End If
            i += uint32_1
        End While
        assert(False)
        Return uint32_0
    End Function

    Public Function remove_trailing_binary_zeros() As UInt32
        Dim r As UInt32 = 0
        r = trailing_binary_zero_count()
        assert_right_shift(r)
        Return r
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function lowest_uint32() As UInt32
        Return get_uint32(0)
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function highest_uint32() As UInt32
        Return get_ruint32(0)
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function second_highest_uint32() As UInt32
        Return get_ruint32(1)
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function highest_uint64() As UInt64
        Return (CULng(highest_uint32()) << bit_count_in_uint32) Or second_highest_uint32()
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function get_uint32(ByVal i As UInt32) As UInt32
        assert(i < uint32_size())
        Return v.get(i)
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function get_ruint32(ByVal i As UInt32) As UInt32
        assert(i < uint32_size())
        Return v.get(uint32_size() - i - uint32_1)
    End Function
End Class
