
Public Module _callback_trace
    Public ReadOnly callback_trace As Boolean = False

    Sub New()
        callback_trace = env_bool(env_keys("callback", "trace")) OrElse
                         env_bool(env_keys("callback", "action", "trace"))
    End Sub
End Module
