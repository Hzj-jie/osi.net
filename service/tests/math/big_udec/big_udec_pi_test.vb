
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector._raise_error
Imports osi.root.constants
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.math
Imports constants = osi.service.resources.math

<test>
Public NotInheritable Class big_udec_pi_test
    <test>
    Private Shared Sub calculate_pi_sequence_of_numbers_257()
        Dim s As big_udec = Nothing
        s = big_udec.zero()
        For i As UInt32 = 1 To 257 Step 2
            Dim c As big_udec = Nothing
            c = big_udec.fraction(CUInt(4), i)
            If (i >> 1).even() Then
                s.add(c)
            Else
                s.assert_sub(c)
            End If
        Next
        assertion.equal(s.str(), "3.1493444751246198849245850855700337265726567226444297158636807717")
        '                         3.14159265
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub calculate_pi_sequence_of_numbers_65537()
        Dim s As big_udec = Nothing
        s = big_udec.zero()
        For i As UInt32 = 1 To 65537 Step 2
            Dim c As big_udec = Nothing
            c = big_udec.fraction(CUInt(4), i)
            If (i >> 1).even() Then
                s.add(c)
            Else
                s.assert_sub(c)
            End If
        Next
        assertion.equal(s.str(), "3.141623170236616979912392273028839981728932502884783801155901887")
        '                         3.14159265
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub calculate_pi_sequence_of_numbers_262145()
        Dim s As big_udec = Nothing
        s = big_udec.zero()
        For i As UInt32 = 1 To 262145 Step 2
            Dim c As big_udec = Nothing
            c = big_udec.fraction(CUInt(4), i)
            If (i >> 1).even() Then
                s.add(c)
            Else
                s.assert_sub(c)
            End If
        Next
        assertion.equal(s.str(), "3.141600282926117160615236325805874484799183595558135389722686711")
        '                         3.14159265
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub calculate_pi_sequence_of_numbers_1048577()
        Dim s As big_udec = Nothing
        s = big_udec.zero()
        For i As UInt32 = 1 To 1048577 Step 2
            Dim c As big_udec = Nothing
            c = big_udec.fraction(CUInt(4), i)
            If (i >> 1).even() Then
                s.add(c)
            Else
                s.assert_sub(c)
            End If
        Next
        assertion.equal(s.str(), "3.141594560934788077359718789531080630631742149542323170812144822")
        '                         3.14159265
        'With GCD disabled and fractional_str()
        'osi.tests.service.math.big_udec_pi_test.calculate_pi_sequence_of_numbers_1048577,
        'total time in milliseconds 16317400
        'With GCD enabled
        'osi.tests.service.math.big_udec_pi_test.calculate_pi_sequence_of_numbers_1048577,
        'total time in milliseconds 110637426
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub calculate_pi_sequence_of_numbers_4194305()
        Dim s As big_udec = Nothing
        s = big_udec.zero()
        For i As UInt32 = 1 To 4194305 Step 2
            Dim c As big_udec = Nothing
            c = big_udec.fraction(CUInt(4), i)
            If (i >> 1).even() Then
                s.add(c)
            Else
                s.assert_sub(c)
            End If
        Next
        assertion.equal(s.fractional_str(), "3.141600282926117160615236325805874484799183595558135389722686711")
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub calculate_pi_sequence_of_numbers_16777217()
        Dim s As big_udec = Nothing
        s = big_udec.zero()
        For i As UInt32 = 1 To 16777217 Step 2
            Dim c As big_udec = Nothing
            c = big_udec.fraction(CUInt(4), i)
            If (i >> 1).even() Then
                s.add(c)
            Else
                s.assert_sub(c)
            End If
        Next
        assertion.equal(s.fractional_str(), "3.141600282926117160615236325805874484799183595558135389722686711")
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub calculate_pi_sequence_of_numbers_67108865()
        Dim s As big_udec = Nothing
        s = big_udec.zero()
        For i As UInt32 = 1 To 67108865 Step 2
            Dim c As big_udec = Nothing
            c = big_udec.fraction(CUInt(4), i)
            If (i >> 1).even() Then
                s.add(c)
            Else
                s.assert_sub(c)
            End If
        Next
        assertion.equal(s.fractional_str(), "3.141600282926117160615236325805874484799183595558135389722686711")
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub calculate_pi_sequence_of_numbers_max_uint32_minus_3_progressively()
        Dim s As big_udec = Nothing
        s = big_udec.zero()
        For i As UInt32 = 1 To max_uint32 - CUInt(3) Step 2
            Dim c As big_udec = Nothing
            c = big_udec.fraction(CUInt(4), i)
            If (i >> 1).even() Then
                s.add(c)
            Else
                s.assert_sub(c)
            End If

            If (i Mod 2500000) = 1 Then
                c.reduce_fraction()
                s.reduce_fraction()
                raise_error(error_type.warning, "@ ", i, " -> ", s.fractional_str())
            End If
        Next
    End Sub

    <test>
    Private Shared Sub calculate_pi_nilakantha_20002()
        Dim s As big_udec = Nothing
        s = New big_udec(uint32_3, uint32_1)
        For i As UInt32 = 2 To 20002 Step 2
            Dim d As big_uint = Nothing
            d = New big_uint(i).
                    multiply(i + uint32_1).
                    multiply(i + uint32_2)
            Dim c As big_udec = Nothing
            c = big_udec.fraction(CUInt(4), d)
            If (i >> 1).odd() Then
                s.add(c)
            Else
                s.assert_sub(c)
            End If
        Next
        assertion.equal(s.str(), "3.1415926535900430885195015124988771459935110088383081739079144538")
        '                         3.1415926535897932384626433832795028841971693993751058209749445923078164062862089986280348253421170679
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub calculate_pi_nilakantha_max_uint32_minus_3_progressively()
        Dim s As big_udec = Nothing
        s = New big_udec(uint32_3, uint32_1)
        For i As UInt32 = 2 To max_uint32 - CUInt(3) Step 2
            Dim d As big_uint = Nothing
            d = New big_uint(i).
                    multiply(i + uint32_1).
                    multiply(i + uint32_2)
            Dim c As big_udec = Nothing
            c = big_udec.fraction(CUInt(4), d)
            If (i >> 1).odd() Then
                s.add(c)
            Else
                s.assert_sub(c)
            End If

            If (i Mod 400000) = 2 Then
                c.reduce_fraction()
                s.reduce_fraction()
                raise_error(error_type.warning, "@ ", i, " -> ", s.fractional_str())
            End If
        Next
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub calculate_pi_wallis_product_max_uint32_progressively()
        Dim s As big_udec = Nothing
        s = big_udec.one()
        s.multiply(big_udec.fraction(CUInt(2), CUInt(1)))
        For i As UInt32 = 2 To max_uint32 - CUInt(2) Step 2
            s.multiply(big_udec.fraction(i, i - uint32_1))
            s.multiply(big_udec.fraction(i, i + uint32_1))

            If (i Mod 900000) = 2 Then
                s.reduce_fraction()
                raise_error(error_type.warning, "@ ", i, " -> ", s.fractional_str())
            End If
        Next
    End Sub

    <test>
    Private Shared Sub calculate_pi_arctangent_500()
        Dim s As big_udec = Nothing
        s = big_udec.fraction(2, 1)
        Dim c As big_udec = Nothing
        c = big_udec.fraction(2, 3)
        For i As UInt32 = 2 To 500
            s.add(c)
            c.multiply(big_udec.fraction(i, (i << 1) + uint32_1))
        Next
        assertion.equal(s.as_str().with_upure_length(151), constants.pi_1k().Substring(0, 153))
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub calculate_pi_arctangent_700()
        Dim s As big_udec = Nothing
        s = big_udec.fraction(2, 1)
        Dim c As big_udec = Nothing
        c = big_udec.fraction(2, 3)
        For i As UInt32 = 2 To 700
            s.add(c)
            c.multiply(big_udec.fraction(i, (i << 1) + uint32_1))
        Next
        assertion.equal(s.as_str().with_upure_length(151), constants.pi_1k().Substring(0, 153))
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub calculate_pi_arctangent_20000()
        Dim s As big_udec = Nothing
        s = big_udec.fraction(2, 1)
        Dim c As big_udec = Nothing
        c = big_udec.fraction(2, 3)
        For i As UInt32 = 2 To 20000
            s.add(c)
            c.multiply(big_udec.fraction(i, (i << 1) + uint32_1))
        Next
        assertion.equal(s.as_str().with_upure_length(151), constants.pi_1k().Substring(0, 153))
    End Sub

    'https://en.wikipedia.org/wiki/Approximations_of_%CF%80
    'https://wikimedia.org/api/rest_v1/media/math/render/svg/6473cdf91ec36900fe186b25394dac360ce4df51
    'Other formulae that have been used to compute estimates of π include:
    '{\displaystyle {\frac {\pi }{2}}=\sum _{k=0}^{\infty }{\frac {k!}{(2k+1)!!}}=\sum _{k=0}^{\infty }{\frac {2^{k}k!^{2}}{(2k+1)!}}=1+{\frac {1}{3}}\left(1+{\frac {2}{5}}\left(1+{\frac {3}{7}}\left(1+\cdots \right)\right)\right)}{\displaystyle {\frac {\pi }{2}}=\sum _{k=0}^{\infty }{\frac {k!}{(2k+1)!!}}=\sum _{k=0}^{\infty }{\frac {2^{k}k!^{2}}{(2k+1)!}}=1+{\frac {1}{3}}\left(1+{\frac {2}{5}}\left(1+{\frac {3}{7}}\left(1+\cdots \right)\right)\right)}
    'Newton.
    <command_line_specified>
    <test>
    Private Shared Sub calculate_pi_arctangent()
        Dim s As big_udec = Nothing
        s = big_udec.fraction(2, 1)
        Dim c As big_udec = Nothing
        c = big_udec.fraction(2, 3)
        For i As UInt32 = 2 To max_uint32 - uint32_1
            s.add(c)
            c.multiply(big_udec.fraction(New big_uint(i), New big_uint(i).left_shift(1).add(uint32_1)))

            If (i Mod 100000) = 0 Then
                c.reduce_fraction()
                s.reduce_fraction()
                raise_error(error_type.warning, "@ ", i, " -> ", s.fractional_str(), " : c -> ", c.fractional_str())
            End If
        Next
    End Sub

    'https://en.wikipedia.org/wiki/Bailey%E2%80%93Borwein%E2%80%93Plouffe_formula
    '{\displaystyle \pi =\sum _{k=0}^{\infty }\left[{\frac {1}{16^{k}}}\left({\frac {4}{8k+1}}-{\frac {2}{8k+4}}-{\frac {1}{8k+5}}-{\frac {1}{8k+6}}\right)\right].}
    <command_line_specified>
    <test>
    Private Shared Sub bbp()
        Dim s As big_udec = Nothing
        s = big_udec.zero()
        For i As UInt32 = 0 To max_uint32 - uint32_1
            Dim c As big_udec = Nothing
            c = big_udec.fraction(4, 8 * i + 1)
            c.assert_sub(big_udec.fraction(2, 8 * i + 4))
            c.assert_sub(big_udec.fraction(1, 8 * i + 5))
            c.assert_sub(big_udec.fraction(1, 8 * i + 6))
            c.multiply(big_udec.fraction(big_uint.one(), big_uint.one().left_shift(CULng(4) * i)))
            s.add(c)

            If i <> 0 AndAlso (i Mod 10000) = 0 Then
                c.reduce_fraction()
                s.reduce_fraction()
                raise_error(error_type.warning, "@ ", i, " -> ", s.fractional_str(), " : c -> ", c.fractional_str())
            End If
        Next
    End Sub

    Private Sub New()
    End Sub
End Class
