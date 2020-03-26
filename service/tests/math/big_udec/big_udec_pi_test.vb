
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

            If (i Mod 100000) = 1 Then
                s.fully_reduce_fraction()
                raise_error(error_type.warning, "@ ", i, " -> ", s.fractional_str())
            End If
        Next
    End Sub

    Private Sub New()
    End Sub
End Class
