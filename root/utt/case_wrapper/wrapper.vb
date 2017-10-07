
Option Explicit On
Option Infer Off
Option Strict On

Public Module _wrapper
    Public Function gc_memory_measured(ByVal c As [case], ByVal interval_ms As Int64) As [case]
        Return New gc_memory_measured_case_wrapper(c, interval_ms)
    End Function

    Public Function gc_memory_measured(ByVal c As [case]) As [case]
        Return New gc_memory_measured_case_wrapper(c)
    End Function

    Public Function memory_measured(ByVal c As [case], ByVal interval_ms As Int64) As [case]
        Return New memory_measured_case_wrapper(c, interval_ms)
    End Function

    Public Function memory_measured(ByVal c As [case]) As [case]
        Return New memory_measured_case_wrapper(c)
    End Function

    Public Function multithreading(ByVal c As [case], ByVal threadcount As Int32) As [case]
        Return New multithreading_case_wrapper(c, threadcount)
    End Function

    Public Function multithreading(ByVal c As [case]) As [case]
        Return New multithreading_case_wrapper(c)
    End Function

    Public Function multi_procedure(ByVal c As event_comb_case, ByVal procedure_count As Int32) As event_comb_case
        Return New multi_procedure_case_wrapper(c, procedure_count)
    End Function

    Public Function multi_procedure(ByVal c As event_comb_case) As event_comb_case
        Return New multi_procedure_case_wrapper(c)
    End Function

    Public Function performance(ByVal c As [case],
                                Optional ByVal max_loops As UInt64 = performance_case_wrapper.undetermined_max_loops,
                                Optional ByVal min_loops As UInt64 = 0,
                                Optional ByVal times As UInt64 = 1) As [case]
        Return New performance_case_wrapper(c, max_loops, min_loops, times)
    End Function

    Public Function processor_measured(ByVal c As [case], ByVal interval_ms As Int64) As [case]
        Return New processor_measured_case_wrapper(c, interval_ms)
    End Function

    Public Function processor_measured(ByVal c As [case]) As [case]
        Return New processor_measured_case_wrapper(c)
    End Function

    Public Function repeat(ByVal c As [case], ByVal testsize As Int64) As [case]
        Return New repeat_case_wrapper(c, testsize)
    End Function

    Public Function repeat(ByVal c As [case]) As [case]
        Return New repeat_case_wrapper(c)
    End Function

    Public Function repeat(ByVal c As event_comb_case, ByVal testsize As Int64) As event_comb_case
        Return New repeat_event_comb_case_wrapper(c, testsize)
    End Function

    Public Function repeat(ByVal c As event_comb_case) As event_comb_case
        Return New repeat_event_comb_case_wrapper(c)
    End Function

    Public Function rinne(ByVal c As [case], ByVal testsize As Int64) As [case]
        Return New rinne_case_wrapper(c, testsize)
    End Function

    Public Function rinne(ByVal c As [case]) As [case]
        Return New rinne_case_wrapper(c)
    End Function

    Public Function chained(ByVal ParamArray cs() As [case]) As [case]
        Return New chained_case_wrapper(cs)
    End Function

    Public Function chained(ByVal continue_when_failure As Boolean, ByVal ParamArray cs() As [case]) As [case]
        Return New chained_case_wrapper(continue_when_failure, cs)
    End Function

    Public Function realtime(ByVal c As [case]) As [case]
        Return New realtime_wrapper(c)
    End Function

    Public Function realtime_wrappered(ByVal c As [case]) As [case]
        Return realtime(c)
    End Function

    Public Function sleep_wrappered(ByVal c As [case], ByVal ms As Int64) As [case]
        Return New sleep_case_wrapper(c, ms)
    End Function

    Public Function sleep_wrappered(ByVal ms As Int64, ByVal c As [case]) As [case]
        Return New sleep_case_wrapper(ms, c)
    End Function

    Public Function sleep_wrappered(ByVal c As [case], ByVal ms As Func(Of Int64)) As [case]
        Return New sleep_case_wrapper(c, ms)
    End Function

    Public Function sleep_wrappered(ByVal ms As Func(Of Int64), ByVal c As [case]) As [case]
        Return New sleep_case_wrapper(ms, c)
    End Function

    Public Function isolated(ByVal c As commandline_specified_case_wrapper) As [case]
        Return New isolate_case_wrapper(c)
    End Function

    Public Function memory_usage_limited(ByVal c As [case], ByVal expected_memory_usage As Int64) As [case]
        Return New memory_usage_limited_case_wrapper(c, expected_memory_usage)
    End Function

    Public Function commandline_specified(ByVal c As [case]) As [case]
        Return New commandline_specified_case_wrapper(c)
    End Function

    Public Function flaky(ByVal c As [case]) As [case]
        Return New flaky_case_wrapper(c)
    End Function
End Module
