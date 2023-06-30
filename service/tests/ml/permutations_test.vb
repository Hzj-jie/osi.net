
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.ml

<test>
Public NotInheritable Class permutations_test
    <test>
    Private Shared Sub run()
        assertion.equal(permutations.of(100, 0), CULng(1))
        assertion.equal(permutations.of(100, 1), CULng(100))
        assertion.equal(permutations.of(100, 2), CULng(9900))
        assertion.equal(permutations.of(100, 5), CULng(9034502400))
    End Sub

    Private Sub New()
    End Sub
End Class
