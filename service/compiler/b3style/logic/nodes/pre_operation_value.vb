
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class b3style
    Private NotInheritable Class pre_operation_value
        Inherits unary_operation_value

        Public Sub New()
            MyBase.New(0, "_pre")
        End Sub
    End Class
End Class
