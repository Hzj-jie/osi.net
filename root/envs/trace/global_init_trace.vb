
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class global_init_trace
    Public Shared ReadOnly log_detail_time_consumption As Boolean =
        env_bool(env_keys("log", "global", "init", "detail", "time", "consumption"))

    Public Shared ReadOnly log_time_consumption As Boolean = log_detail_time_consumption OrElse
        env_bool(env_keys("log", "global", "init", "time", "consumption"))

    Private Sub New()
    End Sub
End Class
