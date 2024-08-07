
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.template
Imports osi.root.utt

Public Class stringbuilder_pre_alloc_perf_test
    Inherits stringbuilder_perf_test(Of _true, _false)
End Class

Public Class stringbuilder_auto_extend_perf_test
    Inherits stringbuilder_perf_test(Of _false, _false)
End Class

' Pre-allocating StringBuilder has no significant benefit.
Public Class stringbuilder_perf_test(Of PRE_ALLOC As _boolean, RND_EACH_ROUND As _boolean)
    Inherits performance_comparison_case_wrapper

    Private Const l1 As Int32 = 64
    Private Const l2 As Int32 = 1024 * 256
    Private Const r As Int32 = 128

    Public Sub New()
        MyBase.New(rinne(repeat(New stringbuilder_perf_case(l1, l2), l2), r),
                   rinne(repeat(New stringbuilder_perf_case(l2, l1), l1), r))
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({2589, 2581}, i, j)
    End Function

    Private Class stringbuilder_perf_case
        Inherits [case]

        Private ReadOnly line_len As Int32
        Private ReadOnly line_count As Int32
        Private ReadOnly v As String
        Private s As StringBuilder

        Public Sub New(ByVal line_len As Int32, ByVal line_count As Int32)
            Me.line_len = line_len
            Me.line_count = line_count
            If Not +alloc(Of RND_EACH_ROUND)() Then
                v = rnd_chars(line_len)
            End If
        End Sub

        Public Overrides Function prepare() As Boolean
            If MyBase.prepare() Then
                If +alloc(Of PRE_ALLOC)() Then
                    s = New StringBuilder(line_len * line_count)
                Else
                    s = New StringBuilder()
                End If
                s.Clear()
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Function run() As Boolean
            If v Is Nothing Then
                s.Append(rnd_chars(line_len))
            Else
                s.Append(v)
            End If
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            s.Clear()
            s.shrink_to_fit()
            garbage_collector.repeat_collect()
            Return MyBase.finish()
        End Function
    End Class
End Class
