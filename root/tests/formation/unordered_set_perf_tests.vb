
Option Explicit On
Option Infer Off
Option Strict On

Public Class unordered_set_uint_perf
    Inherits unordered_set_perf(Of UInt32, unordered_set_perf_templates.small_range_uint)

    Public Sub New()
        MyBase.New(low_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_table() As Double(,)
        Return {{0, 0.35},
                {7, 0}}
    End Function
End Class

Public Class unordered_set_uint_large_range_perf
    Inherits unordered_set_perf(Of UInt32, unordered_set_perf_templates.large_range_uint)

    Public Sub New()
        MyBase.New(low_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_table() As Double(,)
        Return {{0, 0.3},
                {8, 0}}
    End Function
End Class

Public Class unordered_set_string_perf
    Inherits unordered_set_perf(Of String, unordered_set_perf_templates.small_range_string)

    Public Sub New()
        MyBase.New(low_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_table() As Double(,)
        Return {{0, 0.3},
                {8, 0}}
    End Function
End Class

Public Class unordered_set_string_large_range_perf
    Inherits unordered_set_perf(Of String, unordered_set_perf_templates.large_range_string)

    Public Sub New()
        MyBase.New(low_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_table() As Double(,)
        Return {{0, 0.4},
                {6, 0}}
    End Function
End Class

Public Class unordered_set_more_items_uint_perf
    Inherits unordered_set_perf(Of UInt32, unordered_set_perf_templates.small_range_uint)

    Public Sub New()
        MyBase.New(high_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_table() As Double(,)
        Return {{0, 0.4},
                {6, 0}}
    End Function
End Class

Public Class unordered_set_more_items_uint_large_range_perf
    Inherits unordered_set_perf(Of UInt32, unordered_set_perf_templates.large_range_uint)

    Public Sub New()
        MyBase.New(high_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_table() As Double(,)
        Return {{0, 0.4},
                {6, 0}}
    End Function
End Class

Public Class unordered_set_more_items_string_perf
    Inherits unordered_set_perf(Of String, unordered_set_perf_templates.small_range_string)

    Public Sub New()
        MyBase.New(high_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_table() As Double(,)
        Return {{0, 0.4},
                {6, 0}}
    End Function
End Class

Public Class unordered_set_more_items_string_large_range_perf
    Inherits unordered_set_perf(Of String, unordered_set_perf_templates.large_range_string)

    Public Sub New()
        MyBase.New(high_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_table() As Double(,)
        Return {{0, 0.4},
                {6, 0}}
    End Function
End Class
