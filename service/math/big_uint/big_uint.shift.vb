
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class big_uint
    Public Function left_shift(ByVal size As Int32) As big_uint
        If size < 0 Then
            Return right_shift(CULng(-size))
        End If
        Return left_shift(CULng(size))
    End Function

    Public Function left_shift(ByVal size As UInt64) As big_uint
        If size = 0 OrElse is_zero() Then
            Return Me
        End If
        Dim till As UInt32 = 0
        left_shift_slot(assert_which.of(size >> bit_count_in_uint32_shift).can_cast_to_uint32(), till)
        Dim ls As UInt32 = 0
        ls = assert_which.of(size And bit_count_in_uint32_mask).can_cast_to_uint32()
        If ls = 0 Then
            Return Me
        End If
        Dim i As UInt32 = 0
        i = v.size() - uint32_1
        While True
            Dim t As UInt64 = 0
            t = shift.left(v.get(i), ls)
            Dim c As UInt32 = 0
            v.set(i, CUInt(t And max_uint32))
            c = CUInt(t >> bit_count_in_uint32)
            If c > 0 Then
                If i = v.size() - uint32_1 Then
                    v.push_back(0)
                End If
#If DEBUG Then
                assert((v.get(i + uint32_1) And c) = 0)
#End If
                v.set(i + uint32_1, v.get(i + uint32_1) Or c)
            End If
            If i = till Then
                Exit While
            Else
                i -= uint32_1
            End If
        End While
        Return Me
    End Function

    Public Function left_shift(ByVal size As big_uint, ByRef overflow As Boolean) As big_uint
        If size Is Nothing OrElse size.is_zero() Then
            Return Me
        End If
        Dim u As UInt64 = 0
        u = size.as_uint64(overflow)
        If Not overflow Then
            assert(object_compare(left_shift(u), Me) = 0)
        End If
        Return Me
    End Function

    Public Function left_shift(ByVal size As big_uint) As big_uint
        Dim o As Boolean = False
        assert(object_compare(left_shift(size, o), Me) = 0)
        If o Then
            Throw overflow()
        End If
        Return Me
    End Function

    Public Function assert_left_shift(ByVal size As big_uint) As big_uint
        Dim o As Boolean = False
        assert(object_compare(left_shift(size, o), Me) = 0)
        assert(Not o)
        Return Me
    End Function

    Public Function right_shift(ByVal size As Int32) As big_uint
        If size < 0 Then
            Return left_shift(CULng(-size))
        Else
            Return right_shift(CULng(size))
        End If
    End Function

    Public Function right_shift(ByVal size As UInt64) As big_uint
        If size = 0 Then
            Return Me
        End If
        If size >= bit_count() Then
            set_zero()
            Return Me
        End If
        assert(Not is_zero())
        right_shift_slot(assert_which.of(size >> bit_count_in_uint32_shift).can_cast_to_uint32())
        Dim ls As UInt32 = 0
        ls = assert_which.of(size And bit_count_in_uint32_mask).can_cast_to_uint32()
        If ls = 0 Then
            Return Me
        End If
        For i As UInt32 = 0 To v.size() - uint32_1
            Dim t As UInt64 = 0
            t = shift.left(v.get(i), bit_count_in_uint32 - ls)
            Dim c As UInt32 = 0
            c = CUInt(t And max_uint32)
            v.set(i, CUInt(t >> bit_count_in_uint32))
            If i > 0 AndAlso c > 0 Then
#If DEBUG Then
                assert((v.get(i - uint32_1) And c) = 0)
#End If
                v.set(i - uint32_1, v.get(i - uint32_1) Or c)
            End If
        Next
        assert(remove_extra_blank() <= 1)
        Return Me
    End Function

    Public Function right_shift(ByVal size As big_uint, ByRef overflow As Boolean) As big_uint
        If size Is Nothing OrElse size.is_zero() Then
            Return Me
        End If
        Dim u As UInt64 = 0
        u = size.as_uint64(overflow)
        If Not overflow Then
            assert(object_compare(right_shift(u), Me) = 0)
        End If
        Return Me
    End Function

    Public Function right_shift(ByVal size As big_uint) As big_uint
        Dim o As Boolean = False
        assert(object_compare(right_shift(size, o), Me) = 0)
        If o Then
            Throw overflow()
        End If
        Return Me
    End Function

    Public Function assert_right_shift(ByVal size As big_uint) As big_uint
        Dim o As Boolean = False
        assert(object_compare(right_shift(size, o), Me) = 0)
        assert(Not o)
        Return Me
    End Function
End Class
