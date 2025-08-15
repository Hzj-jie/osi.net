
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt.attributes

<test>
Public NotInheritable Class running_myself_test
    <test>
    Private Shared Sub run()
        If assertion.is_not_null(host.current_case()) Then
            assertion.equal(host.current_case().name, "running_myself_test")
        End If
    End Sub

    Private Sub New()
    End Sub
End Class
