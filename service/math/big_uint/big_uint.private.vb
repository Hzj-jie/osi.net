
Option Explicit On
Option Infer Off
Option Strict On

#Const DEBUG = False

#Const BITWISE_DIVIDE = True
Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class big_uint
    'support move constructor
    Private Sub New(ByVal i As adaptive_array_uint32)
        Me.New()
        Me.v = i
    End Sub

    'sub d at position p with carry-over as c
    Private Sub [sub](ByVal d As UInt32, ByRef c As UInt32, ByVal p As UInt32)
        assert(p >= 0 AndAlso p < v.size())
        Dim t As Int64 = 0
        t = -c
        t += v.get(p)
        t -= d
        v.set(p, CUInt(t And max_uint32))
        c = If(t < 0, uint32_1, uint32_0)
    End Sub

    Private Sub [sub](ByVal that As big_uint, ByRef c As UInt32)
        c = 0
        If that Is Nothing OrElse that.is_zero() Then
            Return
        End If
        If that.v.size() > v.size() Then
            v.resize(that.v.size())
        End If
        assert(v.size() > 0 AndAlso that.v.size() > 0)
        Dim i As UInt32 = 0
        For i = 0 To that.v.size() - uint32_1
            [sub](that.v.get(i), c, i)
        Next
        If c > 0 Then
            For i = i To v.size() - uint32_1
                [sub](0, c, i)
                If c = 0 Then
                    Exit For
                End If
            Next
        End If
        remove_extra_blank()
    End Sub

    'add d to the pos as p with carry-over as c
    Private Sub add(ByVal d As UInt32, ByRef c As UInt32, ByVal p As UInt32)
        'this assert is too costly
#If DEBUG Then
        assert(p < v.size())
#End If
        Dim t As UInt64 = 0
        t = c
        t += v.get(p)
        t += d
        v.set(p, CUInt(t And max_uint32))
        c = CUInt(t >> bit_count_in_uint32)
    End Sub

    Private Function remove_extra_blank() As UInt32
        Dim r As UInt32 = 0
        While Not v.empty() AndAlso v.back() = 0
            v.pop_back()
            r += uint32_1
        End While
        Return r
    End Function

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
        v.resize(this.v.size() + that.v.size())
        assert(this.v.size() > 0 AndAlso that.v.size() > 0)
        Dim c As UInt32 = 0
        For i As UInt32 = 0 To this.v.size() - uint32_1
            If this.v.get(i) <> 0 Then
                For j As UInt32 = 0 To that.v.size() - uint32_1
                    Dim t As UInt64 = 0
                    t = this.v.get(i)
                    t *= that.v.get(j)
                    add(CUInt(t And max_uint32), c, i + j)
                    c += CUInt(t >> bit_count_in_uint32)
                Next
                If c > 0 Then
                    add(0, c, i + that.v.size())
                    assert(c = uint32_0)
                End If
            End If
        Next
        assert(remove_extra_blank() <= 1)
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

    'store the result of me / that in me, and remainder will be the remainder
    Private Sub divide(ByVal that As UInt32, ByRef remainder As UInt32, ByRef divide_by_zero As Boolean)
        If that = 0 Then
            divide_by_zero = True
            Return
        End If
        divide_by_zero = False
        remainder = 0
        If is_zero() OrElse that = 1 Then
            Return
        End If
        If is_one() Then
            remainder = 1
            set_zero()
            Return
        End If
        assert(v.size() > 0)
#If 0 Then
        'why this is slower?
        Dim r As UInt64 = 0
        Dim i As UInt32 = 0
        i = v.size() - uint32_1
        While True
            If r > 0 OrElse v(i) > 0 Then
                r <<= bit_count_in_uint32
                r = r Or v(i)

#If DEBUG Then
                    Dim t As UInt64 = 0
                    t = r.div_rem(that, r)
                    assert(t <= max_uint32)
                    v(i) = t
#Else
                    v(i) = CUInt(r.div_rem(that, r))
#End If
            End If
            If i = 0 Then
                Exit While
            End If
            i -= uint32_1
        End While
        assert(r <= max_uint32)
        remainder = r
        assert(remove_extra_blank() <= 1)
#Else
        Dim i As UInt32 = 0
        i = v.size() - uint32_1
        While True
            If remainder > 0 OrElse v.get(i) > 0 Then
                Dim t As UInt64 = 0
                t = remainder
                t <<= bit_count_in_uint32
                t = t Or v.get(i)
                t = t.div_rem(that, remainder)
#If DEBUG Then
                v.set(i, assert_which.of(t).can_cast_to_uint32())
#Else
                v.set(i, CUInt(t))
#End If
            End If
            If i = 0 Then
                Exit While
            End If
            i -= uint32_1
        End While
        assert(remove_extra_blank() <= 1)
