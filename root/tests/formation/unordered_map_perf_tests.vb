
Option Explicit On
Option Infer Off
Option Strict On

Public Class unordered_map_uint_perf
    Inherits unordered_map_perf(Of UInt32, UInt32)

    Public Sub New()
        MyBase.New(low_item_count_percentages(), small_range_uint(), small_range_uint())
    End Sub
End Class

Public Class unordered_map_uint_more_items_perf
    Inherits unordered_map_perf(Of UInt32, UInt32)

    Public Sub New()
        MyBase.New(high_item_count_percentages(), small_range_uint(), small_range_uint())
    End Sub
End Class

Public Class unordered_map_uint_large_range_perf
    Inherits unordered_map_perf(Of UInt32, UInt32)

    Public Sub New()
        MyBase.New(low_item_count_percentages(), large_range_uint(), small_range_uint())
    End Sub
End Class

Public Class unordered_map_uint_large_range_more_items_perf
    Inherits unordered_map_perf(Of UInt32, UInt32)

    Public Sub New()
        MyBase.New(high_item_count_percentages(), large_range_uint(), small_range_uint())
    End Sub
End Class

Public Class unordered_map_string_perf
    Inherits unordered_map_perf(Of String, String)

    Public Sub New()
        MyBase.New(low_item_count_percentages(), small_range_string(), small_range_string())
    End Sub
End Class

Public Class unordered_map_string_more_items_perf
    Inherits unordered_map_perf(Of String, String)

    Public Sub New()
        MyBase.New(high_item_count_percentages(), small_range_string(), small_range_string())
    End Sub
End Class

Public Class unordered_map_string_large_range_perf
    Inherits unordered_map_perf(Of String, String)

    Public Sub New()
        MyBase.New(low_item_count_percentages(), large_range_string(), small_range_string())
    End Sub
End Class

Public Class unordered_map_string_large_range_more_items_perf
    Inherits unordered_map_perf(Of String, String)

    Public Sub New()
        MyBase.New(high_item_count_percentages(), large_range_string(), small_range_string())
    End Sub
End Class