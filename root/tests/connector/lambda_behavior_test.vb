
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class lambda_behavior_test
    Private Sub New()
    End Sub

    <test>
    Private Shared Sub should_capture_latest_value()
        Dim c As Object = Nothing
        Dim f As Action = Nothing
        f = Sub()
                c = New Object()
            End Sub
        Dim g As Action = Nothing
        g = Sub()
                assert_not_nothing(c)
            End Sub

        f()
        g()
    End Sub
End Class
