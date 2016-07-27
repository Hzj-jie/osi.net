
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils

Public Class fake_rnd_test
    Inherits [case]

    Private Shared Sub rnd_min_max(ByRef min As UInt32, ByRef max As UInt32)
        min = rnd_uint(min_uint32, max_uint32)
        assert(min < max_uint32)
        If rnd_bool() Then
            max = rnd_uint(min_uint32, max_uint32)
        ElseIf rnd_bool() Then
            max = min
        Else
            max = min + 1
        End If
        If min > max Then
            swap(min, max)
        End If
    End Sub

    Private Shared Sub rnd_min_max(ByRef min As UInt32, ByRef max As UInt32, ByVal max_value As UInt32)
        assert(max_value < (max_uint32 >> 1))
        rnd_min_max(min, max)
        min = min Mod max_value
        max = max Mod max_value
        If min > max Then
            max += max_value
        End If
    End Sub

    Private Shared Function fake_rnd_uint_case() As Boolean
        Dim j As UInt32 = 0
        j = fake_rnd_uint()
        assert_true(is_fake_rnd_uint(j))
        Return True
    End Function

    Private Shared Function fake_rnd_en_chars_case() As Boolean
        Dim l As UInt32 = 0
        l = rnd_int(0, 128 + 1)
        Dim s As String = Nothing
        s = fake_rnd_en_chars(l)
        assert_equal(strlen(s), l)
        assert_true(is_fake_rnd_en_chars(s))
        Return True
    End Function

    Private Shared Function fake_next_bytes_case() As Boolean
        Dim l As UInt32 = 0
        l = rnd_int(0, 128 + 1)
        Dim b() As Byte = Nothing
        b = fake_next_bytes(l)
        assert_equal(array_size(b), l)
        assert_true(is_fake_next_bytes(b))
        Return True
    End Function

    Private Shared Function fake_rnd_uint_case2() As Boolean
        Dim min As UInt32 = 0
        Dim max As UInt32 = 0
        rnd_min_max(min, max)
        Dim seed As UInt32 = 0
        seed = rnd_uint(min_uint32, max_uint32)
        Dim r As UInt32 = 0
        r = fake_rnd_uint(min, max, seed)
        If min < max Then
            assert_less(r, max)
            assert_more_or_equal(r, min)
        Else
            assert_equal(r, min)
        End If
        assert_true(is_fake_rnd_uint(r, min, max, seed))
        Return True
    End Function

    Private Shared Function fake_next_bytes_case2() As Boolean
        Dim min As UInt32 = 0
        Dim max As UInt32 = 0
        rnd_min_max(min, max, 1024)
        Dim seed As String = Nothing
        seed = guid_str()
        Dim r() As Byte = Nothing
        r = fake_next_bytes(seed, min, max)
        If min < max Then
            assert_less(array_size(r), max)
            assert_more_or_equal(array_size(r), min)
        Else
            assert_equal(array_size(r), min)
        End If
        assert_true(is_fake_next_bytes(seed, r, min, max))
        Return True
    End Function

    Private Shared Function fake_next_bytes_chained_case() As Boolean
        Dim min As UInt32 = 0
        Dim max As UInt32 = 0
        rnd_min_max(min, max, 1024)
        If min = 0 Then
            min = 1
            max += 1
        End If
        Dim seed As String = Nothing
        seed = guid_str()
        Dim r() As Byte = Nothing
        For i As Int32 = rnd_int(2, 16 + 1) - 1 To 0 Step -1
            Dim t() As Byte = Nothing
            t = fake_next_bytes(seed, min, max)
            If assert_more(array_size(t), uint32_0) Then
                ReDim Preserve r(array_size(r) + array_size(t) - 1)
                memcpy(r, array_size(r) - array_size(t), t)
            End If
        Next
        assert_true(is_fake_next_bytes_chained(seed, r, min, max))
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Dim r As Boolean = False
        For i As Int32 = 0 To 409600 - 1
            If Not fake_rnd_uint_case() OrElse
               Not fake_rnd_en_chars_case() OrElse
               Not fake_next_bytes_case() OrElse
               Not fake_rnd_uint_case2() OrElse
               Not fake_next_bytes_case2() OrElse
               Not fake_next_bytes_chained_case() Then
                Return False
            End If
        Next
        Return True
    End Function
End Class
