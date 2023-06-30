
Option Explicit On
Option Infer Off
Option Strict On

Public Module _control_c_trace
    Public ReadOnly control_c_trace As Boolean = env_bool(env_keys("control", "c", "trace"))
End Module
