
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.math

<test>
Public NotInheritable Class big_udec_extract_test
    <test>
    Private Shared Sub _0_5_extract_by_2()
        Dim u As big_udec = Nothing
        u = New big_udec(0.5)
        u = u.assert_extract(New big_uint(2), 350)

        assertion.equal(u.as_str().with_upure_length(128),
                        "0.70710678118654752440084436210484903928483593768847403658833986899536623923105351942519376716382078636750650220971184714848223041")
        '                0.707106781186547524400844362104849039284835937688474036588339868995366239231053519425193767163820786367506
    End Sub

    <test>
    Private Shared Sub _9_div_10_extract_by_2()
        Dim u As big_udec = Nothing
        u = New big_udec(New big_uint(9), New big_uint(10))
        u = u.assert_extract(New big_uint(2))

        assertion.equal(u.str(), "0.9486832980505137995996680633298155546950258892561342237094936251")
        '                         0.94868329805
    End Sub

    <test>
    Private Shared Sub _0_9_extract_by_2()
        Dim u As big_udec = Nothing
        u = New big_udec(0.9)
        u = u.assert_extract(New big_uint(2))

        assertion.equal(u.str(), "0.9486832980505139868441197207538511104559510212952161069137906915")
        '                         0.94868329805
    End Sub

    <test>
    Private Shared Sub _0_999999999_extract_by_2()
        Dim u As big_udec = Nothing
        u = New big_udec(0.999999999)
        u = u.assert_extract(New big_uint(2))

        assertion.equal(u.str(), "0.999999999499999958504814479707219620887017292748197298422087996")
        '                         0.9999999995
    End Sub

    <test>
    Private Shared Sub _0_1_extract_by_2()
        Dim u As big_udec = Nothing
        u = New big_udec(0.1)
        u = u.assert_extract(New big_uint(2))

        assertion.equal(u.str(), "0.3162277660168373714665343821706108474144663832604485795293798408")
        '                         0.31622776601
    End Sub

    <test>
    Private Shared Sub _0_5_extract_by_3()
        Dim u As big_udec = Nothing
        u = New big_udec(0.5)
        u = u.assert_extract(New big_uint(3), 350)

        assertion.equal(u.str(), "0.7937005259840997373758528196361541301957466639499265049041428809")
        '                         0.7937005259841
    End Sub

    <test>
    Private Shared Sub _9_div_10_extract_by_3()
        Dim u As big_udec = Nothing
        u = New big_udec(New big_uint(9), New big_uint(10))
        u = u.assert_extract(New big_uint(3))

        assertion.equal(u.str(), "0.96548938460562975785993278443506691761278948241073248606247529")
        '                         0.9654893846056
    End Sub

    <test>
    Private Shared Sub _0_9_extract_by_3()
        Dim u As big_udec = Nothing
        u = New big_udec(0.9)
        u = u.assert_extract(New big_uint(3))

        assertion.equal(u.str(), "0.9654893846056298849009455038831474684372746987469436978907523137")
        '                         0.9654893846056
    End Sub

    <test>
    Private Shared Sub _0_999999999_extract_by_3()
        Dim u As big_udec = Nothing
        u = New big_udec(0.999999999)
        u = u.assert_extract(New big_uint(3))

        assertion.equal(u.str(), "0.999999999666666638975431870743619434863934788283593211224221846")
        '                         0.99999999966667
    End Sub

    <test>
    Private Shared Sub _0_1_extract_by_3()
        Dim u As big_udec = Nothing
        u = New big_udec(0.1)
        u = u.assert_extract(New big_uint(3))

        assertion.equal(u.str(), "0.4641588833612773395664696169652303239493465460888850982579107996")
        '                         0.46415888336128
    End Sub

    Private Sub New()
    End Sub
End Class
