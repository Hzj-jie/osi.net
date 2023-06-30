
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.math

<test>
Public NotInheritable Class big_uint_gcd_test
    <test>
    Private Shared Sub predefined()
        assertion.equal(big_uint.gcd(CUInt(100), CUInt(200)), New big_uint(100))
        assertion.equal(big_uint.gcd(CUInt(1), CUInt(200)), New big_uint(1))
        assertion.equal(big_uint.gcd(CUInt(200), CUInt(200)), New big_uint(200))

        assertion.equal(big_uint.gcd(CULng(111669149695L), CULng(111669149695L)), New big_uint(CULng(111669149695L)))
        assertion.equal(big_uint.gcd(CULng(111669149695L), CULng(22333829939L)), New big_uint(CULng(22333829939L)))
    End Sub

    <repeat(10000)>
    <test>
    Private Shared Sub random_cases()
        Dim l As big_uint = Nothing
        Dim r As big_uint = Nothing
        Do
            l = big_uint.random()
        Loop While l.is_zero()
        Do
            r = big_uint.random()
        Loop While r.is_zero()
        Dim c As big_uint = Nothing
        c = big_uint.gcd(l, r)
        assertion.is_false(c.is_zero())
        Dim remainder As big_uint = Nothing
        l.assert_divide(c, remainder)
        assertion.is_true(remainder.is_zero())
        r.assert_divide(c, remainder)
        assertion.is_true(remainder.is_zero())
        assertion.is_true(big_uint.gcd(l, r).is_one())
    End Sub

    Private Sub New()
    End Sub
End Class
