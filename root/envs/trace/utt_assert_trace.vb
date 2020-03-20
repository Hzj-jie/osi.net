
Option Explicit On
Option Infer Off
Option Strict On

Public Module _utt_assert_trace
    Public ReadOnly utt_no_assert As Boolean
    Public ReadOnly utt_no_debug_mode As Boolean

    Sub New()
        utt_no_assert = env_bool(env_keys("utt", "no", "assert"))
        utt_no_debug_mode = env_bool(env_keys("utt", "no", "debug", "mode"))
    End Sub
End Module
