
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports ed = osi.service.ml.exponential_distribution

Namespace exponential_distribution
    <test>
    Public NotInheritable Class estimator_estimate_test
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
            Dim n2 As ed = Nothing
            n2 = ed.estimator.estimate(samples)
            assertion.is_true(n.near_match(n2, 0.023), n, n2)
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
            Dim n2 As ed = Nothing
            n2 = ed.estimator.estimate(samples)
            assertion.is_true(n.near_match(n2, 0.00036), n, n2)
        End Sub

        <test>
        Private Shared Sub case3()
            Dim samples() As Double = {
                1, 1, 1, 2, 2, 8, 18, 37, 45, 59, 61, 75, 131, 160, 311, 1555, 10349, 11063, 13718, 14786, 31467, 37532,
                53834, 85574, 118978, 1077332
            }
            Dim n As ed = ed.estimator.estimate(samples)
            assertion.is_true(n.near_match(0.000017844, 0.000000001), n)
        End Sub

        <test>
        Private Shared Sub case4()
            Dim samples() As Double = {
                10.9998078181983,
                1386.97576762193
            }
            Dim n As ed = ed.estimator.estimate(samples)
            assertion.is_true(n.near_match(0.00793078586878145, 0.000000001), n)
            assertions.of(n.cumulative_distribute(samples.first())).in_range(0.0835402414414449)
            assertions.of(n.cumulative_distribute(samples.last())).in_range(0.999983295089138)
        End Sub

        <test>
        Private Shared Sub case5()
            Dim samples() As Double = {
                10.9998078181983,
                10.9998078181983,
                10.9998078181983,
                10.9998078181983,
                10.9998078181983,
                1386.97576762193
            }
            Dim n As ed = ed.estimator.estimate(samples)
            assertion.is_true(n.near_match(0.0396539293439072, 0.000000001), n)
            assertions.of(n.cumulative_distribute(samples.first())).in_range(0.35350227648358)
            assertions.of(n.cumulative_distribute(samples.last())).in_range(1)
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
