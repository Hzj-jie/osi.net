
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
            Return loosen_bound({508, 508, 5478}, i, j)
        Else
            Return loosen_bound({373, 373, 1635}, i, j)
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
            Return loosen_bound({508, 508, 5579}, i, j)
        Else
            Return loosen_bound({506, 506, 1370}, i, j)
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
            Return loosen_bound({611, 611, 2097}, i, j)
        Else
            Return loosen_bound({514, 514, 1495}, i, j)
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
            Return loosen_bound({732, 732, 2198}, i, j)
        Else
            Return loosen_bound({514, 514, 1542}, i, j)
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
            Return loosen_bound({814, 814, 8451}, i, j)
        Else
            Return loosen_bound({104, 118, 281}, i, j)
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
            Return loosen_bound({1458, 1458, 9368}, i, j)
        Else
            If virtual_machine Then
                Return loosen_bound({882, 882, 3176}, i, j)
            Else
                Return loosen_bound({1230, 2145, 3614}, i, j)
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
            Return loosen_bound({1548, 1548, 3115}, i, j)
        Else
            Return loosen_bound({2527, 1695, 2599}, i, j)
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
            Return loosen_bound({2118, 2118, 3563}, i, j)
        Else
            Return loosen_bound({1985, 2940, 3177}, i, j)
        End If
    End Function
End Class
