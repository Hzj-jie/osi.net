
Option Explicit On
Option Infer Off
Option Strict On

Public Module _lock_trace
    Public ReadOnly lock_trace As Boolean = env_bool(env_keys("lock", "trace"))
End Module
