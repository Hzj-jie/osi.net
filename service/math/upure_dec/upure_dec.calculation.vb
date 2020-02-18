
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class upure_dec
    Public Function add(ByVal that As upure_dec, ByRef carryover As Boolean) As upure_dec
        carryover = False
        If that Is Nothing OrElse that.is_zero() Then
            Return Me
        End If
        If is_zero() Then
            replace_by(that)
            Return Me
        End If
        Dim n As big_uint = Nothing
        n = Me.n * that.d + Me.d * that.n
        Dim d As big_uint = Nothing
        d = Me.d * that.d
        If n >= d Then
            n -= d
            carryover = True
        End If
        replace_by(n, d)
        reduct_of_fraction()
        Return Me
    End Function
End Class
