
Imports osi.root.constants
Imports osi.root.connector
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
        If Not assert_more_or_equal_and_less_or_equal(nowadays.low_res_milliseconds() -
                                                      nowadays.normal_res_milliseconds() -
                                                      nowadays.normal_res_milliseconds() +
                                                      nowadays.low_res_milliseconds(),
                                                      -acceptable_difference_ms,
                                                      acceptable_difference_ms,
                                                      "nowadays.low_res_milliseconds()") Then
            low_res_ticks_retriever.force_revise()
        End If
        If Not assert_more_or_equal_and_less_or_equal(nowadays.normal_res_milliseconds() -
                                                      nowadays.high_res_milliseconds() -
                                                      nowadays.high_res_milliseconds() +
                                                      nowadays.normal_res_milliseconds(),
                                                      -acceptable_difference_ms,
                                                      acceptable_difference_ms,
                                                      "nowadays.high_res_milliseconds()") Then
            high_res_ticks_retriever.force_revise()
        End If
        'low_res_ticks is faked as milliseconds_to_ticks(low_res_milliseconds)
        If Not assert_more_or_equal_and_less_or_equal(nowadays.low_res_ticks() -
                                                      nowadays.normal_res_ticks() -
                                                      nowadays.normal_res_ticks() +
                                                      nowadays.low_res_ticks(),
                                                      milliseconds_to_ticks(-acceptable_difference_ms),
                                                      milliseconds_to_ticks(acceptable_difference_ms),
                                                      "nowadays.low_res_ticks()") Then
            low_res_ticks_retriever.force_revise()
        End If
        If Not assert_more_or_equal_and_less_or_equal(nowadays.normal_res_ticks() -
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

Public Class nowadays_specific_test_10
    Inherits commandline_specific_case_wrapper

    Friend Sub New(ByVal sleep_ms As Int64)
        MyBase.New(realtime_wrappered(repeat(sleep_wrappered(New nowadays_case(),
                                                             sleep_ms),
                                             50 * 24 * 60 * 60)))
    End Sub

    Public Sub New()
        Me.New(10)
    End Sub
End Class

Public Class nowadays_specific_test_100
    Inherits nowadays_specific_test_10

    Public Sub New()
        MyBase.New(100)
    End Sub
End Class

Public Class nowadays_specific_test_1000
    Inherits nowadays_specific_test_10

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

    Public Overrides Function preserved_processors() As Int16
        Return Environment.ProcessorCount()
    End Function
End Class

Public Class nowadays_combined_specific_test
    Inherits commandline_specific_case_wrapper

    Public Sub New()
        MyBase.New(New nowadays_combined_test(max_int64))
    End Sub
End Class