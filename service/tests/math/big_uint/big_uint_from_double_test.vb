

Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.math

<test>
Public NotInheritable Class big_uint_from_double_test
    <test>
    Private Shared Sub within_int()
        Dim b As big_uint = Nothing
        b = New big_uint(100.1)
        assertion.equal(b.ToString(), "100")
    End Sub

    <test>
    Private Shared Sub max_uint64_plus_1()
        Dim b As big_uint = Nothing
        b = New big_uint(CDbl(max_uint64) + 1)
        assertion.equal(b.ToString(), "18446744073709551616")
    End Sub

    <test>
    Private Shared Sub over_ulong()
        Dim b As big_uint = Nothing
        b = New big_uint(1.0E+20)
        assertion.equal(b.ToString(), "100000000000000000000")
    End Sub

    <test>
    Private Shared Sub disallow_negative()
        Dim b As big_uint = Nothing
        b = New big_uint()
        assertion.is_false(b.replace_by(CDbl(-1)))
    End Sub

    <test>
    Private Shared Sub max_double()
        Dim b As big_uint = Nothing
        b = New big_uint(Double.MaxValue)
        assertion.equal(b.ToString(), strcat(
                        "179769313486231570814527423731704356798070567525844996598917476803157260780028538760589558632",
                        "766878171540458953514382464234321326889464182768467546703537516986049910576551282076245490090",
                        "389328944075868508455133942304583236903222948165808559332123348274797826204144723168738177180",
                        "919299881250404026184124858368"))
    End Sub

    <test>
    Private Shared Sub max_decimal()
        Dim b As big_uint = Nothing
        b = New big_uint(Decimal.MaxValue)
        assertion.equal(b.ToString(), "79228162514264337597838917631")
    End Sub

    Private Sub New()
    End Sub
End Class
