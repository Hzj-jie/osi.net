
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.constants.system_perf

<global_init(global_init_level.max)>
Public Module _system_perf
    Public ReadOnly perf_run_ms As Int64 = calculate_perf_run_ms()
    Public ReadOnly loops_per_ms As Int64 = ratio \ perf_run_ms

    Private Function perf_run_single() As Int64
        Dim startticks As Int64 = 0
        startticks = nowadays.high_res_ticks()
        fibonacci.run()
        atomic_operator.run()
#If 0 Then
        thread_static_operator.run()
#End If
        memory_access.run()
        integer_operator.run()
        float_operator.run()
        Return nowadays.high_res_ticks() - startticks
    End Function

    Private Function perf_run() As Int64
        'warmup
        perf_run_single()
        Dim min As Int64 = max_int64
        For i As Int32 = 1 To 16
            Dim c As Int64 = 0
            c = perf_run_single()
            If c < min Then
                min = c
            End If
        Next
        Return max(ticks_to_milliseconds(min), 1)
    End Function

    Private Function calculate_perf_run_ms() As Int64
        assert(error_writer_ignore_types(Of colorful_console_error_writer).valued(error_type.warning))
        Using New boost()
            Return perf_run()
        End Using
    End Function

    Private Sub init()
        If env_bool(env_keys("report", "system", "perf")) Then
            ' system_perf will be initialized before file_error_writer, so ensure it won't be ignored by
            ' colorful_console_error_writer.
            raise_error(error_type.warning,
                        "perf_run_ms = ",
                          perf_run_ms,
                          ", loops_per_ms = ",
                          loops_per_ms)
        End If
    End Sub
End Module
