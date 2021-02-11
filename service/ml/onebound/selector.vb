
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class onebound(Of K)
    Public NotInheritable Class selector
        Private Shared Function exponential_distribution(ByVal i As model,
                                                         ByVal x As unordered_map(Of K, Double)) _
                                                        As exponential_distribution
            assert(Not i Is Nothing)
            assert(Not x Is Nothing)
            Dim m As unordered_map(Of Double, UInt32) =
                                                x.stream().
                                                  map(x.second_selector).
                                                  collect_by(stream(Of Double).collectors.frequency())
            Return exponential_distribution.estimator.estimate(m)
        End Function

        Public Shared Function exponential(ByVal i As model, ByVal p As Double) As model
            assert(Not i Is Nothing)
            assert(p >= 0 AndAlso p <= 1)
            Return New model(i.map_each(Function(ByVal x As unordered_map(Of K, Double)) As unordered_map(Of K, Double)
                                            Dim ex As exponential_distribution = exponential_distribution(i, x)
                                            Return x.stream().
                                                     map(x.second_mapper(Function(ByVal y As Double) As Double
                                                                             Return ex.cumulative_distribute(y)
                                                                         End Function)).
                                                     filter(x.second_filter(Function(ByVal y As Double) As Boolean
                                                                                Return y >= p
                                                                            End Function)).
                                                     collect(Of unordered_map(Of K, Double))()
                                        End Function))
        End Function

        Public Shared Function exponential_distributions(ByVal i As model) As unordered_map(Of K, Double)
            assert(Not i Is Nothing)
            Return i.map_each(Function(ByVal x As unordered_map(Of K, Double)) As Double
                                  Return exponential_distribution(i, x).lambda
                              End Function)
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
