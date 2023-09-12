
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class b2style
    Private NotInheritable Class post_operation_value
        Inherits unary_operation_value

        Public Sub New()
            MyBase.New(1, "_post")
        End Sub
    End Class
End Class
