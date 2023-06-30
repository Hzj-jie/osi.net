
Imports osi.root.envs
Imports osi.root.utils
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.utt
Imports osi.root.connector
Imports osi.root.formation

Public Class system_perf_test
    Inherits [case]

    Public Overrides Function reserved_processors() As Int16
        Return Environment.ProcessorCount()
    End Function

    Private Overloads Shared Sub run(ByVal d As Action, ByVal name As String, ByRef acc As Int64)
        assert(Not d Is Nothing)
        assert(Not name.null_or_empty())
        Const round As Int32 = 128
        Dim min As Int64 = max_int64
        Dim max As Int64 = min_int64
        Dim total As Int64 = 0
        For i As Int32 = 0 To round - 1
            Dim p As ref(Of Int64) = Nothing
            p = New ref(Of Int64)()
            Using New hires_ticks_timing_counter(p)
                d()
            End Using
            If (+p) < min Then
                min = (+p)
            End If
            If (+p) > max Then
                max = (+p)
            End If
            total += (+p)
        Next

        raise_error("finished ",
                      name,
                      " with ",
                      round,
                      " rounds, min = ",
                      min,
                      ", max = ",
                      max,
                      ", average = ",
                      total \ round)
        acc += min
    End Sub

    Public Overrides Function run() As Boolean
        Dim t As Int64 = 0
        Using New boost()
            run(AddressOf fibonacci.run, "fibonacci", t)
            run(AddressOf atomic_operator.run, "atomic_operator", t)
#If 0 Then
            run(AddressOf thread_static_operator.run, "thread_static_operator", t)
#End If
            run(AddressOf memory_access.run, "memory_access", t)
            run(AddressOf integer_operator.run, "integer_operator", t)
            run(AddressOf float_operator.run, "float_operator", t)
        End Using
        raise_error("overall min ticks = ", t)
        Return True
    End Function
End Class
