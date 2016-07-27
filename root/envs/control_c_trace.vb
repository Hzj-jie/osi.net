
Public Module _control_c_trace
    Public ReadOnly control_c_trace As Boolean = False

    Sub New()
        control_c_trace = env_bool(env_keys("control", "c", "trace"))
    End Sub
End Module
