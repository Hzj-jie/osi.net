﻿
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class big_udec
    Private Sub reduce_fraction()
        If is_zero() Then
            set_zero()
            Return
        End If
        If is_one() Then
            set_one()
            Return
        End If
        For i As Int32 = 0 To prime_count - 1
            While True
                Dim n As big_uint = Nothing
                Dim d As big_uint = Nothing
                n = Me.n.CloneT()
                d = Me.d.CloneT()
                Dim r As UInt32 = Nothing
                n.assert_divide(prime(i), r)
                If r <> 0 Then
                    Exit While
                End If
                d.assert_divide(prime(i), r)
                If r <> 0 Then
                    Exit While
                End If
                replace_by(n, d)
            End While
        Next
    End Sub
End Class
