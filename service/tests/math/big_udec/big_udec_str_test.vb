﻿
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

        assertion.equal(v.str(), "2.7182818284590446467063")
        '                         2.71828182845905
    End Sub

    <test>
    Private Shared Sub pi()
        Dim v As big_udec = Nothing
        v = New big_udec(System.Math.PI)

        assertion.equal(v.str(), "3.1415926535897931159979")
        '                         3.14159265358979
    End Sub

    <test>
    Private Shared Sub _0_5()
        Dim u As big_udec = Nothing
        u = New big_udec(0.5)

        assertion.equal(u.str(), "0.5")
    End Sub

    <test>
    Private Shared Sub _0_999999999()
        Dim u As big_udec = Nothing
        u = New big_udec(0.999999999)

        '                         0.999999999
        assertion.equal(u.str(), "0.9999999989999999172596")
    End Sub

    <test>
    Private Shared Sub _0_00000000001()
        Dim u As big_udec = Nothing
        u = New big_udec(0.00000000001)

        '                         0.00000000001
        assertion.equal(u.str(), "0.000000000010000000827403709990903")
    End Sub

    <test>
    Private Shared Sub e_pure_part()
        Dim u As big_udec = Nothing
        u = New big_udec(System.Math.E - 2)

        '                         0.71828182845905
        assertion.equal(u.str(), "0.7182818284590446467063")
    End Sub

    <test>
    Private Shared Sub pi_pure_part()
        Dim u As big_udec = Nothing
        u = New big_udec(System.Math.PI - 3)

        '                         0.14159265358979
        assertion.equal(u.str(), "0.1415926535897931159979")
    End Sub

    Private Sub New()
    End Sub
End Class
