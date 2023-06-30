
Option Explicit On
Option Infer Off
Option Strict On

Public Module _promise_trace
    Public ReadOnly promise_trace As Boolean = env_bool(env_keys("promise", "trace"))
End Module
