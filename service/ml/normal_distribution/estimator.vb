
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class normal_distribution
    Public NotInheritable Class estimator
        Public Shared Function estimate(ByVal ParamArray samples() As Double) As normal_distribution
            assert(samples.array_size() > 1)
            Dim mean As Double = 0
            mean = streams.of(samples).aggregate(double_stream.aggregators.average())
            Dim variance As Double = 0
            variance = streams.of(samples).
                               map(Function(ByVal i As Double) As Double
                                       Return (i - mean) ^ 2
                                   End Function).
                               aggregate(stream(Of Double).aggregators.sum) /
                       (samples.array_size() - uint32_1)
            Return New normal_distribution(mean, variance)
        End Function

        Public Shared Function estimate(ByVal ParamArray samples() As tuple(Of Double, UInt32)) As normal_distribution
            Return estimate(streams.of(samples).
                                    flat_map(Function(ByVal s As tuple(Of Double, UInt32)) As stream(Of Double)
                                                 Return streams.repeat(s.first(), s.second())
                                             End Function).
                                    to_array())
        End Function

        Public Shared Function confident(ByVal d As normal_distribution, ByVal ParamArray samples() As Double) As Double
            Return confident(d,
                             streams.of(samples).
                                     collect_by(stream(Of Double).collectors.frequency()).
                                     stream().
                                     map(AddressOf tuple(Of Double, UInt32).from_first_const_pair).
                                     to_array())
        End Function

        Public Shared Function confident(ByVal d As normal_distribution,
                                         ByVal ParamArray samples() As tuple(Of Double, UInt32)) As Double
            Const ratio As Double = 1000000000
            Dim count As UInt32 = 0
            count = streams.of(samples).
                            map(Function(ByVal i As tuple(Of Double, UInt32)) As UInt32
                                    Return i.second
                                End Function).
                            aggregate(stream(Of UInt32).aggregators.sum)
            Dim result As Double = 0
            Using code_block
                Dim v As vector(Of tuple(Of Double, UInt32)) = Nothing
                v = streams.of(samples).
                           sort(Function(ByVal l As tuple(Of Double, UInt32),
                                         ByVal r As tuple(Of Double, UInt32)) As Int32
                                    Return l.first().CompareTo(r.first())
                                End Function).
                           collect(Of vector(Of tuple(Of Double, UInt32)))()
                Dim i As UInt32 = 0
                While i < v.size()
                    Dim p As Double = 0
                    If i = 0 Then
                        p = d.cumulative_distribute(v(i).first())
                    ElseIf i = v.size() - uint32_1 Then
                        p = 1 - d.cumulative_distribute(v(i).first())
                    Else
                        p = d.range_possibility((v(i - uint32_1).first() + v(i).first()) / 2,
                                            (v(i).first() + v(i + uint32_1).first()) / 2)
                    End If
                    result += (p * ratio - v(i).second() * ratio / count) ^ 2
                    i += uint32_1
                End While
            End Using
            Return result / samples.array_size()
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
