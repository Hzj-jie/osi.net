
Option Explicit On
Option Infer Off
Option Strict On

#Const USE_MULTIPLY_BIT = False

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class big_uint
    'support move constructor
    <copy_constructor>
    Private Sub New(ByVal i As adaptive_array_uint32)
        Me.v = i
    End Sub

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Function set_and_borrow(ByVal t As Int64, ByVal p As UInt32) As UInt32
        v.set(p, CUInt(t And max_uint32))
        Return If(t < 0, uint32_1, uint32_0)
    End Function

#If DEBUG Then
    <MethodImpl(math_debug.aggressive_inlining)>
    Private Sub sub_assertions(ByVal c As UInt32, ByVal p As UInt32)
        'this assert is too costly
        assert(p < v.size())
        assert(c = uint32_0 OrElse c = uint32_1)
    End Sub
#End If

    'sub d at position p with carry-over as c
    <MethodImpl(math_debug.aggressive_inlining)>
    Private Function [sub](ByVal d As UInt32, ByVal c As UInt32, ByVal p As UInt32) As UInt32
#If DEBUG Then
        sub_assertions(c, p)
#End If
        Return set_and_borrow(CLng(v.get(p)) - d - c, p)
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Function recursive_sub(ByVal c As UInt32, ByVal p As UInt32) As Boolean
        If c = 0 Then
            Return False
        End If
        While p < v.size()
#If DEBUG Then
            sub_assertions(c, p)
#End If
            c = set_and_borrow(CLng(v.get(p)) - c, p)
            If c = 0 Then
                Exit While
            End If
            p += uint32_1
        End While
        Return (c = uint32_1)
    End Function

#If DEBUG Then
    <MethodImpl(math_debug.aggressive_inlining)>
    Private Sub add_assertions(ByVal p As UInt32)
        'this assert is too costly
        assert(p < v.size())
    End Sub
#End If

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Function set_and_carry(ByVal t As UInt64, ByVal p As UInt32) As UInt32
        v.set(p, CUInt(t And max_uint32))
        t >>= bit_count_in_uint32
#If DEBUG Then
        assert(t <= 2)
#End If
        Return CUInt(t)
    End Function

    'add d to the pos as p with carry-over as c
    <MethodImpl(math_debug.aggressive_inlining)>
    Private Function add(ByVal d As UInt32, ByVal c As UInt32, ByVal p As UInt32) As UInt32
#If DEBUG Then
        add_assertions(p)
#End If
        Return set_and_carry(CULng(v.get(p)) + c + d, p)
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Sub recursive_add(ByVal d As UInt32, ByVal p As UInt32)
        If d = 0 Then
            Return
        End If
        While p < v.size()
#If DEBUG Then
            add_assertions(p)
#End If
            d = set_and_carry(CULng(v.get(p)) + d, p)
            If d = 0 Then
                Return
            End If
            p += uint32_1
        End While
        v.push_back(d)
    End Sub

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Function remove_extra_blank() As UInt32
        If v.empty() Then
            Return uint32_0
        End If
        Dim i As UInt32 = v.size() - uint32_1
        While True
            If v.get(i) <> 0 Then
                If i = v.size() - uint32_1 Then
                    Return uint32_0
                End If
                Dim r As UInt32 = v.size()
                i += uint32_1
                v.resize(i)
                Return r - v.size()
            End If
            If i = 0 Then
                Dim r As UInt32 = v.size()
                v.clear()
                Return r
            End If
            i -= uint32_1
        End While
        assert(False)
        Return uint32_0
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Sub remove_last_blank()
        If v.empty() Then
            Return
        End If
        If v.back() = uint32_0 Then
            v.pop_back()
        End If
#If DEBUG Then
        If Not v.empty() Then
            assert(v.back() <> 0)
        End If
