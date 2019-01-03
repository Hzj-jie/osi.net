
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt

Public Class endian_test
    Inherits [case]

    Private Shared ReadOnly int16_cases() As pair(Of Int16, Int16) = {
        make_pair(Of Int16, Int16)(&HABCDS, &HCDABS),
        make_pair(Of Int16, Int16)(&H1234S, &H3412S)
    }

    Private Shared ReadOnly uint16_cases() As pair(Of UInt16, UInt16) = {
        make_pair(Of UInt16, UInt16)(&HABCDUS, &HCDABUS),
        make_pair(Of UInt16, UInt16)(&H1234US, &H3412US)
    }

    Private Shared ReadOnly int64_cases() As pair(Of Int64, Int64) = {
        make_pair(Of Int64, Int64)(&H1234567890ABCDEFL, &HEFCDAB9078563412L),
        make_pair(Of Int64, Int64)(&H6789234510BCDEFAL, &HFADEBC1045238967L)
    }

    Private Shared ReadOnly uint64_cases() As pair(Of UInt64, UInt64) = {
        make_pair(Of UInt64, UInt64)(&H1234567890ABCDEFUL, &HEFCDAB9078563412UL),
        make_pair(Of UInt64, UInt64)(&H6789234510BCDEFAUL, &HFADEBC1045238967UL)
    }

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To array_size_i(int16_cases) - 1
            assertion.equal(endian.reverse(int16_cases(i).first), int16_cases(i).second)
            assertion.equal(endian.reverse(int16_cases(i).second), int16_cases(i).first)

            assertion.equal(endian.reverse(Of Int16)(int16_cases(i).first), int16_cases(i).second)
            assertion.equal(endian.reverse(Of Int16)(int16_cases(i).second), int16_cases(i).first)
        Next
        For i As Int32 = 0 To array_size_i(uint16_cases) - 1
            assertion.equal(endian.reverse(uint16_cases(i).first), uint16_cases(i).second)
            assertion.equal(endian.reverse(uint16_cases(i).second), uint16_cases(i).first)

            assertion.equal(endian.reverse(Of UInt16)(uint16_cases(i).first), uint16_cases(i).second)
            assertion.equal(endian.reverse(Of UInt16)(uint16_cases(i).second), uint16_cases(i).first)
        Next
        For i As Int32 = 0 To array_size_i(int64_cases) - 1
            assertion.equal(endian.reverse(int64_cases(i).first), int64_cases(i).second)
            assertion.equal(endian.reverse(int64_cases(i).second), int64_cases(i).first)

            assertion.equal(endian.reverse(Of Int64)(int64_cases(i).first), int64_cases(i).second)
            assertion.equal(endian.reverse(Of Int64)(int64_cases(i).second), int64_cases(i).first)
        Next
        For i As Int32 = 0 To array_size_i(uint64_cases) - 1
            assertion.equal(endian.reverse(uint64_cases(i).first), uint64_cases(i).second)
            assertion.equal(endian.reverse(uint64_cases(i).second), uint64_cases(i).first)

            assertion.equal(endian.reverse(Of UInt64)(uint64_cases(i).first), uint64_cases(i).second)
            assertion.equal(endian.reverse(Of UInt64)(uint64_cases(i).second), uint64_cases(i).first)
        Next
        Return True
    End Function
End Class
