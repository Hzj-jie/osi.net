
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class bool_stream
    Public NotInheritable Class aggregators
        Public Shared ReadOnly all_true As Func(Of Boolean, Boolean, Boolean) =
            Function(ByVal l As Boolean, ByVal r As Boolean) As Boolean
                Return l AndAlso r
            End Function

        Private Sub New()
        End Sub
    End Class

    Private Sub New()
    End Sub
End Class
