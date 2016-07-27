
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt

Public Class npos_uint_test
    Inherits [case]

    Private Shared ReadOnly compare_cases() As Int64 =
        {min_int32, max_int32, min_uint32, max_uint32, int32_0, int32_1, uint32_0, uint32_1}

    Private Shared Function compare(ByVal x As npos_uint, ByVal y As Int64) As Boolean
        assert(y <= max_uint32)
        assert(y >= min_int32)
        If y < 0 Then
            Dim z As npos_uint = Nothing
            z = New npos_uint(CInt(y))
            assert_true(x = z)
            assert_false(x <> z)
            assert_true(x <= z)
            assert_true(x >= z)
            assert_false(x < z)
            assert_false(x > z)

            assert_true(x.npos())
            assert_true(x.infinite())
            Dim d As Int32 = 0
            d = x
            assert_equal(d, npos)

            assert_true(x > max_uint32)
            assert_true(x >= max_uint32)
            assert_false(x > min_int32)
            assert_true(x >= min_int32)
            assert_true(max_uint32 < x)
            assert_true(max_uint32 <= x)
            assert_false(min_int32 < x)
            assert_true(min_int32 <= x)
        Else
            Dim z As npos_uint = Nothing
            z = New npos_uint(CUInt(y))
            assert_true(x = z)
            assert_false(x <> z)
            assert_true(x <= z)
            assert_true(x >= z)
            assert_false(x < z)
            assert_false(x > z)

            assert_false(x.npos())
            assert_false(x.infinite())
            Dim u As UInt32 = 0
            u = x
            assert_equal(u, CUInt(y))
            If y <= max_int32 Then
                Dim d As Int32 = 0
                d = x
                assert_equal(d, CInt(y))
            End If

            assert_false(x > max_uint32)
            If y = max_uint32 Then
                assert_true(x >= max_uint32)
            Else
                assert_false(x >= max_uint32)
            End If
            assert_false(x > min_int32)
            assert_false(x >= min_int32)
            assert_false(max_uint32 < x)
            If y = max_uint32 Then
                assert_true(max_uint32 <= x)
            Else
                assert_false(max_uint32 <= x)
            End If
            assert_false(min_int32 < x)
            assert_false(min_int32 <= x)
        End If
        Return True
    End Function

    Private Shared Function compare(ByVal x As Int64) As Boolean
        assert(x <= max_uint32)
        assert(x >= min_int32)
        Return compare(If(x < 0, New npos_uint(CInt(x)), New npos_uint(CUInt(x))), x)
    End Function

    Private Shared Function allocate_all() As Boolean
        Dim x As npos_uint = Nothing
        For i As Int32 = min_int32 To max_int32 - 1
            x = New npos_uint(i)
        Next
        x = New npos_uint(max_int32)
        For i As UInt32 = CUInt(max_int32) + uint32_1 To max_uint32 - uint32_1
            x = New npos_uint(i)
        Next
        x = New npos_uint(max_uint32)
        Return True
    End Function

    Private Shared Function compare_case() As Boolean
        For i As UInt32 = 0 To array_size(compare_cases) - uint32_1
            If Not compare(compare_cases(i)) Then
                Return False
            End If
        Next
        For i As UInt32 = 0 To 1024 - 1
            If Not compare(rnd_uint()) OrElse
               Not compare(rnd_int()) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return allocate_all() AndAlso
               compare_case()
    End Function
End Class
