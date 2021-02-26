
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class exponential_distribution
    Public NotInheritable Class estimator
        Public Shared Function estimate(ByVal samples As unordered_map(Of Double, UInt32)) As exponential_distribution
            assert(Not samples.empty())
            If samples.size() = 1 Then
                ' Return a large lambda to ensure everything returns 1 for cumulative possibility.
                Return New exponential_distribution(100 / samples.begin().value().second)
            End If
            If samples.size() = 2 Then
                Dim min As first_const_pair(Of Double, UInt32) =
                    samples.stream().aggregate(stream(Of first_const_pair(Of Double, UInt32)).aggregators.min)
                Dim max As first_const_pair(Of Double, UInt32) =
                    samples.stream().aggregate(stream(Of first_const_pair(Of Double, UInt32)).aggregators.max)
                Return New exponential_distribution(min.first * min.second / max.first / max.second)
            End If
            Dim count As UInt32 = samples.stream().
                                          map(samples.second_selector).
                                          aggregate(stream(Of UInt32).aggregators.sum)
            Dim lambda As Double = count /
                                   samples.stream().
                                           map(samples.mapper(Function(ByVal i As Double, ByVal j As UInt32) As Double
                                                                  Return i * j
                                                              End Function)).
                                           aggregate(stream(Of Double).aggregators.sum)
            Return New exponential_distribution(lambda)
        End Function

        Public Shared Function estimate(ByVal ParamArray samples() As tuple(Of Double, UInt32)) _
                                       As exponential_distribution
            Dim r As New unordered_map(Of Double, UInt32)()
            For Each sample As tuple(Of Double, UInt32) In samples
                r(sample.first()) += sample.second()
            Next
            Return estimate(r)
        End Function

        Public Shared Function estimate(ByVal ParamArray samples() As Double) As exponential_distribution
            Return estimate(streams.of(samples).collect_by(stream(Of Double).collectors.frequency()))
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
