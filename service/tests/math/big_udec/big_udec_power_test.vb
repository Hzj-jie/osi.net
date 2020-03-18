
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.math

<test>
Public NotInheritable Class big_udec_power_test
    <test>
    Private Shared Sub _100_power_2()
        Dim b As big_udec = Nothing
        b = New big_udec(100)
        b.power(New big_udec(2))
        assertion.equal(b.str(), "10000")
    End Sub

    <test>
    Private Shared Sub _100_power_1_2()
        Dim b As big_udec = Nothing
        b = New big_udec(100)
        b.power(New big_udec(New big_uint(1), New big_uint(2)))
        assertion.equal(b.str(), "10")
    End Sub

    <test>
    Private Shared Sub calculate_pi_integral_0_01()
        Dim x As big_udec = Nothing
        x = New big_udec()
        Dim dx As big_udec = Nothing
        dx = New big_udec(0.01)
        Dim s As big_udec = Nothing
        s = New big_udec()
        Dim one As big_udec = Nothing
        one = big_udec.one()
        Dim two As big_udec = Nothing
        two = New big_udec(2)
        Dim half As big_udec = Nothing
        half = New big_udec(big_uint.one(), New big_uint(2))
        While x < one
            s += ((one - x ^ two) ^ half) * dx
            x += dx
        End While

        s *= New big_udec(4)
        assertion.equal(s.str(), "3.160417040038098059978563049550622536854317482358934093156346794")
        '                         3.14159265
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub calculate_pi_integral_0_001()
        Dim x As big_udec = Nothing
        x = New big_udec()
        Dim dx As big_udec = Nothing
        dx = New big_udec(0.001)
        Dim s As big_udec = Nothing
        s = New big_udec()
        Dim one As big_udec = Nothing
        one = big_udec.one()
        Dim two As big_udec = Nothing
        two = New big_udec(2)
        Dim half As big_udec = Nothing
        half = New big_udec(big_uint.one(), New big_uint(2))
        While x < one
            s += ((one - x ^ two) ^ half) * dx
            x += dx
        End While

        s *= New big_udec(4)
        assertion.equal(s.str(), "3.143555466911071948935343231220868364057631202804471077508511478")
        '                         3.14159265
    End Sub

    <test>
    Private Shared Sub calculate_pi_sequence_of_numbers_257()
        Dim s As big_udec = Nothing
        s = big_udec.zero()
        For i As UInt32 = 1 To 257 Step 2
            Dim c As big_udec = Nothing
            c = big_udec.fraction(CUInt(4), i)
            If ((i \ 2) Mod 2) = 0 Then
                s.add(c)
            Else
                s.assert_sub(c)
            End If
        Next
        assertion.equal(s.str(), "3.149344475124619884924585085570033726572656722644429715863680771")
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
            If ((i \ 2) Mod 2) = 0 Then
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
            If ((i \ 2) Mod 2) = 0 Then
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
            If ((i \ 2) Mod 2) = 0 Then
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
    Private Shared Sub calculate_pi_sequence_of_numbers_4194305()
        Dim s As big_udec = Nothing
        s = big_udec.zero()
        For i As UInt32 = 1 To 4194305 Step 2
            Dim c As big_udec = Nothing
            c = big_udec.fraction(CUInt(4), i)
            If ((i \ 2) Mod 2) = 0 Then
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
    Private Shared Sub calculate_pi_sequence_of_numbers_16777217()
        Dim s As big_udec = Nothing
        s = big_udec.zero()
        For i As UInt32 = 1 To 16777217 Step 2
            Dim c As big_udec = Nothing
            c = big_udec.fraction(CUInt(4), i)
            If ((i \ 2) Mod 2) = 0 Then
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
    Private Shared Sub calculate_pi_sequence_of_numbers_67108865()
        Dim s As big_udec = Nothing
        s = big_udec.zero()
        For i As UInt32 = 1 To 67108865 Step 2
            Dim c As big_udec = Nothing
            c = big_udec.fraction(CUInt(4), i)
            If ((i \ 2) Mod 2) = 0 Then
                s.add(c)
            Else
                s.assert_sub(c)
            End If
        Next
        assertion.equal(s.str(), "3.141600282926117160615236325805874484799183595558135389722686711")
        '                         3.14159265
    End Sub

    Private Sub New()
    End Sub
End Class
