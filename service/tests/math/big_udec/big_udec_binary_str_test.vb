
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.math

<test>
Public NotInheritable Class big_udec_binary_str_test
    <test>
    Private Shared Sub _0_5()
        Dim u As big_udec = Nothing
        u = New big_udec(0.5)

        assertion.equal(u.str(2), "0.1")
    End Sub

    <test>
    Private Shared Sub _0_999999999()
        Dim u As big_udec = Nothing
        u = New big_udec(0.999999999)

        assertion.equal(u.str(2), "0.111111111111111111111111111110111011010001111101")
    End Sub

    <test>
    Private Shared Sub _0_00000000001()
        Dim u As big_udec = Nothing
        u = New big_udec(0.00000000001)

        assertion.equal(u.str(2), "0.00000000000000000000000000000000000010101111111011")
    End Sub

    <test>
    Private Shared Sub e()
        Dim u As big_udec = Nothing
        u = New big_udec(System.Math.E - 2)

        assertion.equal(u.str(2), "0.101101111110000101010001011000101000101011101101")
    End Sub

    <test>
    Private Shared Sub pi()
        Dim u As big_udec = Nothing
        u = New big_udec(System.Math.PI - 3)

        assertion.equal(u.str(2), "0.001001000011111101101010100010001000010110100011")
    End Sub

    Private Sub New()
    End Sub
End Class
