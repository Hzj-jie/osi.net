
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.math

<test>
Public NotInheritable Class big_ufloat_str_test
    <test>
    Private Shared Sub e()
        Dim v As big_ufloat = Nothing
        v = New big_ufloat(System.Math.E)

        assertion.equal(v.str(), "2.718281828459044646706388")
        '                         2.71828182845905
    End Sub

    <test>
    Private Shared Sub pi()
        Dim v As big_ufloat = Nothing
        v = New big_ufloat(System.Math.PI)

        assertion.equal(v.str(), "3.1415926535897931159979634")
        '                         3.14159265358979
    End Sub

    Private Sub New()
    End Sub
End Class
