
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class normal_distribution
    Public NotInheritable Class estimator
        Public Shared Function estimate(ByVal ParamArray samples() As Double) As normal_distribution
            Return estimate(streams.of(samples).count().to_array())
        End Function

        Public Shared Function estimate(ByVal ParamArray samples() As tuple(Of Double, UInt32)) As normal_distribution
            assert(samples.array_size() > 1)
            Dim count As UInt32 = 0
            count = streams.of(samples).
                            map(tuple(Of Double, UInt32).second_selector).
                            aggregate(stream(Of UInt32).aggregators.sum)
            Dim mean As Double = 0
            mean = streams.of(samples).
                           map(Function(ByVal i As tuple(Of Double, UInt32)) As Double
                                   Return i.first() * i.second()
                               End Function).
                           aggregate(stream(Of Double).aggregators.sum) /
                   count
            Dim variance As Double = 0
            variance = streams.of(samples).
                               map(Function(ByVal i As tuple(Of Double, UInt32)) As Double
                                       Return ((i.first() - mean) ^ 2) * i.second()
                                   End Function).
                               aggregate(stream(Of Double).aggregators.sum) /
                       (count - uint32_1)
            Return New normal_distribution(mean, variance)
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
