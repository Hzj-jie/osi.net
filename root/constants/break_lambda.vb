
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class break_lambda
    Inherits Exception

    Private Shared ReadOnly instance As break_lambda

    Shared Sub New()
        instance = New break_lambda()
    End Sub

    Public Shared Sub at_here()
        Throw instance
    End Sub

    Private Sub New()
    End Sub
End Class
