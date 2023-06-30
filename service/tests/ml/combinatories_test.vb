
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.ml

<test>
Public NotInheritable Class combinatories_test
    <test>
    Private Shared Sub run()
        assertion.equal(combinatories.of(100, 1), CULng(100))
        assertion.equal(combinatories.of(100, 5), CULng(75287520))
        assertion.equal(combinatories.of(100, 0), CULng(1))
        assertion.equal(combinatories.of(100, 100), CULng(1))
        assertion.equal(combinatories.of(100, 95), CULng(75287520))
        assertion.equal(combinatories.of(100, 99), CULng(100))
    End Sub

    Private Sub New()
    End Sub
End Class
