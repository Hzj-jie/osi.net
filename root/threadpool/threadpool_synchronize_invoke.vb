
Imports osi.root.connector

Public NotInheritable Class threadpool_synchronize_invoke
    Inherits synchronize_invoke

    Protected Overrides Sub push(ByVal v As Action)
        assert(thread_pool().queue_job(v))
    End Sub
End Class
