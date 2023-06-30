
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class stream_percentile_test
    <test>
    Private Shared Sub percentile_test()
        assertion.equal(streams.range(0, 100).aggregate(stream(Of Int32).aggregators.percentile(0.9)), 90)
        assertion.equal(streams.range(0, 100).aggregate(stream(Of Int32).aggregators.percentile(0)), 0)
        assertion.equal(streams.range(0, 100).aggregate(stream(Of Int32).aggregators.percentile(1)), 99)
    End Sub

    Private Sub New()
    End Sub
End Class
