
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class onebound(Of K)
    Public NotInheritable Class trainer
        Public NotInheritable Class bind
            Public independence As Double
            Public ReadOnly followers As unordered_map(Of K, Double)

            Public Sub New()
                independence = 0
                followers = New unordered_map(Of K, Double)()
            End Sub

            Public Function sum() As Double
                Return independence +
                       followers.
                           stream().
                           map(followers.second_selector).
                           aggregate(stream(Of Double).aggregators.sum)
            End Function

            Public Shared Function sum(ByVal b As bind) As Double
                assert(Not b Is Nothing)
                Return b.sum()
            End Function

            Public Function average() As Double
                Return sum() / (followers.size() + uint32_1)
            End Function

            Public Shared Function average(ByVal b As bind) As Double
                assert(Not b Is Nothing)
                Return b.average()
            End Function
        End Class

        Private ReadOnly m As unordered_map(Of K, bind)

        Public Sub New()
            m = New unordered_map(Of K, bind)()
        End Sub

        Public Function accumulate(ByVal a As K, ByVal b As K, ByVal v As Double) As trainer
            assert(v > 0)
            m(a).followers(b) += v
            Return Me
        End Function

        Public Function accumulate(ByVal a As K, ByVal b As K) As trainer
            Return accumulate(a, b, 1)
        End Function

        Public Function accumulate(ByVal a As K, ByVal v As Double) As trainer
            assert(v > 0)
            m(a).independence += v
            Return Me
        End Function

        Public Function accumulate(ByVal a As K) As trainer
            Return accumulate(a, 1)
        End Function

        Private Shared Function average(ByVal v As Double, ByVal a1 As Double, ByVal a2 As Double) As Double
            Return Math.Sqrt(v * v / a1 / a2)
        End Function

        Private Shared Function exclusive_average(ByVal ave As Double,
                                                  ByVal size As UInt32,
                                                  ByVal v As Double) As Double
            If size = uint32_1 Then
                assert(ave = v)
                Return ave
            End If
            Return (ave * size - v) / (size - uint32_1)
        End Function

        Private Shared Function normalize(ByVal global_ave As Double, ByVal b As bind) As model.bind
            assert(Not b Is Nothing)
            Dim ave As Double = 0
            ave = b.average()
            Return New model.bind(average(b.independence,
                                          global_ave,
                                          exclusive_average(ave, b.followers.size() + uint32_1, b.independence)),
                                  b.followers.
                                    stream().
                                    map(b.followers.second_mapper(
                                            Function(ByVal i As Double) As Double
                                                Return average(i,
                                                               global_ave,
                                                               exclusive_average(ave, b.followers.size() + uint32_1, i))
                                            End Function)).
                                    collect(Of unordered_map(Of K, Double))())
        End Function

        Private Function normalize() As unordered_map(Of K, model.bind)
            Dim ave As Double = 0
            ave = m.stream().
                    map(m.second_selector).
                    map(AddressOf bind.average).
                    aggregate(stream(Of Double).aggregators.average)
            Return m.stream().
                     map(m.second_mapper(Function(ByVal b As bind) As model.bind
                                             Return normalize(ave, b)
                                         End Function)).
                     collect(Of unordered_map(Of K, model.bind))()
        End Function

        Public Function dump() As model
            Return New model(normalize())
        End Function
    End Class
End Class
