﻿
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt

Public Class byte_test
    Inherits [case]

    Private Shared Function make_hex_str_case(ByVal b() As Byte, ByVal s As String) As pair(Of Byte(), String)
        Return emplace_make_pair(b, s)
    End Function

    Private Shared ReadOnly hex_str_cases() As pair(Of Byte(), String) = {
        make_hex_str_case({}, Nothing),
        make_hex_str_case({10}, "0A"),
        make_hex_str_case({10, 11, 12}, "0A0B0C"),
        make_hex_str_case({1, 2, 3, 4, 5, 6, 7, 8, 9, 10}, "0102030405060708090A"),
        make_hex_str_case({0, 15, 16, 17, 127, 128, 129, 254, 255}, "000F10117F8081FEFF")}

    Private Shared Function hex_str_case() As Boolean
        For i As UInt32 = 0 To array_size(hex_str_cases) - uint32_1
            assert_equal(hex_str_cases(i).first.hex_str(), hex_str_cases(i).second)
        Next
        Return True
    End Function

    Private Shared ReadOnly hex_bytes_failure_cases() As String = {
        "9ABCDEF",
        "ABCDEFGH",
        "AABBCCDDEEFFGG"}

    Private Shared Function hex_bytes_case() As Boolean
        For i As UInt32 = 0 To array_size(hex_str_cases) - uint32_1
            Dim b() As Byte = Nothing
            b = hex_str_cases(i).second.hex_bytes_buff()
            assert_true(hex_str_cases(i).second.hex_bytes(b))
            assert_array_equal(b, hex_str_cases(i).first)
        Next

        For i As UInt32 = 0 To array_size(hex_bytes_failure_cases) - uint32_1
            Dim b() As Byte = Nothing
            b = hex_bytes_failure_cases(i).hex_bytes_buff()
            assert_false(hex_bytes_failure_cases(i).hex_bytes(b))
        Next
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return hex_str_case() AndAlso
               hex_bytes_case()
    End Function
End Class
