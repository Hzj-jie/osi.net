
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt

Friend Class nowadays_case
    Inherits [case]

    Private Const acceptable_difference_ms As UInt32 = 500
    Public Shared ReadOnly just_run As [case]

    Shared Sub New()
        just_run = New delegate_case(Sub()
                                         nowadays.low_res_milliseconds()
                                         nowadays.normal_res_milliseconds()
                                         nowadays.high_res_milliseconds()
                                         nowadays.low_res_ticks()
                                         nowadays.normal_res_ticks()
                                         nowadays.high_res_ticks()
                                     End Sub)
    End Sub

    Public Overrides Function run() As Boolean
        If Not assertion.more_or_equal_and_less_or_equal(nowadays.low_res_milliseconds() -
                                                      nowadays.normal_res_milliseconds() -
                                                      nowadays.normal_res_milliseconds() +
                                                      nowadays.low_res_milliseconds(),
                                                      -acceptable_difference_ms,
                                                      acceptable_difference_ms,
                                                      "nowadays.low_res_milliseconds()") Then
            low_res_ticks_retriever.force_revise()
        End If
        If Not assertion.more_or_equal_and_less_or_equal(nowadays.normal_res_milliseconds() -
                                                      nowadays.high_res_milliseconds() -
                                                      nowadays.high_res_milliseconds() +
                                                      nowadays.normal_res_milliseconds(),
                                                      -acceptable_difference_ms,
                                                      acceptable_difference_ms,
                                                      "nowadays.high_res_milliseconds()") Then
            high_res_ticks_retriever.force_revise()
        End If
        'low_res_ticks is faked as milliseconds_to_ticks(low_res_milliseconds)
        If Not assertion.more_or_equal_and_less_or_equal(nowadays.low_res_ticks() -
                                                      nowadays.normal_res_ticks() -
                                                      nowadays.normal_res_ticks() +
                                                      nowadays.low_res_ticks(),
                                                      milliseconds_to_ticks(-acceptable_difference_ms),
                                                      milliseconds_to_ticks(acceptable_difference_ms),
                                                      "nowadays.low_res_ticks()") Then
            low_res_ticks_retriever.force_revise()
        End If
        If Not assertion.more_or_equal_and_less_or_equal(nowadays.normal_res_ticks() -
                                                      nowadays.high_res_ticks() -
                                                      nowadays.high_res_ticks() +
                                                      nowadays.normal_res_ticks(),
                                                      milliseconds_to_ticks(-acceptable_difference_ms),
                                                      milliseconds_to_ticks(acceptable_difference_ms),
                                                      "nowadays.high_res_ticks()") Then
            high_res_ticks_retriever.force_revise()
        End If
        Return True
    End Function
End Class

Public Class nowadays_test
    Inherits realtime_wrapper

    Public Sub New()
        MyBase.New(repeat(New nowadays_case(), 1024 * 1024))
    End Sub
End Class

Public Class nowadays_specified_test_10
    Inherits commandline_specified_case_wrapper

    Friend Sub New(ByVal sleep_ms As Int64)
        MyBase.New(realtime_wrappered(repeat(sleep_wrappered(New nowadays_case(),
                                                             sleep_ms),
                                             50 * 24 * 60 * 60)))
    End Sub

    Public Sub New()
        Me.New(10)
    End Sub
End Class

Public Class nowadays_specified_test_100
    Inherits nowadays_specified_test_10

    Public Sub New()
        MyBase.New(100)
    End Sub
End Class

Public Class nowadays_specified_test_1000
    Inherits nowadays_specified_test_10

    Public Sub New()
        MyBase.New(1000)
    End Sub
End Class

Public Class nowadays_stability_test
    Inherits case_wrapper

    Public Sub New()
        MyBase.New(multithreading(repeat(nowadays_case.just_run,
                                         256 * 1024),
                                  Environment.ProcessorCount() << 2))
    End Sub
End Class

Public Class nowadays_combined_test
    Inherits case_wrapper

    Friend Sub New(ByVal round As Int64)
        MyBase.New(repeat(chained(multithreading(repeat(nowadays_case.just_run,
                                                        1024),
                                                 Environment.ProcessorCount() << 2),
                                  realtime_wrappered(repeat(New nowadays_case(), 100))),
                          round))
    End Sub

    Public Sub New()
        Me.New(100)
    End Sub

    Public Overrides Function reserved_processors() As Int16
        Return CShort(Environment.ProcessorCount())
    End Function
End Class

Public Class nowadays_combined_specified_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(New nowadays_combined_test(max_int64))
    End Sub
End Class

Public Class high_res_normal_res_low_res_ticks_perf_test
    Inherits performance_comparison_case_wrapper

    Private Shared Function R(ByVal f As Action) As [case]
        Return repeat(New delegate_case(f), 1024 * 1024 * 16)
    End Function

    Public Sub New()
        MyBase.New(R(AddressOf nowadays.high_res_ticks),
                   R(AddressOf nowadays.normal_res_ticks),
                   R(AddressOf nowadays.low_res_ticks))
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({2428, 13106, 1146}, i, j)
    End Function
End Class