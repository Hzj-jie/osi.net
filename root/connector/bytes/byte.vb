
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Text
Imports osi.root.constants

<global_init(global_init_level.runtime_checkers)>
Public Module _byte
    Private ReadOnly empty_bytes(-1) As Byte

    Private Sub init()
        assert(strlen(upper_dbc_hex_digits) = hex_digits_char_count)
        assert(hex_digits_char_count = 16)
    End Sub

    Private Function to_hex(ByVal i As Byte) As Char
        assert(i < hex_digits_char_count)
        Return upper_dbc_hex_digits(i)
    End Function

    <Extension()> Public Function hex(ByVal i As Byte) As String
        Return strcat(to_hex(i >> 4), to_hex(i And CByte(15)))
    End Function

    Public Const expected_hex_byte_length As Int32 = 2

    <Extension()> Public Function hex_byte(ByVal i As String,
                                           ByVal start As Int32,
                                           ByRef o As Byte) As Boolean
        If start < 0 OrElse
           strlen(i) < start + expected_hex_byte_length OrElse
           Not i(start).hex() OrElse
           Not i(start + 1).hex() Then
            Return False
        End If
        o = (i(start).hex_value() << 4) + i(start + 1).hex_value()
        Return True
    End Function

    <Extension()> Public Function hex_byte(ByVal i As String, ByRef o As Byte) As Boolean
        Return strlen(i) = expected_hex_byte_length AndAlso
               hex_byte(i, 0, o)
    End Function

    <Extension()> Public Function hex_byte(ByVal i As String, ByVal start As Int32) As Byte
        Dim o As Byte = 0
        assert(hex_byte(i, start, o))
        Return o
    End Function

    <Extension()> Public Function hex_byte(ByVal i As String) As Byte
        Dim o As Byte = 0
        assert(hex_byte(i, o))
        Return o
    End Function

    <Extension()> Public Function hex_str(ByVal i() As Byte,
                                          ByVal start As Int32,
                                          ByVal len As Int32) As String
        If len < 0 OrElse start < 0 OrElse array_size(i) < start + len Then
            Return Nothing
        End If
        If len = 0 Then
            Return ""
        End If
        Dim r As StringBuilder = Nothing
        r = New StringBuilder(len * expected_hex_byte_length - 1)
        For j As Int32 = start To start + len - 1
            r.Append(i(j).hex())
        Next
        Return Convert.ToString(r)
    End Function

    <Extension()> Public Function hex_str(ByVal i() As Byte,
                                          ByVal start As Int32) As String
        Return hex_str(i, start, array_size_i(i) - start)
    End Function

    <Extension()> Public Function hex_str(ByVal i() As Byte) As String
        Return hex_str(i, 0, array_size_i(i))
    End Function

    <Extension()> Public Function hex_str(ByVal i As UInt64) As String
        Return hex_str(uint64_big_endian_bytes(i))
    End Function

    <Extension()> Public Function hex_str(ByVal i As UInt32) As String
        Return hex_str(uint32_big_endian_bytes(i))
    End Function

    <Extension()> Public Function hex_str(ByVal i As UInt16) As String
        Return hex_str(uint16_big_endian_bytes(i))
    End Function

    <Extension()> Public Function hex_str(ByVal i As Char) As String
        Return hex_str(Convert.ToUInt16(i))
    End Function

    <Extension()> Public Function hex_bytes(ByVal s As String,
                                            ByVal str_start As Int32,
                                            ByVal str_len As Int32,
                                            ByVal buff() As Byte,
                                            ByVal buff_start As Int32,
                                            ByVal buff_len As Int32) As Boolean
        If str_start < 0 OrElse str_len < 0 Then
            Return False
        End If
        If buff_start < 0 OrElse buff_len < 0 Then
            Return False
        End If
        If str_len = 0 Then
            Return True
        End If
        If strlen(s) < str_start + str_len OrElse
           array_size(buff) < buff_start + buff_len Then
            Return False
        End If
        If str_len Mod expected_hex_byte_length <> 0 Then
            Return False
        End If
        If buff_len < (str_len \ expected_hex_byte_length) Then
            Return False
        End If
        For i As Int32 = str_start To str_start + str_len - 1 Step expected_hex_byte_length
            If Not s.hex_byte(i, buff(buff_start + ((i - str_start) \ expected_hex_byte_length))) Then
                Return False
            End If
        Next
        Return True
    End Function

    <Extension()> Public Function hex_bytes(ByVal s As String,
                                            ByVal str_start As Int32,
                                            ByVal str_len As Int32,
                                            ByVal buff() As Byte,
                                            ByVal buff_start As Int32) As Boolean
        Return hex_bytes(s, str_start, str_len, buff, buff_start, array_size_i(buff) - buff_start)
    End Function

    <Extension()> Public Function hex_bytes(ByVal s As String,
                                            ByVal str_start As Int32,
                                            ByVal buff() As Byte,
                                            ByVal buff_start As Int32,
                                            ByVal buff_len As Int32) As Boolean
        Return hex_bytes(s, str_start, strlen_i(s) - str_start, buff, buff_start, buff_len)
    End Function

    <Extension()> Public Function hex_bytes(ByVal s As String,
                                            ByVal str_start As Int32,
                                            ByVal buff() As Byte,
                                            ByVal buff_start As Int32) As Boolean
        Return hex_bytes(s, str_start, strlen_i(s) - str_start, buff, buff_start, array_size_i(buff) - buff_start)
    End Function

    <Extension()> Public Function hex_bytes(ByVal s As String,
                                            ByVal str_start As Int32,
                                            ByVal str_len As Int32,
                                            ByVal buff() As Byte) As Boolean
        Return hex_bytes(s, str_start, str_len, buff, 0, array_size_i(buff))
    End Function

    <Extension()> Public Function hex_bytes(ByVal s As String,
                                            ByVal buff() As Byte,
                                            ByVal buff_start As Int32,
                                            ByVal buff_len As Int32) As Boolean
        Return hex_bytes(s, 0, strlen_i(s), buff, buff_start, buff_len)
    End Function

    <Extension()> Public Function hex_bytes(ByVal s As String,
                                            ByVal str_start As Int32,
                                            ByVal buff() As Byte) As Boolean
        Return hex_bytes(s, str_start, strlen_i(s) - str_start, buff, 0, array_size_i(buff))
    End Function

    <Extension()> Public Function hex_bytes(ByVal s As String,
                                            ByVal buff() As Byte,
                                            ByVal buff_start As Int32) As Boolean
        Return hex_bytes(s, 0, strlen_i(s), buff, buff_start, array_size_i(buff) - buff_start)
    End Function

    <Extension()> Public Function hex_bytes(ByVal s As String,
                                            ByVal buff() As Byte) As Boolean
        Return hex_bytes(s, 0, strlen_i(s), buff, 0, array_size_i(buff))
    End Function

    <Extension()> Public Function hex_bytes(ByVal s As String) As Byte()
        Dim r() As Byte = Nothing
        ReDim r(hex_bytes_len(s) - 1)
        assert(hex_bytes(s, r))
        Return r
    End Function

    <Extension()> Public Function hex_bytes_len(ByVal s As String,
                                                ByVal start As Int32,
                                                ByVal len As Int32) As Int32
        If start < 0 OrElse len < 0 Then
            Return npos
        End If
        If strlen(s) < start + len Then
            Return npos
        End If
        If len Mod expected_hex_byte_length <> 0 Then
            Return npos
        End If
        Return len \ expected_hex_byte_length
    End Function

    <Extension()> Public Function hex_bytes_len(ByVal s As String,
                                                ByVal start As Int32) As Int32
        Return hex_bytes_len(s, start, strlen_i(s) - start)
    End Function

    <Extension()> Public Function hex_bytes_len(ByVal s As String) As Int32
        Return hex_bytes_len(s, 0, strlen_i(s))
    End Function

    <Extension()> Public Function hex_bytes_buff(ByVal s As String,
                                                 ByVal start As Int32,
                                                 ByVal len As Int32) As Byte()
        Dim l As Int32 = 0
        l = hex_bytes_len(s, start, len)
        If l < 0 Then
            Return Nothing
        End If
        If l = 0 Then
            Return empty_bytes
        End If
        Dim r() As Byte = Nothing
        ReDim r(l - 1)
        Return r
    End Function

    <Extension()> Public Function hex_bytes_buff(ByVal s As String,
                                                 ByVal start As Int32) As Byte()
        Return hex_bytes_buff(s, start, strlen_i(s) - start)
    End Function

    <Extension()> Public Function hex_bytes_buff(ByVal s As String) As Byte()
        Return hex_bytes_buff(s, 0, strlen_i(s))
    End Function

    Public Function bool_byte(ByVal i As Boolean) As Byte
        Return If(i, max_uint8, uint8_0)
    End Function

    Public Function byte_bool(ByVal i As Byte) As Boolean
        Return (i <> 0)
    End Function
End Module
