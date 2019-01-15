
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.envs

Friend NotInheritable Class env_vars
    Public Shared ReadOnly run_flaky_tests As Boolean
    Public Shared ReadOnly utt_report_case_name As Boolean
    Public Shared ReadOnly utt_report_background_worker_status As Boolean
    Public Shared ReadOnly utt_report_memory_status As Boolean
    Public Shared ReadOnly repeat_per_case As UInt32

    Shared Sub New()
        run_flaky_tests = env_bool(env_keys("run", "flaky", "tests")) OrElse
                          env_bool(env_keys("run", "flaky", "cases"))
        utt_report_case_name = env_bool(env_keys("utt", "report", "case", "name"))
        utt_report_background_worker_status = env_bool(env_keys("utt", "report", "background", "worker", "status"))
        utt_report_memory_status = env_bool(env_keys("utt", "report", "memory", "status")) OrElse
                                   env_bool(env_keys("utt", "report", "memory"))
        repeat_per_case = utt_round_per_case()
    End Sub

    Private Shared Function utt_round_per_case() As UInt32
        Dim r As Int32 = 0
        If Not env_value(env_keys("utt", "repeat"), r) OrElse r <= 0 Then
            Return uint32_1
        End If
        Return CUInt(r)
    End Function

    Private Sub New()
    End Sub
End Class
