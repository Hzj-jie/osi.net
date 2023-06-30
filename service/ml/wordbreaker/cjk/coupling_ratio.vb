
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation

Public NotInheritable Class coupling_ratio
    Public Enum mean_type
        quadratic
        arithmetic
        geometric
        harmonic
    End Enum

    Private Shared prefix_ratio As argument(Of Double)
    Private Shared mean As argument(Of mean_type)
    Private Shared use_percentile_ranking As argument(Of Boolean)

    Private Shared Function pre_ratio() As Double
        Dim v As Double = (prefix_ratio Or 1)
        assert(v >= 0)
        Return v
    End Function

    Private Shared Function calculate_mean(Of T)(ByVal all As Func(Of Double, Double),
                                                 ByVal prefixes As Func(Of T, Double, Double)) _
                                                As Func(Of T, T, Double, Double)
        assert(Not all Is Nothing)
        assert(Not prefixes Is Nothing)
        Return Function(ByVal a As T, ByVal b As T, ByVal c As Double) As Double
                   Dim l As Double = all(c)
                   Dim r As Double = prefixes(a, c)
                   If pre_ratio() <> 1 Then
                       l /= pre_ratio()
                       r *= pre_ratio()
                   End If
                   Select Case mean Or mean_type.arithmetic
                       Case mean_type.arithmetic
                           Return ml.mean.arithmetic(l, r)
                       Case mean_type.geometric
                           Return ml.mean.geometric(l, r)
                       Case mean_type.harmonic
                           Return ml.mean.harmonic(l, r)
                       Case mean_type.quadratic
                           Return ml.mean.quadratic(l, r)
                       Case Else
                           assert(False)
                   End Select
                   Return 0
               End Function
    End Function

    Public Shared Function execute(Of T)(ByVal i As onebound(Of T).model) As onebound(Of T).model
        assert(Not i Is Nothing)
        If use_percentile_ranking Or False Then
            Dim all As percentile.ranking_samples(Of Double) =
                percentile.ascent.ranking_samples(
                    i.flat_map().
                      map(first_const_pair(Of const_pair(Of T, T), Double).second_getter).
                      collect_to(Of vector(Of Double))())
            Dim prefixes As unordered_map(Of T, percentile.ranking_samples(Of Double)) =
                i.map_each(Function(ByVal x As unordered_map(Of T, Double)) As percentile.ranking_samples(Of Double)
                               Return percentile.ascent.ranking_samples(x.stream().
                                                                          map(x.second_selector).
                                                                          collect_to(Of vector(Of Double))())
                           End Function)
            Return i.map(calculate_mean(Function(ByVal c As Double) As Double
                                            Return all(c)
                                        End Function,
                                        Function(ByVal a As T, ByVal c As Double) As Double
                                            Return prefixes(a)(c)
                                        End Function))
        Else
            Dim all As exponential_distribution = onebound(Of T).selector.overall_exponential_distribution(i)
            Dim prefixes As unordered_map(Of T, exponential_distribution) =
                onebound(Of T).selector.
                               exponential_distributions(i).
                               stream().
                               map(Function(ByVal x As first_const_pair(Of T, Double)) _
                                       As first_const_pair(Of T, exponential_distribution)
                                       Return first_const_pair.emplace_of(x.first,
                                                                          New exponential_distribution(x.second))
                                   End Function).
                               collect_to(Of unordered_map(Of T, exponential_distribution))()
            Return i.map(calculate_mean(Function(ByVal c As Double) As Double
                                            Return all.cumulative_distribute(c)
                                        End Function,
                                        Function(ByVal a As T, ByVal c As Double) As Double
                                            Return prefixes(a).cumulative_distribute(c)
                                        End Function))
        End If
    End Function

    Private Sub New()
    End Sub
End Class
