
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class big_uint
    Public Shared Function support_str_char(ByVal c As Char, ByVal base As Byte) As Boolean
        Return char_to_number(c, Nothing, base)
    End Function

    Public Shared Function support_str_char(ByVal c As Char) As Boolean
        Return support_str_char(c, support_str_base)
    End Function

    Private Shared Function number_to_char(ByVal i As Byte) As Char
        assert(i >= 0 AndAlso i < array_size(digits))
        Return digits(i)
    End Function

    Private Shared Function char_to_number(ByVal c As Char, ByRef b As Byte, ByVal base As Byte) As Boolean
        assert_support_base(base)
        Dim r As Int16 = chars(Convert.ToInt32(c))
        If r = npos OrElse r >= base Then
            Return False
        End If
        b = CByte(r)
        Return True
    End Function

    Private Shared Function base_to_shift(ByVal base As Byte) As Byte
        assert_support_base(base)
        assert(base._1count() = 1)
        Dim s As Byte = 0
        Dim b As Byte = base
        While b > 1
            b >>= 1
            s += uint8_1
        End While
        Return s
    End Function

    Private Shared Function shift_base(ByVal base As Byte, ByVal digit_count As Byte) As UInt64
        Dim r As UInt64 = base_to_shift(base)
        r *= digit_count
        Return r
    End Function

    Private Shared Function multiply_base(ByVal base As Byte, ByVal digit_count As Byte) As big_uint
        Dim d As Double = base
        d ^= digit_count
        assert(d <= max_uint64)
        Return New big_uint(CULng(d))
    End Function

    Public Function str(Optional ByVal base As Byte = default_str_base) As String
        If Not support_base(base) Then
            Return Nothing
        End If
        If is_zero() Then
            Return digit_0
        End If
        If is_one() Then
            Return number_to_char(1)
        End If
        Dim chunk_base As UInt32 = chunk_base_per_base(base)
        Dim dc As Byte = chunk_dc_per_base(base)
        Dim chunks As New vector(Of UInt32)()
        Dim t As New big_uint(Me)
        While Not t.is_zero()
            Dim rem As UInt32 = 0
            t.assert_divide(chunk_base, rem)
            chunks.push_back(rem)
        End While
        assert(Not chunks.empty())
        Dim r As New StringBuilder()
        If base = default_str_base Then
            r.Append(Convert.ToString(chunks.back()))
            For i As Int32 = CInt(chunks.size()) - 2 To 0 Step -1
                Dim s As String = Convert.ToString(chunks.get(CUInt(i)))
                If s.Length < 9 Then
                    r.Append("0"c, 9 - s.Length)
                End If
                r.Append(s)
            Next
        Else
            Dim top As UInt32 = chunks.back()
            Dim top_chars As New vector(Of Char)()
            While top > 0
                top_chars.push_back(number_to_char(CByte(top Mod base)))
                top \= base
            End While
            For i As Int32 = CInt(top_chars.size()) - 1 To 0 Step -1
                r.Append(top_chars.get(CUInt(i)))
            Next
            Dim buf(dc - 1) As Char
            For i As Int32 = CInt(chunks.size()) - 2 To 0 Step -1
                Dim rem As UInt32 = chunks.get(CUInt(i))
                For j As Int32 = dc - 1 To 0 Step -1
                    buf(j) = number_to_char(CByte(rem Mod base))
                    rem \= base
                Next
                r.Append(buf)
            Next
        End If
        Return Convert.ToString(r)
    End Function

    Public Shared Function parse(ByVal s As String,
                                 ByRef r As big_uint,
                                 Optional ByVal base As Byte = default_str_base) As Boolean
        If Not support_base(base) Then
            Return False
        End If
        If s.null_or_whitespace() Then
            r = New big_uint()
            Return True
        End If
        s = s.Trim()
        If s = number_to_char(0) Then
            r = New big_uint()
            Return True
        End If
        If s = number_to_char(1) Then
            r = big_uint.one()
            Return True
        End If
        Dim dc As Byte = digit_count_per_parse(base)
        assert(dc > 0)
        Dim multiply_t As big_uint = multiply_base(base, dc)
        r = New big_uint()
        For i As Int32 = 0 To strlen_i(s) - 1 Step dc
            Dim u As UInt32 = 0
            Dim j As Int32 = 0
            For j = 0 To dc - 1
                If i + j >= strlen(s) Then
                    r.multiply(multiply_base(base, assert_which.of(j).can_cast_to_byte()))
                    r.add(u)
                    Return True
                End If
                Dim b As Byte = 0
                If char_to_number(s(i + j), b, base) Then
                    u *= base
                    u += b
                Else
                    Return False
                End If
            Next
            r.multiply(multiply_t)
            r.add(u)
        Next
        Return True
    End Function
End Class
