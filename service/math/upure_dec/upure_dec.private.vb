
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class upure_dec
    Private Sub reduct_of_fraction()
        For i As Int32 = 0 To prime_count - 1
            Dim n As big_uint = Nothing
            Dim d As big_uint = Nothing
            n = Me.n.CloneT()
            d = Me.d.CloneT()
            Dim r As UInt32 = Nothing
            n.assert_divide(prime(i), r)
            If r <> 0 Then
                Continue For
            End If
            d.assert_divide(prime(i), r)
            If r = 0 Then
                replace_by(n, d)
            End If
        Next
    End Sub
End Class
