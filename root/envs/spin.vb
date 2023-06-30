
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

<global_init(global_init_level.other)>
Public Module _spin
    Public ReadOnly suggest_try_wait_length_ms As Int32 = If(single_cpu, 0, CInt(max(timeslice_length_ms \ 16, 1)))
    Public ReadOnly loops_per_yield As Int32 = decide_loops_per_yield()
    Private Const default_loops_per_yield As Int32 = 512

    Private Function decide_loops_per_yield() As Int32
        If single_cpu Then
            Return 1
        End If
        Const ratio As Double = 2
        Const base As Int32 = 4
        Dim r As Double = max(loops_per_ms >> 12, default_loops_per_yield)
        If Environment.ProcessorCount() < base Then
            r /= Math.Pow(ratio, (base / Environment.ProcessorCount()) - 1)
        End If
        Return If(r > max_int32 OrElse r <= 0, default_loops_per_yield, CInt(r))
    End Function

    Private Sub init()
        If env_bool(env_keys("report", "spin", "variables")) Then
            raise_error("loops_per_yield = ",
                          loops_per_yield,
                          ", suggest_try_wait_length_ms = ",
                          suggest_try_wait_length_ms)
        End If
    End Sub
End Module
