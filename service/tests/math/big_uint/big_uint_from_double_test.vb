

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
        assertion.equal(b.ToString(), "18446744082299486209")
    End Sub

    <test>
    Private Shared Sub over_ulong()
        Dim b As big_uint = Nothing
        b = New big_uint(1.0E+20)
        assertion.equal(b.ToString(), "100000000049052868146")
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
                        "179769314825617350057869404685903988209575435695108370126632428513497287241600446167459493384",
                        "781744741289090610649653963845182886256606719088914126374964289066839631537282861618570846051",
                        "746655938852396502351841913761412994518679398318328663176626671207397867083993719165972677371",
                        "351025953628601751508514502656"))
    End Sub

    <test>
    Private Shared Sub max_decimal()
        Dim b As big_uint = Nothing
        b = New big_uint(Decimal.MaxValue)
        assertion.equal(b.ToString(), "79228162569604569827557507072")
    End Sub

    Private Sub New()
    End Sub
End Class
