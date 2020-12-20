
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports nd = osi.service.ml.normal_distribution

Namespace normal_distribution
    <test>
    Public NotInheritable Class estimator_estimate_test
        <test>
        Private Shared Sub case1()
            Dim n As nd = Nothing
            n = New nd(0, 1)
            Dim samples() As tuple(Of Double, UInt32) = Nothing
            samples = streams.range_closed(-50, 50).
                              map(Function(ByVal i As Int32) As tuple(Of Double, UInt32)
                                      Dim v As Double = 0
                                      v = i / 10
                                      Return tuple.of(v, CUInt(n.range_possibility(v - 1 / 20, v + 1 / 20) * 1000))
                                  End Function).
                              to_array()
            Dim n2 As nd = Nothing
            n2 = nd.estimator.estimate(samples)
            assertion.is_true(n.near_match(n2, 0.03), n, n2)
        End Sub

        <test>
        Private Shared Sub case2()
            Dim n As nd = Nothing
            n = New nd(0, 1)
            Dim samples() As tuple(Of Double, UInt32) = Nothing
            samples = streams.range_closed(-500, 500).
                              map(Function(ByVal i As Int32) As tuple(Of Double, UInt32)
                                      Dim v As Double = 0
                                      v = i / 100
                                      Return tuple.of(v, CUInt(n.range_possibility(v - 1 / 200, v + 1 / 200) * 1000000))
                                  End Function).
                              to_array()
            Dim n2 As nd = Nothing
            n2 = nd.estimator.estimate(samples)
            assertion.is_true(n.near_match(n2, 0.0003), n, n2)
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
