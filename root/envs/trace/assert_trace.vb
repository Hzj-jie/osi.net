
Public Module _assert_trace
    Public ReadOnly assert_trace As Boolean = False

    Sub New()
        assert_trace = env_bool(env_keys("strong", "assert")) OrElse
                       env_bool(env_keys("assert", "trace"))
    End Sub
End Module
