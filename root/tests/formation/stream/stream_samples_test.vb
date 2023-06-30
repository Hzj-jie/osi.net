
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class stream_samples_test
    <test>
    Private Shared Sub return_empty_vector()
        assertion.equal(streams.of(1, 2, 3).collect_by(stream(Of Int32).collectors.samples(0)),
                        vector.of(Of Int32)())
    End Sub

    <test>
    Private Shared Sub return_empty_vector_if_input_is_empty()
        assertion.equal(streams.of(Of Int32)().collect_by(stream(Of Int32).collectors.samples(100)),
                        vector.of(Of Int32)())
    End Sub

    <test>
    Private Shared Sub return_as_many_as_possible()
        ' Since all the elements in the original stream are selected, the order should be kept unchanged.
        assertion.equal(streams.of(1, 2, 3).collect_by(stream(Of Int32).collectors.samples(4)),
                        vector.of(1, 2, 3))
    End Sub

    <test>
    Private Shared Sub return_sampled_values()
        Dim v As vector(Of Int32) = streams.range(0, 100).collect_by(stream(Of Int32).collectors.samples(4))
        assertion.equal(v.size(), CUInt(4))
        assertion.equal(v.stream().collect_by(stream(Of Int32).collectors.unique()).size(), CUInt(4))
    End Sub

    Private Sub New()
    End Sub
End Class
