
Imports System.Text
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils

Partial Public Class big_uint
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
        Else
            b = CByte(r)
            Return True
        End If
    End Function

    Private Shared Function base_to_shift(ByVal base As Byte) As Byte
        assert_support_base(base)
        assert(base._1count() = 1)
        Dim s As Byte = 0
        Dim b As Byte = 0
        b = base
        While b > 1
            b >>= 1
            s += int8_1
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
        assert(d <= max_uint32)
        Return New big_uint(CUInt(d))
    End Function

    Private Function str_byte() As Byte
        If is_zero() Then
            Return 0
        ElseIf is_one() Then
            Return 1
        Else
            assert(v.size() = 1)
            Return v.back()
        End If
    End Function

    Public Function str(Optional ByVal base As Byte = default_str_base) As String
        If support_base(base) Then
            If is_zero() Then
                Return digit_0
            ElseIf is_one() Then
                Return number_to_char(1)
            Else
                Dim r As StringBuilder = Nothing
                r = New StringBuilder()
                Dim t As big_uint = Nothing
                t = New big_uint(Me)
                Dim dc As Byte = 0
                dc = digit_count_per_parse(base)
                assert(dc > 0)
                If base._1count() = 1 Then
                    Dim s As UInt64 = 0
                    s = shift_base(base, dc)
                    Dim ss As Byte = 0
                    ss = base_to_shift(base)
                    Dim m As UInt32 = 0
                    m = ((uint32_1 << ss) - uint32_1)
                    While Not t.is_zero()
                        Dim d As UInt32 = 0
                        d = t.v(0)
                        For i As UInt32 = 0 To dc - uint32_1
                            r.Append(number_to_char(d And m))
                            d >>= ss
                            If d = 0 Then
                                If t.v.size() > 1 Then
                                    While i < dc - uint32_1
                                        r.Append(digit_0)
                                        i += 1
                                    End While
                                End If
                                Exit For
                            End If
                        Next
                        t.right_shift(s)
                    End While
                Else
                    Dim bu As UInt32 = 0
                    bu = base
                    bu ^= dc
                    While Not t.is_zero()
                        Dim rm As UInt32 = 0
                        t.assert_divide(bu, rm)
                        For i As UInt32 = 0 To dc - uint32_1
                            r.Append(number_to_char(rm Mod base))
                            rm \= base
                            If rm = 0 Then
                                If Not t.is_zero() Then
                                    While i < dc - uint32_1
                                        r.Append(digit_0)
                                        i += uint32_1
                                    End While
                                End If
                                Exit For
                            End If
                        Next
                    End While
                End If
                Return Convert.ToString(r.reverse())
            End If
        Else
            Return Nothing
        End If
    End Function

    Public Shared Function parse(ByVal s As String,
                                 ByRef r As big_uint,
                                 Optional ByVal base As Byte = default_str_base) As Boolean
        If support_base(base) Then
            If String.IsNullOrEmpty(s) OrElse
               s = number_to_char(0) Then
                r = New big_uint()
                Return True
            ElseIf s = number_to_char(1) Then
                r = New big_uint()
                r.set_one()
                Return True
            Else
                Dim dc As Byte = 0
                dc = digit_count_per_parse(base)
                assert(dc > 0)
                Dim shift_t As UInt64 = 0
                Dim multiply_t As big_uint = Nothing
                If base._1count() = 1 Then
                    shift_t = shift_base(base, dc)
                Else
                    multiply_t = multiply_base(base, dc)
                End If
                r = New big_uint()
                For i As Int32 = 0 To strlen(s) - uint32_1 Step dc
                    Dim u As UInt32 = 0
                    Dim j As UInt32 = 0
                    For j = 0 To dc - uint32_1
                        If i + j >= strlen(s) Then
                            Exit For
                        Else
                            Dim b As Byte = 0
                            If char_to_number(s(i + j), b, base) Then
                                u *= base
                                u += b
                            Else
                                Return False
                            End If
                        End If
                    Next
                    If multiply_t Is Nothing Then
                        If j = dc Then
                            r.left_shift(shift_t)
                        Else
                            r.left_shift(shift_base(base, j))
                        End If
                    Else
                        If j = dc Then
                            r.multiply(multiply_t)
                        Else
                            r.multiply(multiply_base(base, j))
                        End If
                    End If
                    r.add(u)
                Next
                Return True
            End If
        Else
            Return False
        End If
    End Function
End Class