#End If
    End Sub

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Sub multiply_bit(ByVal this As big_uint, ByVal that As big_uint)
        this = this.CloneT()
        that = that.CloneT()
        While Not that.is_zero()
            Dim m As UInt32 = that.remove_trailing_binary_zeros()
            assert(that.odd())
            this.left_shift(m)
            add(this)
        End While
    End Sub

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Sub multiply_uint(ByVal this As big_uint, ByVal that As big_uint)
        v.resize(this.v.size() + that.v.size())
        assert(this.v.size() > 0 AndAlso that.v.size() > 0)
        For i As UInt32 = 0 To this.v.size() - uint32_1
            If this.v.get(i) = 0 Then
                Continue For
            End If
            Dim c As UInt32 = 0
            For j As UInt32 = 0 To that.v.size() - uint32_1
                Dim t As UInt64 = this.v.get(i)
                t *= that.v.get(j)
                c = add(CUInt(t And max_uint32), c, i + j)
                c += CUInt(t >> bit_count_in_uint32)
            Next
#If DEBUG Then
            assert(v.get(i + that.v.size()) = 0)
#End If
            v.set(i + that.v.size(), c)
        Next
        remove_last_blank()
    End Sub

    Private Shared Function multiply_schoolbook(ByVal this As big_uint, ByVal that As big_uint) As big_uint
        If this Is Nothing OrElse that Is Nothing OrElse this.is_zero() OrElse that.is_zero() Then
            Return New big_uint()
        End If
        If this.is_one() Then
            Return that.CloneT()
        End If
        If that.is_one() Then
            Return this.CloneT()
        End If
        Dim res As New big_uint()
        res.multiply_uint(this, that)
        Return res
    End Function

    Private Shared Function multiply_karatsuba(ByVal a As big_uint, ByVal b As big_uint) As big_uint
        If a Is Nothing OrElse b Is Nothing OrElse a.is_zero() OrElse b.is_zero() Then
            Return New big_uint()
        End If
        If a.is_one() Then
            Return b.CloneT()
        End If
        If b.is_one() Then
            Return a.CloneT()
        End If
        If a.power_of_2() Then
            Return b.CloneT().left_shift(a.trailing_binary_zero_count())
        End If
        If b.power_of_2() Then
            Return a.CloneT().left_shift(b.trailing_binary_zero_count())
        End If

        Dim a_size As UInt32 = a.v.size()
        Dim b_size As UInt32 = b.v.size()
        Dim n As UInt32 = max(a_size, b_size)
        If n <= 32 OrElse a_size <= 4 OrElse b_size <= 4 Then
            Return multiply_schoolbook(a, b)
        End If

        Dim m As UInt32 = (n + uint32_1) >> 1

        Dim a0 As big_uint = Nothing
        Dim a1 As big_uint = Nothing
        If a_size <= m Then
            a0 = a
            a1 = New big_uint()
        Else
            a0 = New big_uint()
            a0.v.resize(m)
            arrays.copy(a0.v.data(), 0, a.v.data(), 0, m)
            a0.remove_extra_blank()

            Dim a1_size As UInt32 = a_size - m
            a1 = New big_uint()
            a1.v.resize(a1_size)
            arrays.copy(a1.v.data(), 0, a.v.data(), m, a1_size)
            a1.remove_extra_blank()
        End If

        Dim b0 As big_uint = Nothing
        Dim b1 As big_uint = Nothing
        If b_size <= m Then
            b0 = b
            b1 = New big_uint()
        Else
            b0 = New big_uint()
            b0.v.resize(m)
            arrays.copy(b0.v.data(), 0, b.v.data(), 0, m)
            b0.remove_extra_blank()

            Dim b1_size As UInt32 = b_size - m
            b1 = New big_uint()
            b1.v.resize(b1_size)
            arrays.copy(b1.v.data(), 0, b.v.data(), m, b1_size)
            b1.remove_extra_blank()
        End If

        Dim z0 As big_uint = multiply_karatsuba(a0, b0)
        Dim z2 As big_uint = If(a1.is_zero() OrElse b1.is_zero(), New big_uint(), multiply_karatsuba(a1, b1))

        Dim sum_a As big_uint = a0 + a1
        Dim sum_b As big_uint = b0 + b1
        Dim z1 As big_uint = multiply_karatsuba(sum_a, sum_b)

        Dim mid As big_uint = z1
        mid.assert_sub(z2)
        mid.assert_sub(z0)

        z2.left_shift(CULng(2) * m * bit_count_in_uint32)
        mid.left_shift(CULng(m) * bit_count_in_uint32)

        Dim res As big_uint = z0
        res.add(mid)
        res.add(z2)
        Return res
    End Function

    'store the result of this * that in me
    <MethodImpl(math_debug.aggressive_inlining)>
    Private Sub multiply(ByVal this As big_uint, ByVal that As big_uint)
        If this Is Nothing OrElse that Is Nothing OrElse this.is_zero() OrElse that.is_zero() Then
            set_zero()
            Return
        End If
        If this.is_one() Then
            assert(replace_by(that))
            Return
        End If
        If that.is_one() Then
            assert(replace_by(this))
            Return
        End If
        If this.power_of_2() Then
            assert(replace_by(that))
            left_shift(this.trailing_binary_zero_count())
            Return
        End If
        If that.power_of_2() Then
            assert(replace_by(this))
            left_shift(that.trailing_binary_zero_count())
            Return
        End If
        set_zero()

