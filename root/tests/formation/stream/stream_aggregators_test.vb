
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class stream_aggregators_test
    <test>
    Private Shared Sub min_case()
        assertion.equal(streams.of(1, 2, 3).aggregate(stream(Of Int32).aggregators.min), 1)
        assertion.equal(streams.of(1).aggregate(stream(Of Int32).aggregators.min), 1)
        assertion.equal(streams.of(-1, 2, 3).aggregate(stream(Of Int32).aggregators.min), -1)
        assertion.equal(streams.of(-1).aggregate(stream(Of Int32).aggregators.min), -1)
    End Sub

    <test>
    Private Shared Sub max_case()
        assertion.equal(streams.of(1, 2, 3).aggregate(stream(Of Int32).aggregators.max), 3)
        assertion.equal(streams.of(3).aggregate(stream(Of Int32).aggregators.max), 3)
        assertion.equal(streams.of(-1, -2, -3).aggregate(stream(Of Int32).aggregators.max), -1)
        assertion.equal(streams.of(-1).aggregate(stream(Of Int32).aggregators.max), -1)
    End Sub

    <test>
    Private Shared Sub sum_case()
        assertion.equal(streams.of(1, 2, 3).aggregate(stream(Of Int32).aggregators.sum), 6)
        assertion.equal(streams.of(-1).aggregate(stream(Of Int32).aggregators.sum), -1)
    End Sub

    <test>
    Private Shared Sub double_average_case()
        assertion.equal(streams.of(Of Double)(1, 2, 3).aggregate(double_stream.aggregators.average()), 2)
    End Sub

    <test>
    Private Shared Sub bool_all_true_case()
        assertion.is_true(streams.of(True, True, True).aggregate(bool_stream.aggregators.all_true))
        assertion.is_false(streams.of(False, True, True).aggregate(bool_stream.aggregators.all_true))
        assertion.is_false(streams.of(True, False, True).aggregate(bool_stream.aggregators.all_true))
        assertion.is_false(streams.of(True, True, False).aggregate(bool_stream.aggregators.all_true))
    End Sub

    Private Sub New()
    End Sub
End Class
