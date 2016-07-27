﻿
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.utt

Friend Class accumulate_segment_tree_case
    Inherits segment_tree_case2

    Public Sub New(ByVal verify As Boolean)
        MyBase.New(verify, False)
    End Sub

    Private Function single_find_case(ByVal t As accumulate_segment_tree(Of Int64),
                                      ByVal s As vector(Of pair(Of Int64, pair(Of Int64, Int64)))) As Boolean
        assert(Not t Is Nothing)
        For i As Int32 = 0 To 4096 * If(isreleasebuild(), 10, 1) - 1
            Dim p As Int64 = 0
            p = rnd_position()
            Dim has_value As Boolean = False
            Dim value As Int64 = 0
            stupid_find(s, p, has_value, value)
            Dim r As Int64 = 0
            assert_equal(t.accumulate(p, r), has_value)
            assert_equal(r, value)
        Next
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Dim min As Int64 = 0
        Dim max As Int64 = 0
        Dim s As vector(Of pair(Of Int64, pair(Of Int64, Int64))) = Nothing
        s = prepare_segments(min, max)
        Dim t As accumulate_segment_tree(Of Int64) = Nothing
        t = New accumulate_segment_tree(Of Int64)(min, max)
        For i As Int32 = 0 To s.size() - 1
            assert_true(t.emplace(s(i).second.first, s(i).second.second, s(i).first))
        Next
        Return Not verify OrElse
               single_find_case(t, s)
    End Function
End Class

Public Class accumulate_segment_tree_test
    Inherits repeat_case_wrapper

    Public Sub New()
        MyBase.New(New accumulate_segment_tree_case(True), 32 * If(isreleasebuild(), 8, 1))
    End Sub
End Class

Public Class accumulate_segment_tree_perf
    Inherits performance_case_wrapper

    Public Sub New()
        MyBase.New(repeat(New accumulate_segment_tree_case(False), 32 * If(isreleasebuild(), 8, 1)))
    End Sub
End Class
