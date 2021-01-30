
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class onebound(Of K)
    Public NotInheritable Class selector
        Public Shared Function exponential(ByVal i As model, ByVal p As Double) As model
            assert(Not i Is Nothing)
            assert(p >= 0 AndAlso p <= 1)
            Return New model(i.map_each(Function(ByVal x As unordered_map(Of K, Double)) As unordered_map(Of K, Double)
                                            Dim ex As exponential_distribution =
                                                exponential_distribution.estimator.estimate(
                                                    x.stream().map(x.second_selector).to_array())
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

        Private Sub New()
        End Sub
    End Class
End Class
