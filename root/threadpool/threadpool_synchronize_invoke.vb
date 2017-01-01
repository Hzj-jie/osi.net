
Imports osi.root.connector

Public NotInheritable Class threadpool_synchronize_invoke
    Inherits synchronize_invoke

    Public Shared ReadOnly instance As threadpool_synchronize_invoke

    Shared Sub New()
        instance = New threadpool_synchronize_invoke()
    End Sub

    Private Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Sub push(ByVal v As Action)
        assert(thread_pool().queue_job(v))
    End Sub
End Class
