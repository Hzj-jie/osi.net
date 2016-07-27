
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt

Public Class bit_test
    Inherits [case]

    Private Shared Function bytes_str(ByVal buff() As Byte) As String
        Dim r As String = Nothing
        For i As Int32 = 0 To array_size(buff) - 1
            Dim b As Byte = 0
            b = buff(i)
            For j As Int32 = 0 To bit_count_in_byte - 1
                r += If(b > max_int8, character._1, character._0)
                b <<= 1
            Next
        Next
        Return r
    End Function

    Private Shared Function bit_num_case() As Boolean
        Dim buff() As Byte = Nothing
        Do
            Dim a As Int64 = 0
            buff = int64_bytes(max_int64)
            assert_true(bytes_str(buff).bit_int64(a))
            assert_equal(a, max_int64)

            Dim b As UInt64 = 0
            buff = uint64_bytes(max_uint64)
            assert_true(bytes_str(buff).bit_uint64(b))
            assert_equal(b, max_uint64)
        Loop While False

        For i As Int32 = 0 To 1024 * 32 - 1
            Dim a As SByte = 0
            Dim aa As SByte = 0
            a = rnd_int(min_int8, max_int8 + 1)
            buff = int8_bytes(a)
            assert_true(bytes_str(buff).bit_int8(aa))
            assert_equal(a, aa)

            Dim b As Byte = 0
            Dim bb As Byte = 0
            b = rnd_int(0, max_uint8 + 1)
            buff = uint8_bytes(b)
            assert_true(bytes_str(buff).bit_uint8(bb))
            assert_equal(b, bb)

            Dim c As Int16 = 0
            Dim cc As Int16 = 0
            c = rnd_int(min_int16, max_int16 + 1)
            buff = int16_bytes(c)
            assert_true(bytes_str(buff).bit_int16(cc))
            assert_equal(c, cc)

            Dim d As UInt16 = 0
            Dim dd As UInt16 = 0
            d = rnd_int(0, max_uint16 + 1)
            buff = uint16_bytes(d)
            assert_true(bytes_str(buff).bit_uint16(dd))
            assert_equal(d, dd)

            Dim e As Int32 = 0
            Dim ee As Int32 = 0
            e = rnd_int64(min_int32, CLng(max_int32) + 1)
            buff = int32_bytes(e)
            assert_true(bytes_str(buff).bit_int32(ee))
            assert_equal(e, ee)

            Dim f As UInt32 = 0
            Dim ff As UInt32 = 0
            f = rnd_int64(0, CLng(max_uint32) + 1)
            buff = uint32_bytes(f)
            assert_true(bytes_str(buff).bit_uint32(ff))
            assert_equal(f, ff)

            Dim g As Int64 = 0
            Dim gg As Int64 = 0
            'maxInt64 is covered above
            g = rnd_int64(min_int64, max_int64)
            buff = int64_bytes(g)
            assert_true(bytes_str(buff).bit_int64(gg))
            assert_equal(g, gg)

            Dim h As UInt64 = 0
            Dim hh As UInt64 = 0
            'maxUInt64 is covered above
            h = rnd_uint64(0, max_uint64)
            buff = uint64_bytes(h)
            assert_true(bytes_str(buff).bit_uint64(hh))
            assert_equal(h, hh)
        Next

        assert_false("2".bit_int8(Nothing))
        assert_false("2".bit_uint8(Nothing))
        assert_false("2".bit_int16(Nothing))
        assert_false("2".bit_uint16(Nothing))
        assert_false("2".bit_int32(Nothing))
        assert_false("2".bit_uint32(Nothing))
        assert_false("2".bit_int64(Nothing))
        assert_false("2".bit_uint64(Nothing))

        assert_false("1111111".bit_int8(Nothing))
        assert_false("11111112".bit_int8(Nothing))
        assert_false("111111111".bit_int8(Nothing))
        assert_false("1111111111111111".bit_int8(Nothing))

        assert_false("1111111".bit_uint8(Nothing))
        assert_false("11111112".bit_uint8(Nothing))
        assert_false("111111111".bit_uint8(Nothing))
        assert_false("1111111111111111".bit_uint8(Nothing))

        assert_false("111111111111111".bit_int16(Nothing))
        assert_false("1111111111111112".bit_int16(Nothing))
        assert_false("11111111111111111".bit_int16(Nothing))
        assert_false("11111111111111111111111111111111".bit_int16(Nothing))

        assert_false("111111111111111".bit_uint16(Nothing))
        assert_false("1111111111111112".bit_uint16(Nothing))
        assert_false("11111111111111111".bit_uint16(Nothing))
        assert_false("11111111111111111111111111111111".bit_uint16(Nothing))

        assert_false("1111111111111111111111111111111".bit_int32(Nothing))
        assert_false("11111111111111111111111111111112".bit_int32(Nothing))
        assert_false("111111111111111111111111111111111".bit_int32(Nothing))
        assert_false("1111111111111111111111111111111111111111111111111111111111111111".bit_int32(Nothing))

        assert_false("1111111111111111111111111111111".bit_uint32(Nothing))
        assert_false("11111111111111111111111111111112".bit_uint32(Nothing))
        assert_false("111111111111111111111111111111111".bit_uint32(Nothing))
        assert_false("1111111111111111111111111111111111111111111111111111111111111111".bit_uint32(Nothing))

        assert_false("111111111111111111111111111111111111111111111111111111111111111".bit_int64(Nothing))
        assert_false("1111111111111111111111111111111111111111111111111111111111111112".bit_int64(Nothing))
        assert_false("11111111111111111111111111111111111111111111111111111111111111111".bit_int64(Nothing))
        assert_false(strcat("1111111111111111111111111111111111111111111111111111111111111111",
                            "1111111111111111111111111111111111111111111111111111111111111111").bit_int64(Nothing))

        assert_false("111111111111111111111111111111111111111111111111111111111111111".bit_uint64(Nothing))
        assert_false("1111111111111111111111111111111111111111111111111111111111111112".bit_uint64(Nothing))
        assert_false("11111111111111111111111111111111111111111111111111111111111111111".bit_uint64(Nothing))
        assert_false(strcat("1111111111111111111111111111111111111111111111111111111111111111",
                            "1111111111111111111111111111111111111111111111111111111111111111").bit_int64(Nothing))
        Return True
    End Function

    Private Shared Function _1count_case() As Boolean
        For i As Int32 = sizeof_int8 \ sizeof_int8 To sizeof_int64 \ sizeof_int8
            For j As Int32 = 0 To 1024 * 32 - 1
                Dim s As String = Nothing
                Dim c As Byte = 0
                For k As Int32 = 0 To i - 1
                    For l As Int32 = 0 To bit_count_in_byte - 1
                        Dim v As Boolean = False
                        v = rnd_bool()
                        s += If(v, character._0, character._1)
                        c += If(v, 0, 1)
                    Next
                Next
                If i = 1 Then
                    Dim a As SByte = 0
                    Dim b As Byte = 0
                    assert_true(s.bit_int8(a))
                    assert_equal(a._1count(), c)
                    assert_true(s.bit_uint8(b))
                    assert_equal(b._1count(), c)
                ElseIf i = 2 Then
                    Dim a As Int16 = 0
                    Dim b As UInt16 = 0
                    assert_true(s.bit_int16(a))
                    assert_equal(a._1count(), c)
                    assert_true(s.bit_uint16(b))
                    assert_equal(b._1count(), c)
                ElseIf i = 4 Then
                    Dim a As Int32 = 0
                    Dim b As UInt32 = 0
                    assert_true(s.bit_int32(a))
                    assert_equal(a._1count(), c)
                    assert_true(s.bit_uint32(b))
                    assert_equal(b._1count(), c)
                ElseIf i = 8 Then
                    Dim a As Int64 = 0
                    Dim b As UInt64 = 0
                    assert_true(s.bit_int64(a))
                    assert_equal(a._1count(), c)
                    assert_true(s.bit_uint64(b))
                    assert_equal(b._1count(), c)
                End If
            Next
        Next

        Return True
    End Function

    Private Shared Function setbit_case_byte() As Boolean
        Dim i As Byte = 0
        For j As Int32 = 0 To bit_count_in_byte * sizeof_uint8 - 1
            i.setbit(j, True)
            assert_true(i.getbit(j))
            assert_equal(i._1count(), 1)
            i.setbit(j, False)
            assert_false(i.getbit(j))
            assert_equal(i._1count(), 0)
        Next
        Return True
    End Function

    Private Shared Function setbit_case_sbyte() As Boolean
        Dim i As SByte = 0
        For j As Int32 = 0 To bit_count_in_byte * sizeof_int8 - 1
            i.setbit(j, True)
            assert_true(i.getbit(j))
            assert_equal(i._1count(), 1)
            i.setbit(j, False)
            assert_false(i.getbit(j))
            assert_equal(i._1count(), 0)
        Next
        Return True
    End Function

    Private Shared Function setbit_case_uint16() As Boolean
        Dim i As UInt16 = 0
        For j As Int32 = 0 To bit_count_in_byte * sizeof_uint16 - 1
            i.setbit(j, True)
            assert_true(i.getbit(j))
            assert_equal(i._1count(), 1)
            i.setbit(j, False)
            assert_false(i.getbit(j))
            assert_equal(i._1count(), 0)
        Next
        Return True
    End Function

    Private Shared Function setbit_case_int16() As Boolean
        Dim i As Int16 = 0
        For j As Int32 = 0 To bit_count_in_byte * sizeof_int16 - 1
            i.setbit(j, True)
            assert_true(i.getbit(j))
            assert_equal(i._1count(), 1)
            i.setbit(j, False)
            assert_false(i.getbit(j))
            assert_equal(i._1count(), 0)
        Next
        Return True
    End Function

    Private Shared Function setbit_case_uint32() As Boolean
        Dim i As UInt32 = 0
        For j As Int32 = 0 To bit_count_in_byte * sizeof_uint32 - 1
            i.setbit(j, True)
            assert_true(i.getbit(j))
            assert_equal(i._1count(), 1)
            i.setbit(j, False)
            assert_false(i.getbit(j))
            assert_equal(i._1count(), 0)
        Next
        Return True
    End Function

    Private Shared Function setbit_case_int32() As Boolean
        Dim i As Int32 = 0
        For j As Int32 = 0 To bit_count_in_byte * sizeof_int32 - 1
            i.setbit(j, True)
            assert_true(i.getbit(j))
            assert_equal(i._1count(), 1)
            i.setbit(j, False)
            assert_false(i.getbit(j))
            assert_equal(i._1count(), 0)
        Next
        Return True
    End Function

    Private Shared Function setbit_case_uint64() As Boolean
        Dim i As UInt64 = 0
        For j As Int32 = 0 To bit_count_in_byte * sizeof_uint64 - 1
            i.setbit(j, True)
            assert_true(i.getbit(j))
            assert_equal(i._1count(), 1)
            i.setbit(j, False)
            assert_false(i.getbit(j))
            assert_equal(i._1count(), 0)
        Next
        Return True
    End Function

    Private Shared Function setbit_case_int64() As Boolean
        Dim i As Int64 = 0
        For j As Int32 = 0 To bit_count_in_byte * sizeof_int64 - 1
            i.setbit(j, True)
            assert_true(i.getbit(j))
            assert_equal(i._1count(), 1)
            i.setbit(j, False)
            assert_false(i.getbit(j))
            assert_equal(i._1count(), 0)
        Next
        Return True
    End Function

    Private Shared Function setbit_case() As Boolean
        Return setbit_case_byte() AndAlso
               setbit_case_sbyte() AndAlso
               setbit_case_uint16() AndAlso
               setbit_case_int16() AndAlso
               setbit_case_uint32() AndAlso
               setbit_case_int32() AndAlso
               setbit_case_uint64() AndAlso
               setbit_case_int64()
    End Function

    Private Shared Function setrbit_case_byte() As Boolean
        Dim i As Byte = 0
        For j As Int32 = 0 To bit_count_in_byte * sizeof_uint8 - 1
            i.setrbit(j, True)
            assert_true(i.getrbit(j))
            assert_equal(i._1count(), 1)
            i.setrbit(j, False)
            assert_false(i.getrbit(j))
            assert_equal(i._1count(), 0)
        Next
        Return True
    End Function

    Private Shared Function setrbit_case_sbyte() As Boolean
        Dim i As SByte = 0
        For j As Int32 = 0 To bit_count_in_byte * sizeof_int8 - 1
            i.setrbit(j, True)
            assert_true(i.getrbit(j))
            assert_equal(i._1count(), 1)
            i.setrbit(j, False)
            assert_false(i.getrbit(j))
            assert_equal(i._1count(), 0)
        Next
        Return True
    End Function

    Private Shared Function setrbit_case_uint16() As Boolean
        Dim i As UInt16 = 0
        For j As Int32 = 0 To bit_count_in_byte * sizeof_uint16 - 1
            i.setrbit(j, True)
            assert_true(i.getrbit(j))
            assert_equal(i._1count(), 1)
            i.setrbit(j, False)
            assert_false(i.getrbit(j))
            assert_equal(i._1count(), 0)
        Next
        Return True
    End Function

    Private Shared Function setrbit_case_int16() As Boolean
        Dim i As Int16 = 0
        For j As Int32 = 0 To bit_count_in_byte * sizeof_int16 - 1
            i.setrbit(j, True)
            assert_true(i.getrbit(j))
            assert_equal(i._1count(), 1)
            i.setrbit(j, False)
            assert_false(i.getrbit(j))
            assert_equal(i._1count(), 0)
        Next
        Return True
    End Function

    Private Shared Function setrbit_case_uint32() As Boolean
        Dim i As UInt32 = 0
        For j As Int32 = 0 To bit_count_in_byte * sizeof_uint32 - 1
            i.setrbit(j, True)
            assert_true(i.getrbit(j))
            assert_equal(i._1count(), 1)
            i.setrbit(j, False)
            assert_false(i.getrbit(j))
            assert_equal(i._1count(), 0)
        Next
        Return True
    End Function

    Private Shared Function setrbit_case_int32() As Boolean
        Dim i As Int32 = 0
        For j As Int32 = 0 To bit_count_in_byte * sizeof_int32 - 1
            i.setrbit(j, True)
            assert_true(i.getrbit(j))
            assert_equal(i._1count(), 1)
            i.setrbit(j, False)
            assert_false(i.getrbit(j))
            assert_equal(i._1count(), 0)
        Next
        Return True
    End Function

    Private Shared Function setrbit_case_uint64() As Boolean
        Dim i As UInt64 = 0
        For j As Int32 = 0 To bit_count_in_byte * sizeof_uint64 - 1
            i.setrbit(j, True)
            assert_true(i.getrbit(j))
            assert_equal(i._1count(), 1)
            i.setrbit(j, False)
            assert_false(i.getrbit(j))
            assert_equal(i._1count(), 0)
        Next
        Return True
    End Function

    Private Shared Function setrbit_case_int64() As Boolean
        Dim i As Int64 = 0
        For j As Int32 = 0 To bit_count_in_byte * sizeof_int64 - 1
            i.setrbit(j, True)
            assert_true(i.getrbit(j))
            assert_equal(i._1count(), 1)
            i.setrbit(j, False)
            assert_false(i.getrbit(j))
            assert_equal(i._1count(), 0)
        Next
        Return True
    End Function

    Private Shared Function setrbit_case() As Boolean
        Return setrbit_case_byte() AndAlso
               setrbit_case_sbyte() AndAlso
               setrbit_case_uint16() AndAlso
               setrbit_case_int16() AndAlso
               setrbit_case_uint32() AndAlso
               setrbit_case_int32() AndAlso
               setrbit_case_uint64() AndAlso
               setrbit_case_int64()
    End Function

    Public Overrides Function run() As Boolean
        Return bit_num_case() AndAlso
               _1count_case() AndAlso
               setbit_case() AndAlso
               setrbit_case()
    End Function
End Class
