
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.ml
Imports ed = osi.service.ml.exponential_distribution

Namespace exponential_distribution
    <test>
    Public NotInheritable Class estimator_confident_test
        <test>
        Private Shared Sub case1()
            Dim n As ed = Nothing
            n = New ed(1)
            Dim samples() As tuple(Of Double, UInt32) = Nothing
            samples = streams.range_closed(0, 100).
                              map(Function(ByVal i As Int32) As tuple(Of Double, UInt32)
                                      Dim v As Double = 0
                                      v = i / 10
                                      Return tuple.of(v, CUInt(n.range_possibility(v - 1 / 20, v + 1 / 20) * 1000))
                                  End Function).
                              to_array()
            assertions.of(confidence.of(n, samples)).in_range(23927697573779, 1)
        End Sub

        <test>
        Private Shared Sub case2()
            Dim n As ed = Nothing
            n = New ed(1)
            Dim samples() As tuple(Of Double, UInt32) = Nothing
            samples = streams.range_closed(0, 1000).
                              map(Function(ByVal i As Int32) As tuple(Of Double, UInt32)
                                      Dim v As Double = 0
                                      v = i / 100
                                      Return tuple.of(v, CUInt(n.range_possibility(v - 1 / 200, v + 1 / 200) * 1000000))
                                  End Function).
                              to_array()
            assertions.of(confidence.of(n, samples)).in_range(24859031708, 1)
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
