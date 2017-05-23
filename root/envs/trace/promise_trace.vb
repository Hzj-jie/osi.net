
Option Explicit On
Option Infer Off
Option Strict On

Public Module _promise_trace
    Public ReadOnly promise_trace As Boolean

    Sub New()
        promise_trace = env_bool(env_keys("promise", "trace"))
    End Sub
End Module
