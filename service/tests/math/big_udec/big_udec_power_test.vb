
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
        b.power(big_udec.fraction(1, 2))
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
        half = big_udec.fraction(1, 2)
        While x < one
            s += ((one - x ^ two) ^ half) * dx
            x += dx
        End While

        s *= New big_udec(4)
        assertion.equal(s.str(), "3.1604170400380980599785630495506225368543174823589340931563467947")
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
        half = big_udec.fraction(1, 2)
        While x < one
            s += ((one - x ^ two) ^ half) * dx
            x += dx
        End While

        s *= New big_udec(4)
        assertion.equal(s.str(), "3.143555466911071948935343231220868364057631202804471077508511478")
        '                         3.14159265
    End Sub

    Private Sub New()
    End Sub
End Class
