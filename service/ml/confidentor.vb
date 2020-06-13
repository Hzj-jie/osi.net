
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Public NotInheritable Class confidentor
    Private Const default_ratio As Double = 1000000000

    Public Shared Function confident(ByVal ratio As Double,
                                     ByVal d As distribution,
                                     ByVal ParamArray samples() As Double) As Double
        Return confident(ratio,
                         d,
                         streams.of(samples).
                                 count().
                                 to_array())
    End Function

    Public Shared Function confident(ByVal d As distribution,
                                     ByVal ParamArray samples() As Double) As Double
        Return confident(default_ratio, d, samples)
    End Function

    Public Shared Function confident(ByVal ratio As Double,
                                     ByVal d As distribution,
                                     ByVal ParamArray samples() As tuple(Of Double, UInt32)) As Double
        Dim count As UInt32 = 0
        count = streams.of(samples).
                        map(tuple(Of Double, UInt32).second_selector()).
                        aggregate(stream(Of UInt32).aggregators.sum)
        Dim result As Double = 0
        Using code_block
            Dim v As vector(Of tuple(Of Double, UInt32)) = Nothing
            v = streams.of(samples).
                        sort(tuple(Of Double, UInt32).first_comparer()).
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

    Public Shared Function confident(ByVal d As distribution,
                                     ByVal ParamArray samples() As tuple(Of Double, UInt32)) As Double
        Return confident(default_ratio, d, samples)
    End Function

    Private Sub New()
    End Sub
End Class
