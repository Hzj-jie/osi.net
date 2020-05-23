
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class unordered_set_uint_perf
    Inherits unordered_set_perf(Of UInt32, unordered_set_perf_templates.small_range_uint)

    Public Sub New()
        MyBase.New(low_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({322, 298, 1413}, i, j)
    End Function
End Class

Public NotInheritable Class unordered_set_uint_large_range_perf
    Inherits unordered_set_perf(Of UInt32, unordered_set_perf_templates.large_range_uint)

    Public Sub New()
        MyBase.New(low_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({247, 249, 1515}, i, j)
    End Function
End Class

Public NotInheritable Class unordered_set_string_perf
    Inherits unordered_set_perf(Of String, unordered_set_perf_templates.small_range_string)

    Public Sub New()
        MyBase.New(low_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({745, 471, 1488}, i, j)
    End Function
End Class

Public NotInheritable Class unordered_set_string_large_range_perf
    Inherits unordered_set_perf(Of String, unordered_set_perf_templates.large_range_string)

    Public Sub New()
        MyBase.New(low_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({595, 595, 1165}, i, j)
    End Function
End Class

Public NotInheritable Class unordered_set_more_items_uint_perf
    Inherits unordered_set_perf(Of UInt32, unordered_set_perf_templates.small_range_uint)

    Public Sub New()
        MyBase.New(high_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({770, 544, 2730}, i, j)
    End Function
End Class

Public NotInheritable Class unordered_set_more_items_uint_large_range_perf
    Inherits unordered_set_perf(Of UInt32, unordered_set_perf_templates.large_range_uint)

    Public Sub New()
        MyBase.New(high_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({1042, 843, 3302}, i, j)
    End Function
End Class

Public NotInheritable Class unordered_set_more_items_string_perf
    Inherits unordered_set_perf(Of String, unordered_set_perf_templates.small_range_string)

    Public Sub New()
        MyBase.New(high_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({1315, 1389, 2532}, i, j)
    End Function
End Class

Public NotInheritable Class unordered_set_more_items_string_large_range_perf
    Inherits unordered_set_perf(Of String, unordered_set_perf_templates.large_range_string)

    Public Sub New()
        MyBase.New(high_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({1637, 1490, 2457}, i, j)
    End Function
End Class
