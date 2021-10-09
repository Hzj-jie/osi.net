
Option Explicit On
Option Infer Off
Option Strict On

Public Module _assert_trace
    Public ReadOnly assert_trace As Boolean =
        env_bool(env_keys("strong", "assert")) OrElse
        env_bool(env_keys("assert", "trace"))
End Module
