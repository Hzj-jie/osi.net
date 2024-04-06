
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.envs

Friend NotInheritable Class env_vars
    Public Shared ReadOnly run_flaky_tests As Boolean =
        env_bool(env_keys("run", "flaky", "tests")) OrElse
        env_bool(env_keys("run", "flaky", "cases"))
    Public Shared ReadOnly utt_report_case_name As Boolean =
        env_bool(env_keys("utt", "report", "case", "name"))
    Public Shared ReadOnly utt_report_background_worker_status As Boolean =
        env_bool(env_keys("utt", "report", "background", "worker", "status"))
    Public Shared ReadOnly utt_report_memory_status As Boolean =
        env_bool(env_keys("utt", "report", "memory", "status")) OrElse
        env_bool(env_keys("utt", "report", "memory"))
    Public Shared ReadOnly repeat_per_case As UInt32 =
        Function() As UInt32
            Dim r As Int32 = 0
            If Not env_value(env_keys("utt", "repeat"), r) OrElse r <= 0 Then
                Return uint32_1
            End If
            Return CUInt(r)
        End Function()
    Public Shared ReadOnly utt_memory_limit As UInt64 =
        Function() As UInt64
            Dim r As Int64 = 0
            If Not env_value(env_keys("utt", "memory", "limit"), r) OrElse r < 0 Then
                Return 0
            End If
            Return CULng(r)
        End Function()

    Private Sub New()
    End Sub
End Class
