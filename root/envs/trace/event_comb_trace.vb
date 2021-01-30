
Option Explicit On
Option Infer Off
Option Strict On

Public Module _event_comb_trace
    Public ReadOnly event_comb_trace As Boolean = env_bool(env_keys("event", "comb", "trace"))
    Public ReadOnly event_comb_alloc_trace As Boolean =
        event_comb_trace OrElse
        env_bool(env_keys("event", "comb", "alloc", "trace"))
    Public ReadOnly event_comb_full_alloc_stack As Boolean =
        env_bool(env_keys("event", "comb", "full", "alloc", "stack"))
End Module
