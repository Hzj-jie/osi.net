
Option Explicit On
Option Infer Off
Option Strict On

Public Module _busy_wait
    Public ReadOnly busy_wait As Boolean = Not single_cpu AndAlso env_bool(env_keys("busy", "wait"))
End Module
