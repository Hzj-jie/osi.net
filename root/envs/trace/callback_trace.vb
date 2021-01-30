
Option Explicit On
Option Infer Off
Option Strict On

Public Module _callback_trace
    Public ReadOnly callback_trace As Boolean =
        env_bool(env_keys("callback", "trace")) OrElse
        env_bool(env_keys("callback", "action", "trace"))
End Module
