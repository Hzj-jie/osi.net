
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants

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
        Dim r As Int16 = 0
        r = chars(Convert.ToInt32(c))
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
        Dim b As Byte = 0
        b = base
        While b > 1
            b >>= 1
            s += uint8_1
        End While
        Return s
    End Function

    Private Shared Function shift_base(ByVal base As Byte, ByVal digit_count As Byte) As UInt64
        Dim r As UInt64 = 0
        r = base_to_shift(base)
        r *= digit_count
        Return r
    End Function

    Private Shared Function multiply_base(ByVal base As Byte, ByVal digit_count As Byte) As big_uint
        Dim d As Double = 0
        d = base
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
        Dim r As StringBuilder = Nothing
        r = New StringBuilder()
        Dim t As big_uint = Nothing
        t = New big_uint(Me)
        Dim dc As Byte = 0
        dc = digit_count_per_parse(base)
        assert(dc > 0)
        Dim bu As big_uint = Nothing
        bu = assert_which.of(base ^ dc).can_cast_to_uint64()
        assert(bu.less_or_equal(CULng(max_uint32_plus_1)))
        While Not t.is_zero()
            Dim rmb As big_uint = Nothing
            t.assert_divide(bu, rmb)
            Dim rm As UInt32 = 0
            rm = rmb.as_uint32()
            For i As UInt32 = 0 To dc - uint32_1
                Dim c As UInt32 = 0
                rm = rm.div_rem(base, c)
                r.Append(number_to_char(assert_which.of(c).can_cast_to_byte()))
                If rm = 0 Then
                    If Not t.is_zero() Then
                        r.Append(digit_0, CInt(dc - i - uint32_1))
                    End If
                    Exit For
                End If
            Next
        End While
        Return Convert.ToString(r.reverse())
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
        Dim dc As Byte = 0
        dc = digit_count_per_parse(base)
        assert(dc > 0)
        Dim multiply_t As big_uint = Nothing
        multiply_t = multiply_base(base, dc)
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
