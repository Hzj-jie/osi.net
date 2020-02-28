
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.math

<test>
Public NotInheritable Class big_udec_str_test
    <test>
    Private Shared Sub e()
        Dim v As big_udec = Nothing
        v = New big_udec(System.Math.E)

        assertion.equal(v.str(), "2.718281828459044646706388")
        '                         2.71828182845905
    End Sub

    <test>
    Private Shared Sub pi()
        Dim v As big_udec = Nothing
        v = New big_udec(System.Math.PI)

        assertion.equal(v.str(), "3.1415926535897931159979634")
        '                         3.14159265358979
    End Sub

    <test>
    Private Shared Sub _0_5()
        Dim u As upure_dec = Nothing
        u = New upure_dec(0.5)

        assertion.equal(u.str(), "0.5")
    End Sub

    <test>
    Private Shared Sub _0_999999999()
        Dim u As upure_dec = Nothing
        u = New upure_dec(0.999999999)

        '                         0.999999999
        assertion.equal(u.str(), "0.999999998999999917259629")
    End Sub

    <test>
    Private Shared Sub _0_00000000001()
        Dim u As upure_dec = Nothing
        u = New upure_dec(0.00000000001)

        '                         0.00000000001
        assertion.equal(u.str(), "0.00000000001000000082740370999090373")
    End Sub

    <test>
    Private Shared Sub e_pure_part()
        Dim u As upure_dec = Nothing
        u = New upure_dec(System.Math.E - 2)

        '                         0.71828182845905
        assertion.equal(u.str(), "0.718281828459044646706388")
    End Sub

    <test>
    Private Shared Sub pi_pure_part()
        Dim u As upure_dec = Nothing
        u = New upure_dec(System.Math.PI - 3)

        '                         0.14159265358979
        assertion.equal(u.str(), "0.1415926535897931159979634")
    End Sub

    Private Sub New()
    End Sub
End Class
