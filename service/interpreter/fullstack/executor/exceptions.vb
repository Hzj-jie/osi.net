
Namespace fullstack.executor
    Public Class break_exception
        Inherits Exception

        Public Shared ReadOnly instance As break_exception

        Shared Sub New()
            instance = New break_exception()
        End Sub

        Private Sub New()
        End Sub
    End Class

    Public Class return_exception
        Inherits Exception

        Public Shared ReadOnly instance As return_exception

        Shared Sub New()
            instance = New return_exception()
        End Sub

        Private Sub New()
        End Sub
    End Class

    Public Class invalid_runtime_casting_exception
        Inherits Exception

        Public Shared ReadOnly instance As invalid_runtime_casting_exception

        Shared Sub New()
            instance = New invalid_runtime_casting_exception()
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
