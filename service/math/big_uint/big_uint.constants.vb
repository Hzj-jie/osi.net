
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class big_uint
    Private Enum bit_wise_operator
        [and]
        [or]
        [xor]
    End Enum

    Private Const default_str_base As Byte = 10
    Private Const max_uint32_plus_1 As Int64 = max_uint32 + 1
    Private Shared ReadOnly support_str_base As Byte
    Private Shared ReadOnly byte_count_in_uint32 As Byte
    Private Shared ReadOnly bit_count_in_uint32 As Byte
    Private Shared ReadOnly bit_count_in_uint32_shift As Byte
    Private Shared ReadOnly bit_count_in_uint32_mask As Byte
    Private Shared ReadOnly digits() As Char
    Private Shared ReadOnly digit_0 As Char
    Private Shared ReadOnly chars() As Int16
    Private Shared ReadOnly digit_count_per_parse() As Byte

    Shared Sub New()
        byte_count_in_uint32 = CByte(sizeof_uint32 \ sizeof_int8)
        bit_count_in_uint32 = bit_count_in_byte * byte_count_in_uint32
        assert(bit_count_in_uint32.power_of_2())
        bit_count_in_uint32_mask = 1
        bit_count_in_uint32_shift = 0
        While bit_count_in_uint32_mask < bit_count_in_uint32
            bit_count_in_uint32_shift += CByte(1)
            bit_count_in_uint32_mask <<= 1
        End While
        assert(bit_count_in_uint32_mask = bit_count_in_uint32)
        bit_count_in_uint32_mask -= CByte(1)

        digits = (dbc_digits + upper_english_characters + lower_english_characters).c_str()
        support_str_base = CByte(array_size(digits))
        ReDim digit_count_per_parse(support_str_base)
        For i As Byte = 0 To support_str_base
            If support_base(i) Then
                assert(i > 0)
                digit_count_per_parse(i) = CByte(System.Math.Floor(bit_count_in_uint32 /
                                                                   (System.Math.Log(i) / System.Math.Log(2))))
            End If
        Next
        digit_0 = digits(0)

        ReDim chars(Convert.ToInt32(Char.MaxValue) - Convert.ToInt32(Char.MinValue))
        arrays.fill(chars, npos)
        For i As Int32 = 0 To array_size_i(digits) - 1
            chars(Convert.ToInt32(digits(i))) = CShort(i)
        Next
    End Sub
End Class
