
Public Module _event_comb_trace
    Public ReadOnly event_comb_trace As Boolean = False
    Public ReadOnly event_comb_alloc_trace As Boolean = False
    Public ReadOnly event_comb_full_alloc_stack As Boolean = False

    Sub New()
        event_comb_trace = env_bool(env_keys("event", "comb", "trace"))
        event_comb_alloc_trace = event_comb_trace OrElse
                                 env_bool(env_keys("event", "comb", "alloc", "trace"))
        event_comb_full_alloc_stack = env_bool(env_keys("event", "comb", "full", "alloc", "stack"))
    End Sub
End Module
