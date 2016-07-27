
Public Module _busy_wait
    Public ReadOnly busy_wait As Boolean = False

    Sub New()
        busy_wait = Not single_cpu AndAlso
                    env_bool(env_keys("busy", "wait"))
    End Sub
End Module
