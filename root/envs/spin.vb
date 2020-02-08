
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector

Public Module _spin
    Public ReadOnly suggest_try_wait_length_ms As Int32 = 0
    Public ReadOnly loops_per_yield As Int32 = 0
    Private Const default_loops_per_yield As Int32 = 512

    Private Function decide_loops_per_yield() As Int32
        If single_cpu Then
            Return 1
        Else
            Const ratio As Double = 2
            Const base As Int32 = 4
            Dim r As Double = 0
            r = max(loops_per_ms >> 12, default_loops_per_yield)
            If Environment.ProcessorCount() < base Then
                r /= Math.Pow(ratio, (base / Environment.ProcessorCount()) - 1)
            End If
            Return If(r > max_int32 OrElse r <= 0, default_loops_per_yield, CInt(r))
        End If
    End Function

    Sub New()
        If single_cpu Then
            suggest_try_wait_length_ms = 0
        Else
            suggest_try_wait_length_ms = CInt(sixteenth_timeslice_length_ms)
        End If
        loops_per_yield = decide_loops_per_yield()

        If env_bool(env_keys("report", "spin", "variables")) Then
            raise_error("loops_per_yield = ",
                          loops_per_yield,
                          ", suggest_try_wait_length_ms = ",
                          suggest_try_wait_length_ms)
        End If
    End Sub
End Module
