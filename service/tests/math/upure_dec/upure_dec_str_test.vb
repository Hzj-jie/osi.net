
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.math

<test>
Public NotInheritable Class upure_dec_str_test
    <test>
    Private Shared Sub _0_5()
        Dim u As upure_dec = Nothing
        u = New upure_dec(0.5)

        assertion.equal(u.str(), "0.5")
    End Sub

    <test>
    Private Shared Sub _0_00000000001()
        Dim u As upure_dec = Nothing
        u = New upure_dec(0.00000000001)

        assertion.equal(u.str(), "0.0000000000099999999999999993949696928193981")
    End Sub

    <test>
    Private Shared Sub e()
        Dim u As upure_dec = Nothing
        u = New upure_dec(System.Math.E - 2)

        assertion.equal(u.str(), "0.718281828459045090795598")
    End Sub

    <test>
    Private Shared Sub pi()
        Dim u As upure_dec = Nothing
        u = New upure_dec(System.Math.PI - 3)

        assertion.equal(u.str(), "0.1415926535897931159979634")
    End Sub

    Private Sub New()
    End Sub
End Class
