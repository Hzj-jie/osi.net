
Imports osi.root.constants
Imports System.Runtime.CompilerServices

Public Module _shift
    Private Sub valid_shift_moves(Of T)(ByVal moves As Byte)
        If isdebugmode() Then
            assert(moves < sizeof(Of T)() * bit_count_in_byte,
                     "cannot shift a variable over its size * ", bit_count_in_byte)
        End If
    End Sub

    Private ReadOnly bitcount_in_uint32 As Byte = sizeof_int32 * bit_count_in_byte

    <Extension()> Public Function right_shift(ByRef b As UInt32, ByVal moves As Byte) As UInt32
        valid_shift_moves(Of UInt32)(moves)
        If moves > 0 Then
            b = (b >> moves) +
                ((b And ((uint64_1 << moves) - 1)) << (bitcount_in_uint32 - moves))
        End If
        Return b
    End Function

    <Extension()> Public Function left_shift(ByRef b As UInt32, ByVal moves As Byte) As UInt32
        valid_shift_moves(Of UInt32)(moves)
        If moves > 0 Then
            b = (b << moves) +
                ((b And (((uint64_1 << moves) - 1) << (bitcount_in_uint32 - moves))) >> (bitcount_in_uint32 - moves))
        End If
        Return b
    End Function
End Module
