
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt.attributes

<test>
Public NotInheritable Class running_myself_test
    <test>
    Private Shared Sub run()
        If assertion.is_false(current_case.is_null()) Then
            assertion.equal(current_case.[of]().name, "running_myself_test")
        End If
    End Sub

    Private Sub New()
    End Sub
End Class
