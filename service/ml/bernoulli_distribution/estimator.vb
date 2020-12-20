
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class bernoulli_distribution
    Public NotInheritable Class estimator
        Public Shared Function estimate(ByVal ParamArray samples() As Boolean) As bernoulli_distribution
            Return New bernoulli_distribution(+streams.of(samples).
                                                       filter(Function(ByVal x As Boolean) As Boolean
                                                                  Return x
                                                              End Function).
                                                       collect_by(stream(Of Boolean).collectors.count()) /
                                              array_size(samples))
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
