
Imports osi.root.connector

Public NotInheritable Class queue_runner_synchronize_invoke
    Inherits synchronize_invoke

    Protected Overrides Sub push(ByVal v As Action)
        assert(queue_runner.once(v))
    End Sub
End Class
