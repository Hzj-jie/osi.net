
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports SysMath = System.Math

Public NotInheritable Class mean
    Public Shared Function quadratic(ByVal ParamArray inputs() As Double) As Double
        Return SysMath.Sqrt(streams.of(inputs).
                                    map(Function(ByVal i As Double) As Double
                                            Return i * i
                                        End Function).
                                    aggregate(stream(Of Double).aggregators.sum) /
                            inputs.array_size())
    End Function

    Public Shared Function arithmetic(ByVal ParamArray inputs() As Double) As Double
        Return streams.of(inputs).aggregate(stream(Of Double).aggregators.sum) / inputs.array_size()
    End Function

    Public Shared Function geometric(ByVal ParamArray inputs() As Double) As Double
        Dim r As Double = 1
        For Each i As Double In inputs
            assert(i > 0)
            r *= i
        Next
        Return SysMath.Pow(r, 1 / inputs.array_size())
    End Function

    Public Shared Function harmonic(ByVal ParamArray inputs() As Double) As Double
        Dim product As Double = streams.of(inputs).aggregate(double_stream.aggregators.product)
        Dim denominator As Double = streams.of(inputs).
                                            map(Function(ByVal i As Double) As Double
                                                    assert(i > 0)
                                                    Return product / i
                                                End Function).
                                            aggregate(stream(Of Double).aggregators.sum)
        Return product * inputs.array_size() / denominator
    End Function

    Private Sub New()
    End Sub
End Class
