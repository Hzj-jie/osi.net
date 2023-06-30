
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports p = osi.service.ml.percentile

<test>
Public NotInheritable Class ranking_test
    <test>
    Private Shared Sub ascent_case()
        assertion.near_match(p.ascent.ranking(streams.range(0, 100).collect_to(Of vector(Of Int32))(), 79), 0.8, 0.00001)
        assertion.near_match(p.ascent.ranking(streams.range(0, 100).collect_to(Of vector(Of Int32))(), 99), 1, 0.00001)
        assertion.near_match(p.ascent.ranking(streams.range(0, 100).collect_to(Of vector(Of Int32))(), 0), 0.01, 0.00001)
        assertion.near_match(p.ascent.ranking(streams.range(0, 100).collect_to(Of vector(Of Int32))(), 1), 0.02, 0.00001)
    End Sub

    <test>
    Private Shared Sub descent_case()
        assertion.near_match(p.descent.ranking(streams.range(0, 100).collect_to(Of vector(Of Int32))(), 80), 0.2, 0.00001)
        assertion.near_match(p.descent.ranking(streams.range(0, 100).collect_to(Of vector(Of Int32))(), 99), 0.01, 0.00001)
        assertion.near_match(p.descent.ranking(streams.range(0, 100).collect_to(Of vector(Of Int32))(), 0), 1, 0.00001)
        assertion.near_match(p.descent.ranking(streams.range(0, 100).collect_to(Of vector(Of Int32))(), 1), 0.99, 0.00001)
    End Sub

    <test>
    Private Shared Sub large_range_case()
        assertion.near_match(p.ascent.ranking(streams.range(0, 10000).collect_to(Of vector(Of Int32))(),
                                              9000),
                             0.91,
                             0.05)
        assertion.near_match(p.ascent.ranking(streams.range(0, 10000).collect_to(Of vector(Of Int32))(),
                                              5000),
                             0.51,
                             0.05)
        assertion.near_match(p.ascent.ranking(streams.range(0, 10000).collect_to(Of vector(Of Int32))(),
                                              1000),
                             0.11,
                             0.05)
        assertion.near_match(p.ascent.ranking(streams.range(0, 10000).collect_to(Of vector(Of Int32))(),
                                              8000),
                             0.81,
                             0.05)
        assertion.near_match(p.ascent.ranking(streams.range(0, 10000).collect_to(Of vector(Of Int32))(),
                                              4000),
                             0.41,
                             0.05)
        assertion.near_match(p.ascent.ranking(streams.range(0, 10000).collect_to(Of vector(Of Int32))(),
                                              2000),
                             0.21,
                             0.05)
    End Sub

    <test>
    Private Shared Sub large_range_case2()
        assertion.near_match(p.ascent.ranking(streams.range(0, 10000000).collect_to(Of vector(Of Int32))(),
                                              9123456),
                             0.92,
                             0.05)
        assertion.near_match(p.ascent.ranking(streams.range(0, 10000000).collect_to(Of vector(Of Int32))(),
                                              4987654),
                             0.5,
                             0.05)
        assertion.near_match(p.ascent.ranking(streams.range(0, 10000000).collect_to(Of vector(Of Int32))(),
                                              1234567),
                             0.13,
                             0.05)
        assertion.near_match(p.ascent.ranking(streams.range(0, 10000000).collect_to(Of vector(Of Int32))(),
                                              7543210),
                             0.76,
                             0.05)
        assertion.near_match(p.ascent.ranking(streams.range(0, 10000000).collect_to(Of vector(Of Int32))(),
                                              5432109),
                             0.55,
                             0.05)
        assertion.near_match(p.ascent.ranking(streams.range(0, 10000000).collect_to(Of vector(Of Int32))(),
                                              987654),
                             0.1,
                             0.05)
    End Sub

    Private Sub New()
    End Sub
End Class
