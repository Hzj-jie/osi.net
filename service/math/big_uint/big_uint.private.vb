
Option Explicit On
Option Infer Off
Option Strict On

' #Const DEBUG = False
#Const USE_MULTIPLY_BIT = False

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class big_uint
    'support move constructor
    Private Sub New(ByVal i As adaptive_array_uint32)
        Me.v = i
    End Sub

    'sub d at position p with carry-over as c
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function [sub](ByVal d As UInt32, ByVal c As UInt32, ByVal p As UInt32) As UInt32
        'this assert is too costly
#If DEBUG Then
        assert(p < v.size())
#End If
        Dim t As Int64 = 0
        t = -c
        t += v.get(p)
        t -= d
        v.set(p, t.first_uint32())
        Return If(t < 0, uint32_1, uint32_0)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function sub_with_overflow(ByVal that As big_uint) As Boolean
        Dim c As UInt32 = 0
        If that Is Nothing OrElse that.is_zero() Then
            Return False
        End If
        If that.v.size() > v.size() Then
            v.resize(that.v.size())
        End If
        assert(v.size() > 0 AndAlso that.v.size() > 0)
        Dim i As UInt32 = 0
        For i = 0 To that.v.size() - uint32_1
            c = [sub](that.v.get(i), c, i)
        Next
        If c > 0 Then
            For i = i To v.size() - uint32_1
                c = [sub](0, c, i)
                If c = 0 Then
                    Exit For
                End If
            Next
        End If
        remove_extra_blank()
        assert(c = 0 OrElse c = 1)
        Return (c = 1)
    End Function

    'add d to the pos as p with carry-over as c
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function add(ByVal d As UInt32, ByVal c As UInt32, ByVal p As UInt32) As UInt32
        'this assert is too costly
#If DEBUG Then
        assert(p < v.size())
#End If
        Dim t As UInt64 = 0
        t = c
        t += v.get(p)
        t += d
        v.set(p, t.first_uint32())
        Return t.second_uint32()
    End Function

    'add d to the pos as p with carry-over as c
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function add(ByVal d As UInt32, ByVal p As UInt32) As UInt32
        'this assert is too costly
#If DEBUG Then
        assert(p < v.size())
#End If
        Dim t As UInt64 = 0
        t = v.get(p)
        t += d
        v.set(p, t.first_uint32())
        Return t.second_uint32()
    End Function

    Private Sub recursive_add(ByVal d As UInt32, ByVal p As UInt32)
        While d > 0 AndAlso p < v.size()
            d = add(d, p)
            p += uint32_1
        End While
        If d > 0 Then
            v.push_back(d)
        End If
    End Sub

    Private Function remove_extra_blank() As UInt32
        Dim r As UInt32 = 0
        While Not v.empty() AndAlso v.back() = 0
            v.pop_back()
            r += uint32_1
        End While
        Return r
    End Function

    Private Sub multiply_bit(ByVal this As big_uint, ByVal that As big_uint)
        this = this.CloneT()
        that = that.CloneT()
        While Not that.is_zero()
            Dim m As UInt32 = 0
            m = that.remove_binary_trailing_zeros()
            assert(that.odd())
            this.left_shift(m)
            add(this)
        End While
    End Sub

    Private Sub multiply_uint32(ByVal this As big_uint, ByVal that As big_uint)
        v.resize(this.v.size() + that.v.size())
        assert(this.v.size() > 0 AndAlso that.v.size() > 0)
        For i As UInt32 = 0 To this.v.size() - uint32_1
            If this.v.get(i) = 0 Then
                Continue For
            End If
            Dim c As UInt32 = 0
            For j As UInt32 = 0 To that.v.size() - uint32_1
                Dim t As UInt64 = 0
                t = this.v.get(i)
                t *= that.v.get(j)
                c = add(t.first_uint32(), c, i + j)
                c += t.second_uint32()
            Next
            If c > 0 Then
                c = add(c, i + that.v.size())
                assert(c = uint32_0)
            End If
        Next
        assert(remove_extra_blank() <= 1)
    End Sub

    'store the result of this * that in me
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
        set_zero()

#If USE_MULTIPLY_BIT Then
        If that._1count() <= (that.uint32_size() << 1) Then
            multiply_bit(this, that)
        Else
            multiply_uint32(this, that)
        End If
#Else
        multiply_uint32(this, that)
#End If
    End Sub

    'store the result of yroot(me, that) in me, and the remainder will be the me - (me ^ (yroot(me, that)))
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
        Dim r As big_uint = Nothing
        r = New big_uint(bit_count())
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

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Sub divide_bit(ByVal that As big_uint, ByVal remainder As big_uint)
        divide_bit(that, remainder, Me)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Sub divide_uint(ByVal that As big_uint, ByVal remainder As big_uint)
        divide_uint(that, remainder, Me)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Sub modulus_bit(ByVal that As big_uint)
        divide_bit(that, Me, Nothing)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Sub modulus_uint(ByVal that As big_uint)
        divide_uint(that, Me, Nothing)
    End Sub

    'fake a push_front action for vector and return the last non-zero position
    Private Function left_shift_slot_till(ByVal slot_count As UInt32) As UInt32
        assert(slot_count > 0)
        If slot_count = 0 Then
            Return Me.last_non_zero_position()
        End If

        Dim last_non_zero_position As UInt32 = 0
        v.resize(slot_count + v.size())
        Dim i As UInt32 = 0
        i = v.size() - uint32_1
        While True
            v.set(i, v.get(i - slot_count))
            If v.get(i) <> 0 Then
                last_non_zero_position = i
            End If
            If i = slot_count Then
                Exit While
            End If
            i -= uint32_1
        End While
        assert(last_non_zero_position >= slot_count AndAlso last_non_zero_position < v.size())
        memclr(v.data(), 0, slot_count)
        Return last_non_zero_position
    End Function

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

    'fake a pop_front action for vector
    Private Sub right_shift_slot(ByVal slot_count As UInt32)
        If slot_count = 0 Then
            Return
        End If
        assert(slot_count < v.size())
        Dim ns As UInt32 = 0
        ns = v.size() - slot_count
        memmove(v.data(), 0, v.data(), slot_count, ns)
        v.resize(ns)
    End Sub

    Private Function as_uint32() As UInt32
        Dim o As Boolean = False
        Dim r As UInt32 = 0
        r = as_uint32(o)
        assert(Not o)
        Return r
    End Function

    Private Function as_uint64() As UInt64
        Dim o As Boolean = False
        Dim r As UInt64 = 0
        r = as_uint64(o)
        assert(Not o)
        Return r
    End Function
End Class
