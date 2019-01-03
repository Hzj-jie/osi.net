
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt

Public Class non_integer_overflow_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        assertion.equal(int8_uint8(-1), max_uint8)
        assertion.equal(int8_uint8(min_int8), max_int8 + 1)
        assertion.equal(uint8_int8(max_uint8), -1)
        assertion.equal(uint8_int8(max_int8 + 1), min_int8)
        assertion.equal(int16_uint16(-1), max_uint16)
        assertion.equal(int16_uint16(min_int16), max_int16 + 1)
        assertion.equal(uint16_int16(max_uint16), -1)
        assertion.equal(uint16_int16(max_int16 + 1), min_int16)
        assertion.equal(int32_uint32(-1), max_uint32)
        assertion.equal(int32_uint32(min_int32), CUInt(max_int32) + 1)
        assertion.equal(uint32_int32(max_uint32), -1)
        assertion.equal(uint32_int32(CUInt(max_int32) + 1), min_int32)
        assertion.equal(int64_uint64(-1), max_uint64)
        assertion.equal(int64_uint64(min_int64), CULng(max_int64) + 1)
        assertion.equal(uint64_int64(max_uint64), -1)
        assertion.equal(uint64_int64(CULng(max_int64) + 1), min_int64)

        For i As Int32 = 0 To 1024 * 32 - 1
            Dim a As SByte = 0
            a = CSByte(rnd_int(min_int8, max_int8 + 1))
            assertion.equal(uint8_int8(int8_uint8(a)), a)
            Dim b As Int16 = 0
            b = CShort(rnd_int(min_int16, max_int16 + 1))
            assertion.equal(uint16_int16(int16_uint16(b)), b)
            Dim c As Int32 = 0
            c = CInt(rnd_int64(min_int32, CLng(max_int32) + 1))
            assertion.equal(uint32_int32(int32_uint32(b)), b)
            Dim d As Int64 = 0
            'did not cover maxInt64, which was covered before
            d = CLng(rnd_int64(min_int64, max_int64))
            assertion.equal(uint64_int64(int64_uint64(d)), d)
        Next

        Return True
    End Function
End Class
