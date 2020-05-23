
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class break_lambda
    Inherits Exception

    Public Shared ReadOnly instance As break_lambda

    Shared Sub New()
        instance = New break_lambda()
    End Sub

    Private Sub New()
    End Sub
End Class
