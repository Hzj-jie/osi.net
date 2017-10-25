
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _shift
    Private Sub valid_shift_moves(Of T)(ByVal moves As Byte)
        If isdebugmode() Then
            assert(moves <= sizeof(Of T)() * bit_count_in_byte,
                   "cannot shift a variable over its size * ", bit_count_in_byte)
        End If
    End Sub

    Private ReadOnly bitcount_in_uint32 As Byte = CByte(sizeof_uint32 * bit_count_in_byte)
    Private ReadOnly bitcount_in_uint64 As Byte = CByte(sizeof_uint64 * bit_count_in_byte)

    <Extension()> Public Function right_shift(ByRef b As UInt32, ByVal moves As Byte) As UInt32
        valid_shift_moves(Of UInt32)(moves)
        If moves > 0 AndAlso moves < sizeof(Of UInt32)() * bit_count_in_byte Then
            b = (b >> moves) +
                ((b And ((uint32_1 << moves) - uint32_1)) << (bitcount_in_uint32 - moves))
        End If
        Return b
    End Function

    <Extension()> Public Function left_shift(ByRef b As UInt32, ByVal moves As Byte) As UInt32
        valid_shift_moves(Of UInt32)(moves)
        If moves > 0 AndAlso moves < sizeof(Of UInt32)() * bit_count_in_byte Then
            b = (b << moves) +
                ((b And (((uint32_1 << moves) - uint32_1) << (bitcount_in_uint32 - moves))) _
                    >> (bitcount_in_uint32 - moves))
        End If
        Return b
    End Function

    <Extension()> Public Function right_shift(ByRef b As Int32, ByVal moves As Byte) As Int32
        b = uint32_int32(right_shift(int32_uint32(b), moves))
        Return b
    End Function

    <Extension()> Public Function left_shift(ByRef b As Int32, ByVal moves As Byte) As Int32
        b = uint32_int32(left_shift(int32_uint32(b), moves))
        Return b
    End Function

    <Extension()> Public Function right_shift(ByRef b As UInt64, ByVal moves As Byte) As UInt64
        valid_shift_moves(Of UInt64)(moves)
        If moves > 0 AndAlso moves < sizeof(Of UInt64)() * bit_count_in_byte Then
            b = (b >> moves) +
                ((b And ((uint64_1 << moves) - uint64_1)) << (bitcount_in_uint64 - moves))
        End If
        Return b
    End Function

    <Extension()> Public Function left_shift(ByRef b As UInt64, ByVal moves As Byte) As UInt64
        valid_shift_moves(Of UInt64)(moves)
        If moves > 0 AndAlso moves < sizeof(Of UInt64)() * bit_count_in_byte Then
            b = (b << moves) +
                ((b And (((uint64_1 << moves) - uint64_1) << (bitcount_in_uint64 - moves))) _
                    >> (bitcount_in_uint64 - moves))
        End If
        Return b
    End Function

    <Extension()> Public Function right_shift(ByRef b As Int64, ByVal moves As Byte) As Int64
        b = uint64_int64(right_shift(int64_uint64(b), moves))
        Return b
    End Function

    <Extension()> Public Function left_shift(ByRef b As Int64, ByVal moves As Byte) As Int64
        b = uint64_int64(left_shift(int64_uint64(b), moves))
        Return b
    End Function
End Module
