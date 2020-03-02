
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class big_dec
    Private Sub set_signal(ByVal v As Boolean)
        If d.is_zero() Then
            s = True
        Else
            s = v
        End If
        'make sure the logic is correct to set the signal as true when is_zero
        is_zero()
    End Sub

    Private Sub confirm_signal()
        set_signal(signal())
    End Sub
End Class
