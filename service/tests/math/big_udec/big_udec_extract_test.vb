
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
                        "0.7071067811865475244008443621048490392848359376884740365883398689953662392310535194251937671638207863675065022097118471484822304")
        '                0.707106781186547524400844362104849039284835937688474036588339868995366239231053519425193767163820786367506
    End Sub

    <test>
    Private Shared Sub _9_div_10_extract_by_2()
        Dim u As big_udec = Nothing
        u = New big_udec(New big_uint(9), New big_uint(10))
        u = u.assert_extract(New big_uint(2))

        assertion.equal(u.str(), "0.948683298050513799599668063329815554695025889256134223709493")
        '                         0.94868329805
    End Sub

    <test>
    Private Shared Sub _0_9_extract_by_2()
        Dim u As big_udec = Nothing
        u = New big_udec(0.9)
        u = u.assert_extract(New big_uint(2))

        assertion.equal(u.str(), "0.948683298050513986844119720753851110455951021295216106913790691")
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

        assertion.equal(u.str(), "0.31622776601683737146653438217061084741446638326044857952937984")
        '                         0.31622776601
    End Sub

    <test>
    Private Shared Sub _0_5_extract_by_3()
        Dim u As big_udec = Nothing
        u = New big_udec(0.5)
        u = u.assert_extract(New big_uint(3), 350)

        assertion.equal(u.str(), "0.79370052598409973737585281963615413019574666394992650490414288")
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

        assertion.equal(u.str(), "0.965489384605629884900945503883147468437274698746943697890752313")
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

        assertion.equal(u.str(), "0.464158883361277339566469616965230323949346546088885098257910799")
        '                         0.46415888336128
    End Sub

    Private Sub New()
    End Sub
End Class
