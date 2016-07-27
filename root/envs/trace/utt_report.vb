
Public Module _utt_report
    Public ReadOnly utt_report_case_name As Boolean
    Public ReadOnly utt_report_background_worker_status As Boolean
    Public ReadOnly utt_report_memory_status As Boolean

    Sub New()
        utt_report_case_name = env_bool(env_keys("utt", "report", "case", "name"))
        utt_report_background_worker_status = env_bool(env_keys("utt", "report", "background", "worker", "status"))
        utt_report_memory_status = env_bool(env_keys("utt", "report", "memory", "status")) OrElse
                                   env_bool(env_keys("utt", "report", "memory"))
    End Sub
End Module
