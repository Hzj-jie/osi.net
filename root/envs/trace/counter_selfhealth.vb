
Option Explicit On
Option Infer Off
Option Strict On

Public Module _counter_selfhealth
    Public ReadOnly counter_selfhealth As Boolean = env_bool(env_keys("counter", "selfhealth"))
End Module
