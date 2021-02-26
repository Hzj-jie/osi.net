
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
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

    Private Sub New()
    End Sub
End Class
