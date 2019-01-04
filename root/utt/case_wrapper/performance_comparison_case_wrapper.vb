
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Public Class performance_comparison_case_wrapper
    Inherits comparison_case_wrapper

    Private Const multiple_factor As Double = 3
    Private Shared ReadOnly loosening_factor As Double = Math.Sqrt(3)

    Private Shared Function to_perf_cases(ByVal times As UInt64, ByVal cs() As [case]) As [case]()
        assert(Not isemptyarray(cs))
        Dim r() As [case] = Nothing
        ReDim r(array_size_i(cs) - 1)
        For i As Int32 = 0 To array_size_i(cs) - 1
            r(i) = performance(cs(i), times:=times)
        Next
        Return r
    End Function

    Public Sub New(ByVal continue_when_failure As Boolean,
                   ByVal times As UInt64,
                   ByVal ParamArray cs() As [case])
        MyBase.New(continue_when_failure, to_perf_cases(times, cs))
    End Sub

    Public Sub New(ByVal times As UInt64,
                   ByVal ParamArray cs() As [case])
        MyBase.New(to_perf_cases(times, cs))
    End Sub

    Public Sub New(ByVal ParamArray cs() As [case])
        Me.New(1, cs)
    End Sub

    Protected Overridable Function max_rate_table() As Double(,)
        Return Nothing
    End Function

    Protected Overridable Function min_rate_table() As Double(,)
        Return Nothing
    End Function

    Protected Overridable Function average_rate_table() As Double(,)
        Return Nothing
    End Function

    Private Shared Function scan_table(ByVal t(,) As Double, ByVal i As Int32, ByVal j As Int32) As Double
        If t Is Nothing OrElse t.GetLength(0) <= i OrElse t.GetLength(1) <= j Then
            Return -1
        ElseIf t(i, j) = -1 AndAlso t.GetLength(0) > j AndAlso t.GetLength(1) > i AndAlso t(j, i) > 0 Then
            Return multiple_factor / t(j, i)
        Else
            Return t(i, j)
        End If
    End Function

    Private Shared Function scan_table(ByVal t(,) As Double, ByVal i As UInt32, ByVal j As UInt32) As Double
        Return scan_table(t, CInt(i), CInt(j))
    End Function

    Private Function perf_case(ByVal i As UInt32) As performance_case_wrapper
        Return direct_cast(Of performance_case_wrapper)([case](i))
    End Function

    Protected Shared Function loosen_bound(ByVal t() As Double, ByVal i As UInt32, ByVal j As UInt32) As Double
        assert(array_size(t) > i AndAlso array_size(t) > j)
        Return t(CInt(i)) / t(CInt(j)) * loosening_factor
    End Function

    Protected Overridable Function max_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return scan_table(max_rate_table(), i, j)
    End Function

    Protected Overridable Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return scan_table(min_rate_table(), i, j)
    End Function

    Protected Overridable Function average_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return scan_table(average_rate_table(), i, j)
    End Function

    Protected Overrides Sub compare()
        For i As UInt32 = 0 To array_size(cases()) - uint32_1
            For j As UInt32 = 0 To array_size(cases()) - uint32_1
                If i <> j Then
                    Dim v As Double = 0
                    v = max_rate_upper_bound(i, j)
                    If v >= 0 Then
                        expectation.less(perf_case(i).max_used_loops() / perf_case(j).max_used_loops(),
                                         v,
                                         "comparing max_used_loops of case ",
                                         i,
                                         " with case ",
                                         j)
                    End If
                    v = min_rate_upper_bound(i, j)
                    If v >= 0 Then
                        expectation.less(perf_case(i).min_used_loops() / perf_case(j).min_used_loops(),
                                         v,
                                         "comparing min_used_loops of case ",
                                         i,
                                         " with case ",
                                         j)
                    End If
                    v = average_rate_upper_bound(i, j)
                    If v >= 0 Then
                        expectation.less(perf_case(i).average_used_loops() / perf_case(j).average_used_loops(),
                                         v,
                                         "comparing average_used_loops of case ",
                                         i,
                                         " with case ",
                                         j)
                    End If
                End If
            Next
        Next
    End Sub
End Class
