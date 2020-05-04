
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class big_uint
    <MethodImpl(math_debug.aggressive_inlining)>
    Private Shared Sub shift_blocks(ByVal size As UInt64, ByRef slot_count As UInt32, ByRef bit_count As Byte)
        slot_count = assert_which.of(size >> bit_count_in_uint32_shift).can_cast_to_uint32()
        bit_count = assert_which.of(size And bit_count_in_uint32_mask).can_cast_to_byte()
    End Sub

    Public Function left_shift(ByVal size As UInt64) As big_uint
        If size = 0 OrElse is_zero() Then
            Return Me
        End If
        Dim slot_count As UInt32 = 0
        Dim bit_count As Byte = 0
        shift_blocks(size, slot_count, bit_count)
        If bit_count = 0 Then
            left_shift_slot(slot_count)
        Else
            left_shift(slot_count, bit_count)
        End If
        Return Me
    End Function

    Public Function right_shift(ByVal size As UInt64) As big_uint
        If size = 0 Then
            Return Me
        End If
        If size >= Me.bit_count() Then
            set_zero()
            Return Me
        End If
        assert(Not is_zero())
        Dim slot_count As UInt32 = 0
        Dim bit_count As Byte = 0
        shift_blocks(size, slot_count, bit_count)
        If bit_count = 0 Then
            right_shift_slot(slot_count)
        Else
            right_shift(slot_count, bit_count)
        End If
        Return Me
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Sub left_shift_slot(ByVal slot_count As UInt32)
        assert(slot_count > 0)
        v.resize(slot_count + v.size())
        arrays.copy(v.data(), slot_count, v.data(), 0, v.size() - slot_count)
        arrays.clear(v.data(), 0, slot_count)
    End Sub

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Sub uint32_or(ByVal p As UInt32, ByVal i As UInt32)
        v.set(p, v.get(p) Or i)
    End Sub

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Sub left_shift(ByVal slot_count As UInt32, ByVal bit_count As Byte)
        assert(bit_count > 0)
        v.resize(v.size() + slot_count + uint32_1)
        Dim i As UInt32 = 0
        i = v.size() - slot_count - uint32_2
        While True
            Dim t As UInt64 = 0
            t = shift.left(v.get(i), bit_count)
            uint32_or(i + slot_count + uint32_1, t.high_uint32())
            v.set(i + slot_count, t.low_uint32())

            If i = 0 Then
                Exit While
            End If
            i -= uint32_1
        End While
        arrays.clear(v.data(), 0, slot_count)
        remove_last_blank()
    End Sub

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Sub right_shift_slot(ByVal slot_count As UInt32)
        assert(slot_count > 0)
        assert(slot_count < v.size())
        arrays.copy(v.data(), 0, v.data(), slot_count, v.size() - slot_count)
        v.resize(v.size() - slot_count)
    End Sub

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Sub right_shift(ByVal slot_count As UInt32, ByVal bit_count As Byte)
        assert(bit_count > 0)
        assert(v.size() >= slot_count + uint32_1)
        bit_count = bit_count_in_uint32 - bit_count
        Dim i As UInt32 = 0
        i = slot_count
        v.set(uint32_0, shift.left(v.get(i), bit_count).high_uint32())
        If i < v.size() - uint32_1 Then
            i += uint32_1
            While True
                Dim t As UInt64 = 0
                t = shift.left(v.get(i), bit_count)
                v.set(i - slot_count, t.high_uint32())
                uint32_or(i - slot_count - uint32_1, t.low_uint32())

                If i = v.size() - uint32_1 Then
                    Exit While
                End If
                i += uint32_1
            End While
        End If
        v.resize(v.size() - slot_count)
        remove_last_blank()
    End Sub

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function left_shift(ByVal size As Int32) As big_uint
        If size < 0 Then
            Return right_shift(CULng(-size))
        End If
        Return left_shift(CULng(size))
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function left_shift(ByVal size As big_uint, ByRef overflow As Boolean) As big_uint
        If size Is Nothing OrElse size.is_zero() Then
            Return Me
        End If
        Dim u As UInt64 = 0
        u = size.as_uint64(overflow)
        If Not overflow Then
            left_shift(u)
        End If
        Return Me
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function left_shift(ByVal size As big_uint) As big_uint
        Dim o As Boolean = False
        left_shift(size, o)
        If o Then
            Throw overflow()
        End If
        Return Me
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function assert_left_shift(ByVal size As big_uint) As big_uint
        Dim o As Boolean = False
        left_shift(size, o)
        assert(Not o)
        Return Me
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function right_shift(ByVal size As Int32) As big_uint
        If size < 0 Then
            Return left_shift(CULng(-size))
        End If
        Return right_shift(CULng(size))
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function right_shift(ByVal size As big_uint, ByRef overflow As Boolean) As big_uint
        If size Is Nothing OrElse size.is_zero() Then
            Return Me
        End If
        Dim u As UInt64 = 0
        u = size.as_uint64(overflow)
        If Not overflow Then
            right_shift(u)
        End If
        Return Me
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function right_shift(ByVal size As big_uint) As big_uint
        Dim o As Boolean = False
        right_shift(size, o)
        If o Then
            Throw overflow()
        End If
        Return Me
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function assert_right_shift(ByVal size As big_uint) As big_uint
        Dim o As Boolean = False
        right_shift(size, o)
        assert(Not o)
        Return Me
    End Function
End Class
