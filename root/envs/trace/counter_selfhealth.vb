
Public Module _counter_selfhealth
    Public ReadOnly counter_selfhealth As Boolean

    Sub New()
        counter_selfhealth = env_bool(env_keys({"counter", "selfhealth"}))
    End Sub
End Module
