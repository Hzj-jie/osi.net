
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt

Public Class byte_test
    Inherits [case]

    Private Shared Function make_hex_str_case(ByVal b() As Byte, ByVal s As String) As pair(Of Byte(), String)
        Return pair.emplace_of(b, s)
    End Function

    Private Shared ReadOnly hex_str_cases() As pair(Of Byte(), String) = {
        make_hex_str_case({}, ""),
        make_hex_str_case({10}, "0A"),
        make_hex_str_case({10, 11, 12}, "0A0B0C"),
        make_hex_str_case({1, 2, 3, 4, 5, 6, 7, 8, 9, 10}, "0102030405060708090A"),
        make_hex_str_case({0, 15, 16, 17, 127, 128, 129, 254, 255}, "000F10117F8081FEFF")}

    Private Shared Function hex_str_case() As Boolean
        For i As Int32 = 0 To array_size_i(hex_str_cases) - 1
            assertion.equal(hex_str_cases(i).first.hex_str(), hex_str_cases(i).second)
        Next
        assertion.equal(hex_str(uint16_0), "0000")
        assertion.equal(hex_str(uint32_0), "00000000")
        assertion.equal(hex_str(uint64_0), "0000000000000000")
        assertion.equal(hex_str(max_uint16), "FFFF")
        assertion.equal(hex_str(max_uint32), "FFFFFFFF")
        assertion.equal(hex_str(max_uint64), "FFFFFFFFFFFFFFFF")
        assertion.equal(hex_str((CUInt(&H12) << 24) + (CUInt(&H34) << 16) + (CUInt(&H56) << 8) + CUInt(&H78)), "12345678")
        assertion.equal(hex_str((CUInt(&HAB) << 24) + (CUInt(&HCD) << 16) + (CUInt(&HEF) << 8) + CUInt(&H12)), "ABCDEF12")
        assertion.equal(hex_str((CULng(&HAB) << 24) + (CULng(&HCD) << 16) + (CULng(&HEF) << 8) + CULng(&H12)),
                     "00000000ABCDEF12")
        assertion.equal(hex_str((CULng(&H87) << 24) + (CULng(&H65) << 16) + (CULng(&H43) << 8) + CULng(&HDE)),
                     "00000000876543DE")
        assertion.equal(hex_str((CULng(&H12) << 56) + (CULng(&H34) << 48) + (CULng(&H56) << 40) + (CULng(&H78) << 32) +
                             (CULng(&H9A) << 24) + (CULng(&HBC) << 16) + (CULng(&HDE) << 8) + CULng(&HF0)),
                     "123456789ABCDEF0")
        Return True
    End Function

    Private Shared ReadOnly hex_bytes_failure_cases() As String = {
        "9ABCDEF",
        "ABCDEFGH",
        "AABBCCDDEEFFGG"}

    Private Shared Function hex_bytes_case() As Boolean
        For i As Int32 = 0 To array_size_i(hex_str_cases) - 1
            Dim b() As Byte = Nothing
            b = hex_str_cases(i).second.hex_bytes_buff()
            assertion.is_true(hex_str_cases(i).second.hex_bytes(b))
            assertion.array_equal(b, hex_str_cases(i).first)
        Next

        For i As Int32 = 0 To array_size_i(hex_bytes_failure_cases) - 1
            Dim b() As Byte = Nothing
            b = hex_bytes_failure_cases(i).hex_bytes_buff()
            assertion.is_false(hex_bytes_failure_cases(i).hex_bytes(b))
        Next
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return hex_str_case() AndAlso
               hex_bytes_case()
    End Function
End Class
