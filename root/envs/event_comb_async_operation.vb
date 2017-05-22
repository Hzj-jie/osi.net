
Option Explicit On
Option Infer Off
Option Strict On

Public Module _event_comb_async_operation
    Public ReadOnly use_promise_for_event_comb_async_operation As Boolean

    Sub New()
        use_promise_for_event_comb_async_operation = env_bool(env_keys("use",
                                                                       "promise",
                                                                       "for",
                                                                       "event",
                                                                       "comb",
                                                                       "async",
                                                                       "operation"))
    End Sub
End Module
