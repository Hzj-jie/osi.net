
Option Explicit On
Option Infer Off
Option Strict On

<Serializable>
Public NotInheritable Class break_lambda
    Inherits Exception

    Private Shared ReadOnly instance As break_lambda = New break_lambda()

    Public Shared Sub at_here()
        Throw instance
    End Sub

    Private Sub New()
    End Sub
End Class
