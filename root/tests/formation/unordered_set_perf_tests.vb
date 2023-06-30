
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class unordered_set_uint_perf
    Inherits unordered_set_perf(Of UInt32, unordered_set_perf_templates.small_range_uint)

    Public Sub New()
        MyBase.New(low_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({2431, 16575}, i, j)
    End Function
End Class

Public NotInheritable Class unordered_set_uint_large_range_perf
    Inherits unordered_set_perf(Of UInt32, unordered_set_perf_templates.large_range_uint)

    Public Sub New()
        MyBase.New(low_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({1468, 12350}, i, j)
    End Function
End Class

Public NotInheritable Class unordered_set_string_perf
    Inherits unordered_set_perf(Of String, unordered_set_perf_templates.small_range_string)

    Public Sub New()
        MyBase.New(low_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({7249, 22101}, i, j)
    End Function
End Class

Public NotInheritable Class unordered_set_string_large_range_perf
    Inherits unordered_set_perf(Of String, unordered_set_perf_templates.large_range_string)

    Public Sub New()
        MyBase.New(low_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({8641, 17946}, i, j)
    End Function
End Class

Public NotInheritable Class unordered_set_more_items_uint_perf
    Inherits unordered_set_perf(Of UInt32, unordered_set_perf_templates.small_range_uint)

    Public Sub New()
        MyBase.New(high_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({4840, 33151}, i, j)
    End Function
End Class

Public NotInheritable Class unordered_set_more_items_uint_large_range_perf
    Inherits unordered_set_perf(Of UInt32, unordered_set_perf_templates.large_range_uint)

    Public Sub New()
        MyBase.New(high_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({5525, 41439}, i, j)
    End Function
End Class

Public NotInheritable Class unordered_set_more_items_string_perf
    Inherits unordered_set_perf(Of String, unordered_set_perf_templates.small_range_string)

    Public Sub New()
        MyBase.New(high_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({1495, 2804}, i, j)
    End Function
End Class

Public NotInheritable Class unordered_set_more_items_string_large_range_perf
    Inherits unordered_set_perf(Of String, unordered_set_perf_templates.large_range_string)

    Public Sub New()
        MyBase.New(high_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({1390, 2620}, i, j)
    End Function
End Class
