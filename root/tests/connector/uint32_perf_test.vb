﻿
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt

Public Class uint32_perf_test
    Inherits performance_comparison_case_wrapper

    Private Const size As UInt32 = 1024 * 1024 * 16

    Public Sub New()
        MyBase.New(New delegate_case(AddressOf uint32_eva),
                   New delegate_case(AddressOf uint32_int32_eva),
                   New delegate_case(AddressOf int32_eva),
                   New delegate_case(AddressOf int32_uint32_eva),
                   New delegate_case(AddressOf uint32_add_minus),
                   New delegate_case(AddressOf uint32_int32_add_minus),
                   New delegate_case(AddressOf int32_add_minus),
                   New delegate_case(AddressOf int32_uint32_add_minus),
                   New delegate_case(AddressOf uint32_multiply_division),
                   New delegate_case(AddressOf int32_multiply_division),
                   New delegate_case(AddressOf int32_compare),
                   New delegate_case(AddressOf uint32_compare),
                   New delegate_case(AddressOf int32_uint32_compare),
                   New delegate_case(AddressOf uint32_int32_compare))
    End Sub

    Protected Overrides Function max_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        If i < 4 AndAlso j < 4 Then
            Return 1.2
        ElseIf i >= 4 AndAlso i < 8 AndAlso j >= 4 AndAlso j < 8 Then
            Return 1.2
        ElseIf i >= 8 AndAlso i < 10 AndAlso j >= 8 AndAlso j < 10 Then
            Return 1.2
        ElseIf i >= 10 AndAlso i < 14 AndAlso j >= 10 AndAlso j < 14 Then
            Return 1.2
        End If
        Return -1
    End Function

    Private Shared Sub uint32_eva()
        Dim x As UInt32 = 0
        Dim y As UInt32 = 0
        y = rnd_uint()
        For i As UInt32 = 0 To size - uint32_1
            x = y
            assert_equal(x, y)
            x = 0
            assert_equal(x, uint32_0)
        Next
    End Sub

    Private Shared Sub uint32_int32_eva()
        Dim x As Int32 = 0
        Dim y As UInt32 = 0
        y = rnd_uint(0, max_int32)
        For i As UInt32 = 0 To size - uint32_1
            x = CInt(y)
            assert_equal(x, CInt(y))
            x = 0
            assert_equal(x, 0)
        Next
    End Sub

    Private Shared Sub int32_eva()
        Dim x As Int32 = 0
        Dim y As Int32 = 0
        y = rnd_int()
        For i As UInt32 = 0 To size - uint32_1
            x = y
            assert_equal(x, y)
            x = 0
            assert_equal(x, 0)
        Next
    End Sub

    Private Shared Sub int32_uint32_eva()
        Dim x As UInt32 = 0
        Dim y As Int32 = 0
        y = rnd_int(0, max_int32)
        For i As UInt32 = 0 To size - uint32_1
            x = CUInt(y)
            assert_equal(x, CUInt(y))
            x = 0
            assert_equal(x, uint32_0)
        Next
    End Sub

    Private Shared Sub uint32_add_minus()
        Dim x As UInt32 = 0
        For i As UInt32 = 0 To size - uint32_1
            x += uint32_1
            assert_equal(x, uint32_1)
            x -= uint32_1
            assert_equal(x, uint32_0)
        Next
    End Sub

    Private Shared Sub uint32_int32_add_minus()
        Dim x As UInt32 = 0
        For i As UInt32 = 0 To size - uint32_1
            x += 1
            assert_equal(x, uint32_1)
            x -= 1
            assert_equal(x, uint32_0)
        Next
    End Sub

    Private Shared Sub int32_add_minus()
        Dim x As Int32 = 0
        For i As UInt32 = 0 To size - uint32_1
            x += 1
            assert_equal(x, 1)
            x -= 1
            assert_equal(x, 0)
        Next
    End Sub

    Private Shared Sub int32_uint32_add_minus()
        Dim x As Int32 = 0
        For i As UInt32 = 0 To size - uint32_1
            x += uint32_1
            assert_equal(x, 1)
            x -= uint32_1
            assert_equal(x, 0)
        Next
    End Sub

    Private Shared Sub uint32_multiply_division()
        Dim x As UInt32 = 0
        Dim y As UInt32 = 0
        Dim r As UInt32 = 0
        x = rnd_uint8()
        Do
            y = rnd_uint8()
        Loop While y = 0
        r = x * y
        For i As UInt32 = 0 To size - uint32_1
            Dim c As UInt32 = 0
            c = x * y
            assert_equal(c, r)
            c \= y
            assert_equal(c, x)
        Next
    End Sub

    Private Shared Sub int32_multiply_division()
        Dim x As Int32 = 0
        Dim y As Int32 = 0
        Dim r As Int32 = 0
        x = rnd_int8()
        Do
            y = rnd_int8()
        Loop While y = 0
        r = x * y
        For i As UInt32 = 0 To size - uint32_1
            Dim c As Int32 = 0
            c = x * y
            assert_equal(c, r)
            c \= y
            assert_equal(c, x)
        Next
    End Sub

    Private Shared Sub int32_compare()
        Dim x As Int32 = 0
        Dim y As Int32 = 0
        x = rnd_int()
        y = rnd_int()
        For i As UInt32 = 0 To size - uint32_1
            do_nothing(x > y)
            do_nothing(x = y)
            do_nothing(x < y)
            do_nothing(x >= y)
            do_nothing(x <= y)
            do_nothing(x <> y)
        Next
    End Sub

    Private Shared Sub uint32_compare()
        Dim x As UInt32 = 0
        Dim y As UInt32 = 0
        x = rnd_uint()
        y = rnd_uint()
        For i As UInt32 = 0 To size - uint32_1
            do_nothing(x > y)
            do_nothing(x = y)
            do_nothing(x < y)
            do_nothing(x >= y)
            do_nothing(x <= y)
            do_nothing(x <> y)
        Next
    End Sub

    Private Shared Sub int32_uint32_compare()
        Dim x As Int32 = 0
        Dim y As UInt32 = 0
        x = rnd_int()
        y = rnd_uint()
        For i As UInt32 = 0 To size - uint32_1
            do_nothing(x > y)
            do_nothing(x = y)
            do_nothing(x < y)
            do_nothing(x >= y)
            do_nothing(x <= y)
            do_nothing(x <> y)
        Next
    End Sub

    Private Shared Sub uint32_int32_compare()
        Dim x As UInt32 = 0
        Dim y As Int32 = 0
        x = rnd_uint()
        y = rnd_int()
        For i As UInt32 = 0 To size - uint32_1
            do_nothing(x > y)
            do_nothing(x = y)
            do_nothing(x < y)
            do_nothing(x >= y)
            do_nothing(x <= y)
            do_nothing(x <> y)
        Next
    End Sub
End Class
