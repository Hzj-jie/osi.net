
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Public NotInheritable Class big_uint
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
            divide(that.as_uint32(), divide_by_zero, r)
            assert(Not divide_by_zero)
            remainder = New big_uint(r)
            Return Me
        End If
        assert(Not that.is_zero_or_one())
        remainder = move(Me)
        set_zero()
        If remainder.less(that) Then
            Return Me
        End If

#If DEBUG Then
        assert(remainder.bit_count() >= that.bit_count())
#End If

        'make sure the that will not be impacted during the calculation
#If DEBUG Then
        Dim original_that As big_uint = Nothing
        original_that = that.CloneT()
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

    Public Function divide(ByVal that As UInt32,
                           ByRef divide_by_zero As Boolean,
                           Optional ByRef remainder As UInt32 = 0) As big_uint
        If that = 0 Then
            divide_by_zero = True
            Return Me
        End If
        divide_by_zero = False
        remainder = 0
        If is_zero() OrElse that = 1 Then
            Return Me
        End If
        If is_one() Then
            remainder = 1
            set_zero()
            Return Me
        End If
        If that.bit_count() = 1 Then
            remainder = (lowest_uint32() Mod that)
            right_shift(that.bit_count() - uint32_1)
            Return Me
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
        remove_last_blank()
#If DEBUG Then
        assert(remainder < that)
#End If
        Return Me
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Shared Sub divide_bit(ByVal that As big_uint,
                                  ByVal remainder As big_uint,
                                  ByVal result As big_uint)
        Dim i As UInt64 = 0
        i = remainder.bit_count() - that.bit_count()
        If Not result Is Nothing Then
            result.set_bit_count(remainder.bit_count() - that.bit_count() + uint64_1)
        End If
        that = that.CloneT()
        that.left_shift(remainder.bit_count() - that.bit_count())
        While True
            Dim cmp As Int32 = 0
            cmp = that.compare(remainder)
            If cmp = 0 Then
                If Not result Is Nothing Then
                    result.setrbit(i, True)
                End If
                remainder.set_zero()
                'do not care about that after the operation, since the data has been copied already
                Exit While
            End If
            If cmp < 0 Then
                If Not result Is Nothing Then
                    result.setrbit(i, True)
                End If
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

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Shared Sub divide_uint(ByVal that As big_uint,
                                   ByVal remainder As big_uint,
                                   ByVal result As big_uint)
        Dim i As UInt32 = 0
        i = remainder.uint32_size() - that.uint32_size()
        If Not result Is Nothing Then
            result.v.resize(i + uint32_1)
        End If
        While True
            While True
                If remainder.uint32_size() < that.uint32_size() + i Then
                    Exit While
                End If
                Dim n As UInt64 = 0
                Dim d As UInt64 = 0
                select_significant_divide_fraction(remainder, that, i, n, d)
                If n < d Then
                    Exit While
                End If
                If n = d Then
                    Dim cmp As Int32 = 0
                    cmp = compare(remainder, that, i)
                    If cmp >= 0 Then
                        remainder.assert_sub(that, i)
                        If Not result Is Nothing Then
                            result.recursive_add(uint32_1, i)
                        End If
                        If cmp = 0 Then
                            Return
                        End If
                    End If
                    Exit While
                End If
                n \= (d + uint64_1)
                Dim t32 As UInt32 = 0
#If DEBUG Then
                t32 = assert_which.of(n).can_cast_to_uint32()
#Else
                t32 = CUInt(n)
#End If
                If Not result Is Nothing Then
                    result.recursive_add(t32, i)
                End If
                remainder.assert_sub(that * t32, i)
            End While

            If i = 0 Then
                Return
            End If
            i -= uint32_1
        End While
    End Sub

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Shared Sub select_significant_divide_fraction(ByVal remainder As big_uint,
                                                          ByVal that As big_uint,
                                                          ByVal i As UInt32,
                                                          ByRef n As UInt64,
                                                          ByRef d As UInt64)
#If DEBUG Then
        assert(remainder.uint32_size() >= that.uint32_size() + i)
#End If
        If remainder.uint32_size() = that.uint32_size() + i Then
            If remainder.uint32_size() > 1 Then
                n = remainder.highest_uint64()
                d = that.highest_uint64()
                Return
            End If
            n = remainder.highest_uint32()
            d = that.highest_uint32()
            Return
        End If
        n = remainder.highest_uint64()
        d = that.highest_uint32()
    End Sub
End Class