#End If
        assert(remainder < that)
    End Sub

    'store the result of me / that in me, and remainder will be the remainder
    Private Sub divide(ByVal that As big_uint, ByRef remainder As big_uint, ByRef divide_by_zero As Boolean)
        If that Is Nothing OrElse that.is_zero() Then
            divide_by_zero = True
            Return
        End If
        If that.fit_uint32() Then
            Dim r As UInt32 = 0
            divide(that.as_uint32(), r, divide_by_zero)
            remainder = New big_uint(r)
            Return
        End If
        assert(Not that.is_zero_or_one())
        remainder = move(Me)
        set_zero()
        If remainder.is_zero_or_one() Then
            Return
        End If
        Dim remainder_bit_count As UInt64 = 0
        Dim that_bit_count As UInt64 = 0
        remainder_bit_count = remainder.bit_count()
        that_bit_count = that.bit_count()
        If remainder_bit_count < that_bit_count Then
            'do not need to divide
            Return
        End If
        'make sure the that will not be impacted during the calculation
#If DEBUG Then
        Dim original_that As big_uint = Nothing
        original_that = that
#End If
        that = New big_uint(that)
        set_bit_count(remainder_bit_count - that_bit_count + uint64_1)
#If BITWISE_DIVIDE Then
        that.left_shift(remainder_bit_count - that_bit_count)
        Dim i As UInt64 = 0
        i = remainder_bit_count - that_bit_count
        While True
            Dim cmp As Int32 = 0
            cmp = that.compare(remainder)
            If cmp = 0 Then
                setrbit(i, True)
                remainder.set_zero()
                'do not care about that after the operation, since the data has been copied already
                Exit While
            End If
            If cmp < 0 Then
                setrbit(i, True)
                remainder.assert_sub(that)
            Else 'that > remainder, right_shift again
            End If
            'do not care about that after the operation, since the data has been copied already
            remainder_bit_count = remainder.bit_count()
            that_bit_count = that.bit_count()
            If that_bit_count > remainder_bit_count Then
                Dim s As UInt64 = 0
                s = that_bit_count - remainder_bit_count
                If s > i Then
                    Exit While
                End If
                that.right_shift(s)
                i = i + uint64_1 - s
            ElseIf that_bit_count = remainder_bit_count Then
                that.right_shift(uint64_1)
            Else
                'should not happen, since remainder has just been subtracted by that
                assert(False)
            End If
            If i = 0 Then
                Exit While
            End If
            i -= uint64_1
        End While
#Else
        that.left_shift_slot(v.size(), Nothing)
        Dim i As UInt32 = 0
        i = v.size() - uint32_1
        While True
            that.right_shift_slot(1)
            Dim cmp As Int32 = 0
            cmp = that.compare(remainder)
            If cmp = 0 Then
                v(i) = 1
                remainder.set_zero()
                Exit While
            End If
            If cmp < 0 Then
                assert(remainder.v.size() - that.v.size() <= 1)
                Dim t As UInt32 = 0
                If remainder.v.size() = that.v.size() Then
                    t = remainder.v.back() \ that.v.back()
                Else
                    t = ((CULng(remainder.v.back()) << bit_count_in_uint32) +
                         remainder.v(remainder.v.size() - 2)) \ that.v.back()
                    assert(t > 0)
                End If
                If t > 0 Then
                    Dim c As big_uint = Nothing
                    c = that * t
                    While remainder.less(c) AndAlso t > 1
                        t -= uint32_1
                        c = that * t
                    End While
                    If t > 0 Then
                        v(i) = t
                        remainder.assert_sub(c)
                        If remainder.is_zero_or_one() Then
                            Exit While
                        End If
                    End If
                End If
            End If

            If i = 0 Then
                Exit While
            End If
            i -= uint32_1
        End While
#End If
#If DEBUG Then
        assert(remainder.less(original_that))
#End If
        assert(remove_extra_blank() <= 1)
    End Sub

    'fake a push_front action for vector
    Private Sub left_shift_slot(ByVal slot_count As UInt32, ByRef last_no_zero_position As UInt32)
        If slot_count = 0 Then
            Return
        End If
        v.resize(slot_count + v.size())
        Dim i As UInt32 = 0
        i = v.size() - uint32_1
        While True
            v.set(i, v.get(i - slot_count))
            If v.get(i) <> 0 Then
                last_no_zero_position = i
            End If
            If i = slot_count Then
                Exit While
            End If
            i -= uint32_1
        End While
        assert(last_no_zero_position >= slot_count AndAlso last_no_zero_position < v.size())
        i = slot_count - uint32_1
        While True
            v.set(i, uint32_0)
            If i = 0 Then
                Exit While
            End If
            i -= uint32_1
        End While
    End Sub

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
