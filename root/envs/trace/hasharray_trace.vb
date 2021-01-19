
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class hasharray_trace
    Public Shared ReadOnly log_rehash As Boolean

    Shared Sub New()
        log_rehash = env_bool(env_keys("log", "hasharray", "rehash"))
    End Sub

    Private Sub New()
    End Sub
End Class
