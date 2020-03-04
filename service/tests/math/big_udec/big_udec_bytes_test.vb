
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.math

<repeat(1000000)>
<test>
Public NotInheritable Class big_udec_bytes_test
    <test>
    Private Shared Sub run()
        Dim b As big_udec = Nothing
        b = big_udec.random()
        Dim o As big_udec = Nothing
        o = New big_udec()
        assertion.is_true(o.replace_by(b.as_bytes()))
        assertion.equal(o, b)
    End Sub

    Private Sub New()
    End Sub
End Class
