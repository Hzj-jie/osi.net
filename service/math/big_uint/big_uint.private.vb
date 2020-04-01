
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices

' #Const DEBUG = False
#Const USE_MULTIPLY_BIT = False
#Const USE_DIVIDE_BIT = False

Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class big_uint
    'support move constructor
    Private Sub New(ByVal i As adaptive_array_uint32)
        Me.v = i
    End Sub

    'sub d at position p with carry-over as c
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Sub [sub](ByVal d As UInt32, ByRef c As UInt32, ByVal p As UInt32)
        'this assert is too costly
#If DEBUG Then
        assert(p < v.size())
#End If
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
    <MethodImpl(method_impl_options.aggressive_inlining)>
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

    Private Sub add(ByVal d As UInt32, ByVal p As UInt32)
        While d > 0 AndAlso p < v.size()
            Dim nd As UInt32 = 0
            add(d, nd, p)
            d = nd
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
        Dim c As UInt32 = 0
        For i As UInt32 = 0 To this.v.size() - uint32_1
            If this.v.get(i) = 0 Then
                Continue For
            End If
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
        assert(remainder < that)
    End Sub

    Private Sub divide_bit(ByVal that As big_uint, ByVal remainder As big_uint)
        Dim i As UInt64 = 0
        i = remainder.bit_count() - that.bit_count()
        set_bit_count(remainder.bit_count() - that.bit_count() + uint64_1)
        that = that.CloneT()
        that.left_shift(remainder.bit_count() - that.bit_count())
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
            cmp = that.bit_count().CompareTo(remainder.bit_count())
            If cmp > 0 Then
                Dim s As UInt64 = 0
                s = that.bit_count() - remainder.bit_count()
                If s > i Then
                    Exit While
                End If
                that.right_shift(s)
                i = i + uint64_1 - s
            ElseIf cmp = 0 Then
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
    End Sub

    Private Sub divide_uint(ByVal that As big_uint, ByVal remainder As big_uint)
        Dim i As UInt32 = 0
        i = remainder.uint32_size() - that.uint32_size()
        v.resize(i + uint32_1)
        that.left_shift(CULng(i) << bit_count_in_uint32_shift)
        While True
            While True
                If remainder.uint32_size() < that.uint32_size() Then
                    Exit While
                End If
                Dim t As UInt64 = 0
                t = remainder.highest_uint32()
                If remainder.uint32_size() > (that.uint32_size()) Then
                    t <<= bit_count_in_uint32
                    t = t Or remainder.second_highest_uint32()
                End If
                If t < that.highest_uint32() Then
                    Exit While
                End If
                If t = that.highest_uint32() Then
                    Dim cmp As Int32 = 0
                    cmp = that.compare(remainder)
                    If cmp <= 0 Then
                        remainder.assert_sub(that)
                        add(uint32_1, i)
                        If cmp = 0 Then
                            that.right_shift(CULng(i) << bit_count_in_uint32_shift)
                            Return
                        End If
                    End If
                    Exit While
                End If
                t \= (that.highest_uint32() + uint32_1)
                Dim t32 As UInt32 = 0
#If DEBUG Then
                t32 = assert_which.of(t).can_cast_to_uint32()
#Else
                t32 = CUInt(t)
#End If
                add(t32, i)
                remainder.assert_sub(that * t32)
            End While

            If i = 0 Then
                Return
            End If

            that.right_shift(CULng(bit_count_in_uint32))
            i -= uint32_1
        End While
    End Sub

    'store the result of me / that in me, and remainder will be the remainder
    Private Sub divide(ByVal that As big_uint, ByRef remainder As big_uint, ByRef divide_by_zero As Boolean)
        If that Is Nothing OrElse that.is_zero() Then
            divide_by_zero = True
            Return
        End If
        divide_by_zero = False
        If is_zero() OrElse that.is_one() Then
            remainder = big_uint.zero()
            Return
        End If
        If is_one() Then
            remainder = big_uint.one()
            set_zero()
            Return
        End If
        If that.power_of_2() Then
            Dim l As UInt64 = 0
            l = that.bit_count() - uint64_1
            remainder = Me.CloneT().[and](that - uint32_1)
            right_shift(l)
            Return
        End If
        If that.fit_uint32() Then
            Dim r As UInt32 = 0
            divide(that.as_uint32(), r, divide_by_zero)
            assert(Not divide_by_zero)
            remainder = New big_uint(r)
            Return
        End If
        assert(Not that.is_zero_or_one())
        remainder = move(Me)
        set_zero()
        If remainder.less(that) Then
            Return
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
        assert(remove_extra_blank() <= 1)

#If DEBUG Then
        assert(remainder.less(original_that))
        assert(that.equal(original_that))
#End If
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

    Private Sub modulus_bit(ByVal that As big_uint)
        Dim dl As UInt64 = 0
        dl = bit_count() - that.bit_count()
        that = that.CloneT()
        that.left_shift(dl)
        While True
            Dim cmp As Int32 = 0
            cmp = compare(that)
            If cmp = 0 Then
                set_zero()
                Return
            End If
            If cmp > 0 Then
                assert_sub(that)
            End If
            If dl = uint64_0 Then
                Exit While
            End If
            dl -= uint64_1
            that.right_shift(1)
        End While
    End Sub

    Private Sub modulus_uint(ByVal that As big_uint)
        Dim i As UInt32 = 0
        i = uint32_size() - that.uint32_size()
        that.left_shift(CULng(i) << bit_count_in_uint32_shift)
        While True
            While True
                If uint32_size() < that.uint32_size() Then
                    Exit While
                End If
                Dim t As UInt64 = 0
                t = highest_uint32()
                If uint32_size() > (that.uint32_size()) Then
                    t <<= bit_count_in_uint32
                    t = t Or second_highest_uint32()
                End If
                If t < that.highest_uint32() Then
                    Exit While
                End If
                If t = that.highest_uint32() Then
                    Dim cmp As Int32 = 0
                    cmp = that.compare(Me)
                    If cmp <= 0 Then
                        assert_sub(that)
                        If cmp = 0 Then
                            that.right_shift(CULng(i) << bit_count_in_uint32_shift)
                            Return
                        End If
                    End If
                    Exit While
                End If
                t \= (that.highest_uint32() + uint32_1)
                Dim t32 As UInt32 = 0
#If DEBUG Then
                t32 = assert_which.of(t).can_cast_to_uint32()
#Else
                t32 = CUInt(t)
#End If
                assert_sub(that * t32)
            End While

            If i = 0 Then
                Return
            End If

            that.right_shift(CULng(bit_count_in_uint32))
            i -= uint32_1
        End While
    End Sub
End Class