#If USE_MULTIPLY_BIT Then
        If that._1count() <= (that.uint32_size() << 1) Then
            multiply_bit(this, that)
            Return
        End If
#End If
        Dim n As UInt32 = max(this.v.size(), that.v.size())
        If n <= 32 OrElse this.v.size() <= 4 OrElse that.v.size() <= 4 Then
            multiply_uint(this, that)
        Else
            Dim r As big_uint = multiply_karatsuba(this, that)
            adaptive_array_uint32.swap(v, r.v)
        End If
    End Sub

    'store the result of yroot(me, that) in me, and the remainder will be the me - (me ^ (yroot(me, that)))
    <MethodImpl(math_debug.aggressive_inlining)>
    Private Sub extract(ByVal that As big_uint, ByRef remainder As big_uint, ByRef divide_by_zero As Boolean)
        If that Is Nothing OrElse that.is_zero() Then
            If is_one() Then
                divide_by_zero = False
                remainder = zero()
            Else
                divide_by_zero = True
            End If
            Return
        End If
        divide_by_zero = False
        If that.is_one() Then
            remainder = zero()
            Return
        End If
        If is_zero() OrElse is_one() Then
            remainder = zero()
            Return
        End If
        Dim r As New big_uint(bit_count())
        r.divide(that, remainder)
        Dim l As UInt64 = 0
        If remainder.is_zero() Then
            l = r.as_uint64()
        Else
            l = r.as_uint64() + uint64_1
        End If
        assert(l > 0)
        r.set_zero()
        r.set_bit_count(l)
        For i As UInt64 = 0 To l - uint64_1
            r.setrbit(l - i - uint64_1, True)
            If r ^ that > Me Then
                r.setrbit(l - i - uint64_1, False)
            End If
#If DEBUG Then
            assert(r ^ that <= Me)
#End If
        Next
        remainder = New big_uint(Me - r ^ that)
        assert(replace_by(r))
    End Sub

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Sub divide_bit(ByVal that As big_uint, ByVal remainder As big_uint)
        divide_bit(that, remainder, Me)
    End Sub

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Sub divide_uint(ByVal that As big_uint, ByVal remainder As big_uint)
        divide_uint(that, remainder, Me)
    End Sub

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Sub modulus_bit(ByVal that As big_uint)
        divide_bit(that, Me, Nothing)
    End Sub

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Sub modulus_uint(ByVal that As big_uint)
        divide_uint(that, Me, Nothing)
    End Sub

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Function last_non_zero_position() As UInt32
        Dim i As UInt32 = 0
        While i < v.size()
            If v.get(i) <> 0 Then
                Return i
            End If
            i += uint32_1
        End While
        assert(False)
        Return max_uint32
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Function as_uint32() As UInt32
        Dim o As Boolean = False
        Dim r As UInt32 = as_uint32(o)
        assert(Not o)
        Return r
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Function as_uint64() As UInt64
        Dim o As Boolean = False
        Dim r As UInt64 = as_uint64(o)
        assert(Not o)
        Return r
    End Function
End Class
