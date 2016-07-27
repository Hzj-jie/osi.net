
Public Module _utt_assert_trace
    Public ReadOnly utt_no_assert As Boolean

    Sub New()
        utt_no_assert = env_bool(env_keys("utt", "no", "assert"))
    End Sub
End Module
