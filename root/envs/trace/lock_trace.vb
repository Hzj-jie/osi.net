
Public Module _lock_trace
    Public ReadOnly lock_trace As Boolean = False

    Sub New()
        lock_trace = env_bool(env_keys("lock", "trace"))
    End Sub
End Module
