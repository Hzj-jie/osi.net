
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt

Public Class to_uint32_test
    Inherits [case]

    Private Shared Function _byte_to_uint32_case() As Boolean
        Dim x As _byte_to_uint32 = Nothing
        x = New _byte_to_uint32()
        For i As UInt16 = min_uint8 To max_uint8
            assert_equal(x.reverse(x(CByte(i))), i)
        Next
        Return True
    End Function

    Private Shared Function _int16_to_uint32_case() As Boolean
        Dim x As _int16_to_uint32 = Nothing
        x = New _int16_to_uint32()
        For i As Int32 = min_int16 To max_int16
            assert_equal(x.reverse(x(CShort(i))), i)
        Next
        Return True
    End Function

    Private Shared Function _uint16_to_uint32_case() As Boolean
        Dim x As _uint16_to_uint32 = Nothing
        x = New _uint16_to_uint32()
        For i As UInt32 = min_uint16 To max_uint16
            assert_equal(x.reverse(x(CUShort(i))), i)
        Next
        Return True
    End Function

    Private Shared Function _int64_to_uint32_case() As Boolean
        Dim x As _int64_to_uint32 = Nothing
        x = New _int64_to_uint32()
        x.reverse(x(min_int64))
        x.reverse(x(0))
        x.reverse(x(max_int64))
        Return True
    End Function

    Private Shared Function _uint32_to_uint32_case() As Boolean
        Dim x As _uint32_to_uint32 = Nothing
        x = New _uint32_to_uint32()
        assert_equal(x.reverse(x(min_uint32)), min_uint32)
        assert_equal(x.reverse(x(uint32_0)), uint32_0)
        assert_equal(x.reverse(x(max_uint32)), max_uint32)
        Return True
    End Function

    Private Shared Function _int32_to_uint32_case() As Boolean
        Dim x As _int32_to_uint32 = Nothing
        x = New _int32_to_uint32()
        assert_equal(x.reverse(x(min_int32)), min_int32)
        assert_equal(x.reverse(x(0)), 0)
        assert_equal(x.reverse(x(max_int32)), max_int32)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return _byte_to_uint32_case() AndAlso
               _int16_to_uint32_case() AndAlso
               _uint16_to_uint32_case() AndAlso
               _int64_to_uint32_case() AndAlso
               _uint32_to_uint32_case() AndAlso
               _int32_to_uint32_case()
    End Function
End Class
