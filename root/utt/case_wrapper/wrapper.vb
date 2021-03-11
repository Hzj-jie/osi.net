
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public Module _wrapper
    Public Function gc_memory_measured(ByVal c As [case], ByVal interval_ms As Int64) As gc_memory_measured_case_wrapper
        Return New gc_memory_measured_case_wrapper(c, interval_ms)
    End Function

    Public Function gc_memory_measured(ByVal c As [case]) As gc_memory_measured_case_wrapper
        Return New gc_memory_measured_case_wrapper(c)
    End Function

    Public Function memory_measured(ByVal c As [case], ByVal interval_ms As Int64) As memory_measured_case_wrapper
        Return New memory_measured_case_wrapper(c, interval_ms)
    End Function

    Public Function memory_measured(ByVal c As [case]) As memory_measured_case_wrapper
        Return New memory_measured_case_wrapper(c)
    End Function

    Public Function multithreading(ByVal c As [case], ByVal threadcount As Int32) As multithreading_case_wrapper
        assert(threadcount >= 0)
        Return multithreading(c, CUInt(threadcount))
    End Function

    Public Function multithreading(ByVal c As [case], ByVal threadcount As UInt32) As multithreading_case_wrapper
        Return New multithreading_case_wrapper(c, threadcount)
    End Function

    Public Function multithreading(ByVal c As [case]) As multithreading_case_wrapper
        Return New multithreading_case_wrapper(c)
    End Function

    Public Function multi_procedure(ByVal c As event_comb_case,
                                    ByVal procedure_count As Int32) As multi_procedure_case_wrapper
        assert(procedure_count >= 0)
        Return multi_procedure(c, CUInt(procedure_count))
    End Function

    Public Function multi_procedure(ByVal c As event_comb_case,
                                    ByVal procedure_count As UInt32) As multi_procedure_case_wrapper
        Return New multi_procedure_case_wrapper(c, procedure_count)
    End Function

    Public Function multi_procedure(ByVal c As event_comb_case) As multi_procedure_case_wrapper
        Return New multi_procedure_case_wrapper(c)
    End Function

    Public Function performance(ByVal c As [case],
                                Optional ByVal max_loops As UInt64 = performance_case_wrapper.undetermined_max_loops,
                                Optional ByVal min_loops As UInt64 = 0,
                                Optional ByVal times As UInt64 = 1) As performance_case_wrapper
        Return New performance_case_wrapper(c, max_loops, min_loops, times)
    End Function

    Public Function processor_measured(ByVal c As [case], ByVal interval_ms As Int64) As processor_measured_case_wrapper
        Return New processor_measured_case_wrapper(c, interval_ms)
    End Function

    Public Function processor_measured(ByVal c As [case]) As processor_measured_case_wrapper
        Return New processor_measured_case_wrapper(c)
    End Function

    Public Function repeat(ByVal c As [case], ByVal testsize As UInt64) As repeat_case_wrapper
        Return New repeat_case_wrapper(c, testsize)
    End Function

    Public Function repeat(ByVal c As [case], ByVal testsize As Int64) As repeat_case_wrapper
        Return repeat(c, assert_which.of(testsize).can_cast_to_uint64())
    End Function

    Public Function repeat(ByVal c As [case]) As repeat_case_wrapper
        Return New repeat_case_wrapper(c)
    End Function

    Public Function repeat(ByVal c As event_comb_case, ByVal testsize As Int64) As repeat_event_comb_case_wrapper
        Return repeat(c, assert_which.of(testsize).can_cast_to_uint64())
    End Function

    Public Function repeat(ByVal c As event_comb_case, ByVal testsize As UInt64) As repeat_event_comb_case_wrapper
        Return New repeat_event_comb_case_wrapper(c, testsize)
    End Function

    Public Function repeat(ByVal c As event_comb_case) As repeat_event_comb_case_wrapper
        Return New repeat_event_comb_case_wrapper(c)
    End Function

    Public Function rinne(ByVal c As [case], ByVal testsize As Int64) As rinne_case_wrapper
        Return New rinne_case_wrapper(c, testsize)
    End Function

    Public Function rinne(ByVal c As [case]) As rinne_case_wrapper
        Return New rinne_case_wrapper(c)
    End Function

    Public Function chained(ByVal ParamArray cs() As [case]) As chained_case_wrapper
        Return New chained_case_wrapper(cs)
    End Function

    Public Function chained(ByVal continue_when_failure As Boolean,
                            ByVal ParamArray cs() As [case]) As chained_case_wrapper
        Return New chained_case_wrapper(continue_when_failure, cs)
    End Function

    Public Function realtime(ByVal c As [case]) As realtime_wrapper
        Return New realtime_wrapper(c)
    End Function

    Public Function realtime_wrappered(ByVal c As [case]) As realtime_wrapper
        Return realtime(c)
    End Function

    Public Function sleep_wrappered(ByVal c As [case], ByVal ms As Int64) As sleep_case_wrapper
        Return New sleep_case_wrapper(c, ms)
    End Function

    Public Function sleep_wrappered(ByVal ms As Int64, ByVal c As [case]) As sleep_case_wrapper
        Return New sleep_case_wrapper(ms, c)
    End Function

    Public Function sleep_wrappered(ByVal c As [case], ByVal ms As Func(Of Int64)) As sleep_case_wrapper
        Return New sleep_case_wrapper(c, ms)
    End Function

    Public Function sleep_wrappered(ByVal ms As Func(Of Int64), ByVal c As [case]) As sleep_case_wrapper
        Return New sleep_case_wrapper(ms, c)
    End Function

    Public Function isolated(ByVal c As commandline_specified_case_wrapper) As isolate_case_wrapper
        Return New isolate_case_wrapper(c)
    End Function

    Public Function memory_usage_limited(ByVal c As [case],
                                         ByVal expected_memory_usage As Int64) As memory_usage_limited_case_wrapper
        Return New memory_usage_limited_case_wrapper(c, expected_memory_usage)
    End Function

    Public Function commandline_specified(ByVal c As [case]) As commandline_specified_case_wrapper
        Return New commandline_specified_case_wrapper(c)
    End Function

    Public Function commandline_specified(ByVal c As [case],
                                          ByVal full_name As String,
                                          ByVal assembly_qualified_name As String,
                                          ByVal name As String) As commandline_specified_case_wrapper
        Return New commandline_specified_case_wrapper(c, full_name, assembly_qualified_name, name)
    End Function

    Public Function flaky(ByVal c As [case]) As flaky_case_wrapper
        Return New flaky_case_wrapper(c)
    End Function
End Module
