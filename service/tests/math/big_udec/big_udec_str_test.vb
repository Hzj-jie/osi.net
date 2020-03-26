
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

        assertion.equal(v.str(), "2.718281828459044646706388448365032672882080078125")
        '                         2.71828182845905
    End Sub

    <test>
    Private Shared Sub pi()
        Dim v As big_udec = Nothing
        v = New big_udec(System.Math.PI)

        assertion.equal(v.str(), "3.141592653589793115997963468544185161590576171875")
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

        assertion.equal(u.str(), "0.999999998999999917259629000909626483917236328125")
        '                         0.999999999
    End Sub

    <test>
    Private Shared Sub _0_00000000001()
        Dim u As big_udec = Nothing
        u = New big_udec(0.00000000001)

        assertion.equal(u.str(), "0.00000000001000000082740370999090373516082763671875")
        '                         0.00000000001
    End Sub

    <test>
    Private Shared Sub e_pure_part()
        Dim u As big_udec = Nothing
        u = New big_udec(System.Math.E - 2)

        assertion.equal(u.str(), "0.718281828459044646706388448365032672882080078125")
        '                         0.71828182845905
    End Sub

    <test>
    Private Shared Sub pi_pure_part()
        Dim u As big_udec = Nothing
        u = New big_udec(System.Math.PI - 3)

        assertion.equal(u.str(), "0.141592653589793115997963468544185161590576171875")
        '                         0.14159265358979
    End Sub

    Private Sub New()
    End Sub
End Class
