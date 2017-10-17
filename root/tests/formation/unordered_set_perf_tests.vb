
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.envs

Public Class unordered_set_uint_perf
    Inherits unordered_set_perf(Of UInt32, unordered_set_perf_templates.small_range_uint)

    Public Sub New()
        MyBase.New(low_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        If isdebugbuild() Then
            Return loosen_bound({508, 5478}, i, j)
        Else
            Return loosen_bound({373, 1635}, i, j)
        End If
    End Function
End Class

Public Class unordered_set_uint_large_range_perf
    Inherits unordered_set_perf(Of UInt32, unordered_set_perf_templates.large_range_uint)

    Public Sub New()
        MyBase.New(low_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        If isdebugbuild() Then
            Return loosen_bound({508, 5579}, i, j)
        Else
            Return loosen_bound({281, 1587}, i, j)
        End If
    End Function
End Class

Public Class unordered_set_string_perf
    Inherits unordered_set_perf(Of String, unordered_set_perf_templates.small_range_string)

    Public Sub New()
        MyBase.New(low_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        If isdebugbuild() Then
            Return loosen_bound({611, 2097}, i, j)
        Else
            Return loosen_bound({514, 1495}, i, j)
        End If
    End Function
End Class

Public Class unordered_set_string_large_range_perf
    Inherits unordered_set_perf(Of String, unordered_set_perf_templates.large_range_string)

    Public Sub New()
        MyBase.New(low_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        If isdebugbuild() Then
            Return loosen_bound({732, 2198}, i, j)
        Else
            Return loosen_bound({514, 1542}, i, j)
        End If
    End Function
End Class

Public Class unordered_set_more_items_uint_perf
    Inherits unordered_set_perf(Of UInt32, unordered_set_perf_templates.small_range_uint)

    Public Sub New()
        MyBase.New(high_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        If isdebugbuild() Then
            Return loosen_bound({814, 8451}, i, j)
        Else
            Return loosen_bound({514, 3178}, i, j)
        End If
    End Function
End Class

Public Class unordered_set_more_items_uint_large_range_perf
    Inherits unordered_set_perf(Of UInt32, unordered_set_perf_templates.large_range_uint)

    Public Sub New()
        MyBase.New(high_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        If isdebugbuild() Then
            Return loosen_bound({1458, 9368}, i, j)
        Else
            If virtual_machine Then
                Return loosen_bound({882, 3176}, i, j)
            Else
                Return loosen_bound({792, 3737}, i, j)
            End If
        End If
    End Function
End Class

Public Class unordered_set_more_items_string_perf
    Inherits unordered_set_perf(Of String, unordered_set_perf_templates.small_range_string)

    Public Sub New()
        MyBase.New(high_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        If isdebugbuild() Then
            Return loosen_bound({1548, 3115}, i, j)
        Else
            Return loosen_bound({1587, 2709}, i, j)
        End If
    End Function
End Class

Public Class unordered_set_more_items_string_large_range_perf
    Inherits unordered_set_perf(Of String, unordered_set_perf_templates.large_range_string)

    Public Sub New()
        MyBase.New(high_item_count_percentages())
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        If isdebugbuild() Then
            Return loosen_bound({2118, 3563}, i, j)
        Else
            Return loosen_bound({1306, 2945}, i, j)
        End If
    End Function
End Class
