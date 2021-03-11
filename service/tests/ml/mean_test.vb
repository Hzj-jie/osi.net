
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.ml

<test>
Public NotInheritable Class mean_test
    <test>
    Private Shared Sub quadratic_case()
        assertion.near_match(mean.quadratic(1, 2, 3), 2.160247, 0.000001)
    End Sub

    <test>
    Private Shared Sub arithmetic_case()
        assertion.near_match(mean.arithmetic(1, 2, 3), 2, 0.000001)
    End Sub

    <test>
    Private Shared Sub geometric_case()
        assertion.near_match(mean.geometric(1, 2, 3), 1.817121, 0.000001)
    End Sub

    <test>
    Private Shared Sub geometric_accepts_only_positive()
        assertion.death(Sub()
                            mean.geometric(1, 2, 3, -1, 100)
                        End Sub)
        assertion.death(Sub()
                            mean.geometric(1, 2, 3, 0, 1)
                        End Sub)
    End Sub

    <test>
    Private Shared Sub harmonic_case()
        assertion.near_match(mean.harmonic(1, 2, 3), 1.636363, 0.000001)
    End Sub

    <test>
    Private Shared Sub harmonic_accepts_only_positive()
        assertion.death(Sub()
                            mean.harmonic(1, 2, 3, -1, 100)
                        End Sub)
        assertion.death(Sub()
                            mean.harmonic(1, 2, 3, 0, 1)
                        End Sub)
    End Sub

    <test>
    <repeat(100)>
    Private Shared Sub random_case()
        Dim v As const_array(Of Double) = const_array.alloc_of(Function() As Double
                                                                   Return rnd_double(-100, 100)
                                                               End Function,
                                                               rnd_uint(1, 100))
        assertion.less_or_equal(Math.Abs(mean.arithmetic(v)), mean.quadratic(v))
    End Sub

    <test>
    <repeat(100)>
    Private Shared Sub random_positive_case()
        Dim v As const_array(Of Double) = const_array.alloc_of(Function() As Double
                                                                   Return rnd_double(0.0001, 100)
                                                               End Function,
                                                               rnd_uint(1, 100))
        assertion.less_or_equal(mean.harmonic(v), mean.geometric(v))
        assertion.less_or_equal(mean.geometric(v), mean.arithmetic(v))
        assertion.less_or_equal(mean.arithmetic(v), mean.quadratic(v))
    End Sub

    <test>
    <repeat(100)>
    Private Shared Sub random_positive_case_equal()
        Dim v As const_array(Of Double) = const_array.repeat_of(rnd_double(0.00001, 100), rnd_uint(1, 100))
        assertion.near_match(mean.harmonic(v), mean.geometric(v), 0.0000001)
        assertion.near_match(mean.geometric(v), mean.arithmetic(v), 0.0000001)
        assertion.near_match(mean.arithmetic(v), mean.quadratic(v), 0.0000001)
    End Sub

    Private Sub New()
    End Sub
End Class
