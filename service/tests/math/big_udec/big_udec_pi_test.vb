
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector._raise_error
Imports osi.root.constants
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.math

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
                s.fully_reduce_fraction()
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
                s.fully_reduce_fraction()
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
                s.fully_reduce_fraction()
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
        assertion.equal(s.as_str().with_upure_length(151), constants.pi_1m().Substring(0, 153))
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
        assertion.equal(s.as_str().with_upure_length(151), constants.pi_1m().Substring(0, 153))
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub calculate_pi_arctangent()
        Dim s As big_udec = Nothing
        s = big_udec.fraction(2, 1)
        Dim c As big_udec = Nothing
        c = big_udec.fraction(2, 3)
        For i As UInt32 = 2 To max_uint32 - uint32_1
            s.add(c)
            c.multiply(big_udec.fraction(i, (i << 1) + uint32_1))

            If (i Mod 100000) = 0 Then
                s.fully_reduce_fraction()
                raise_error(error_type.warning, "@ ", i, " -> ", s.fractional_str())
            End If
        Next
    End Sub

    Private Sub New()
    End Sub
End Class
