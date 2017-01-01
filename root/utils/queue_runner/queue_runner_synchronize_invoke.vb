
Imports osi.root.connector

Public NotInheritable Class queue_runner_synchronize_invoke
    Inherits synchronize_invoke

    Public Shared ReadOnly instance As queue_runner_synchronize_invoke

    Shared Sub New()
        instance = New queue_runner_synchronize_invoke()
    End Sub

    Private Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Sub push(ByVal v As Action)
        assert(queue_runner.once(v))
    End Sub

    Protected Overrides Function synchronously() As Boolean
        Return queue_runner.running_in_current_thread()
    End Function
End Class
