
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.math

<test>
Public NotInheritable Class upure_dec_extract_test
    <test>
    Private Shared Sub _0_5_extract_by_2()
        Dim u As upure_dec = Nothing
        u = New upure_dec(0.5)
        u = u.assert_extract(New big_uint(2))

        ' TODO: High precision is needed.
        ' assertion.equal(u.str(), "0.70710678118")
    End Sub

    Private Sub New()
    End Sub
End Class
