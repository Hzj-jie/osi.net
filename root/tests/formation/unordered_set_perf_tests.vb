
Option Explicit On
Option Infer Off
Option Strict On

Public Class unordered_set_uint_perf
    Inherits unordered_set_perf(Of UInt32, unordered_set_perf.small_range_uint)

    Public Sub New()
        MyBase.New(0.22, 0.22, 0.25, 0.26, 0.05)
    End Sub

    Protected Overrides Function min_rate_table() As Double(,)
        Return {{0, 0.6},
                {4, 0}}
    End Function
End Class

Public Class unordered_set_uint_large_range_perf
    Inherits unordered_set_perf(Of UInt32, unordered_set_perf.large_range_uint)

    Public Sub New()
        MyBase.New(0.22, 0.22, 0.25, 0.26, 0.05)
    End Sub

    Protected Overrides Function min_rate_table() As Double(,)
        Return {{0, 0.6},
                {4, 0}}
    End Function
End Class

Public Class unordered_set_string_perf
    Inherits unordered_set_perf(Of String, unordered_set_perf.small_range_string)

    Public Sub New()
        MyBase.New(0.22, 0.22, 0.25, 0.26, 0.05)
    End Sub

    Protected Overrides Function min_rate_table() As Double(,)
        Return {{0, 0.6},
                {4, 0}}
    End Function
End Class

Public Class unordered_set_string_large_range_perf
    Inherits unordered_set_perf(Of String, unordered_set_perf.large_range_string)

    Public Sub New()
        MyBase.New(0.22, 0.22, 0.25, 0.26, 0.05)
    End Sub

    Protected Overrides Function min_rate_table() As Double(,)
        Return {{0, 0.6},
                {4, 0}}
    End Function
End Class
