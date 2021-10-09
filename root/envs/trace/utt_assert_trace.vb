
Option Explicit On
Option Infer Off
Option Strict On

Public Module _utt_assert_trace
    Public ReadOnly utt_no_assert As Boolean = env_bool(env_keys("utt", "no", "assert"))
    Public ReadOnly utt_no_debug_mode As Boolean = env_bool(env_keys("utt", "no", "debug", "mode"))
End Module
