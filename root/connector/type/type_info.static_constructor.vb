
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class type_info(Of T)
    Private NotInheritable Class static_constructor_cache
        Public Shared ReadOnly s As static_constructor

        Shared Sub New()
            s = New static_constructor(GetType(T))
        End Sub

        Private Sub New()
        End Sub
    End Class
End Class
