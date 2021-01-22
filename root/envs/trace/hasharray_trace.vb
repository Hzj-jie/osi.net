
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class hasharray_trace
    Public Shared ReadOnly log_rehash As Boolean = env_bool(env_keys("log", "hasharray", "rehash"))

    Private Sub New()
    End Sub
End Class
