
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class onebound(Of K)
    Public NotInheritable Class trainer
        Public NotInheritable Class bind
            Public ReadOnly successors As unordered_map(Of K, Double)
            Public ReadOnly predecessors As unordered_map(Of K, Double)
            Private c As config
            Private successor_sum As Double
            Private successor_max As Double
            Private predecessor_sum As Double
            Private predecessor_max As Double

            Public Sub New()
                successors = New unordered_map(Of K, Double)()
                predecessors = New unordered_map(Of K, Double)()
            End Sub

            Private Shared Function sum(ByVal m As unordered_map(Of K, Double)) As Double
                assert(Not m Is Nothing)
                Return m.stream().
                         map(m.second_selector).
                         aggregate(stream(Of Double).aggregators.sum)
            End Function

            Private Shared Function max(ByVal m As unordered_map(Of K, Double)) As Double
                assert(Not m Is Nothing)
                Return m.stream().
                         map(m.second_selector).
                         aggregate(stream(Of Double).aggregators.max)
            End Function

            Public Sub sum(ByVal c As config)
                assert(Not c Is Nothing)
                Me.c = c
                If c.compare = config.comparison.with_average OrElse
                   c.compare = config.comparison.with_exclusive_average Then
                    successor_sum = sum(successors)
                    If c.bidirectional Then
                        predecessor_sum = sum(predecessors)
                    End If
                ElseIf c.compare = config.comparison.with_max Then
                    successor_max = max(successors)
                    If c.bidirectional Then
                        predecessor_max = max(predecessors)
                    End If
                End If
            End Sub

            Private Function ratio(ByVal m As unordered_map(Of K, Double),
                                   ByVal i As K,
                                   ByVal sum As Double,
                                   ByVal max As Double) As Double
                Dim it As unordered_map(Of K, Double).iterator = Nothing
                it = m.find(i)
                assert(it <> m.end())

                If c.compare = config.comparison.with_average Then
                    assert((+it).second <= sum)
                    Return (+it).second * m.size() / sum
                End If
                If c.compare = config.comparison.with_max Then
                    assert((+it).second <= max)
                    Return (+it).second / max
                End If
                If c.compare = config.comparison.with_exclusive_average Then
                    assert((+it).second <= sum)
                    If m.size() = uint32_1 Then
                        Return Double.MaxValue
                    End If
                    Return (+it).second * (m.size() - uint32_1) / (sum - (+it).second)
                End If
                If c.compare = config.comparison.raw Then
                    Return (+it).second
                End If
                assert(False)
                Return 0
            End Function

            Public Function successor_ratio(ByVal i As K) As Double
                Return ratio(successors, i, successor_sum, successor_max)
            End Function

            Public Function predecessor_ratio(ByVal i As K) As Double
                assert(c.bidirectional)
                Return ratio(predecessors, i, predecessor_sum, predecessor_max)
            End Function
        End Class

        Public NotInheritable Class config
            Public Enum comparison
                with_average
                with_max
                with_exclusive_average
                raw
            End Enum

            Public compare As comparison
            Public bidirectional As Boolean

            Public Sub New()
                compare = comparison.with_average
                bidirectional = True
            End Sub
        End Class

        Private ReadOnly m As unordered_map(Of K, bind)
        Private ReadOnly c As config

        Public Sub New()
            Me.New(New config())
        End Sub

        Public Sub New(ByVal c As config)
            assert(Not c Is Nothing)
            Me.c = c
            Me.m = New unordered_map(Of K, bind)()
        End Sub

        Public Function accumulate(ByVal a As K, ByVal b As K, ByVal v As Double) As trainer
            assert(v > 0)
            m(a).successors(b) += v
            If c.bidirectional Then
                m(b).predecessors(a) += v
            End If
            Return Me
        End Function

        Public Function accumulate(ByVal a As K, ByVal b As K) As trainer
            Return accumulate(a, b, 1)
        End Function

        Private Function normalize(ByVal k As K,
                                   ByVal b As bind,
                                   ByVal m As unordered_map(Of K, bind)) As unordered_map(Of K, Double)
            assert(Not b Is Nothing)
            Return b.successors.
                     stream().
                     map(b.successors.mapper(
                             Function(ByVal successor As K, ByVal v As Double) As first_const_pair(Of K, Double)
                                 Return first_const_pair.emplace_of(
                                            successor,
                                            b.successor_ratio(successor) *
                                            If(c.bidirectional, m(successor).predecessor_ratio(k), 1.0))
                             End Function)).
                     collect(Of unordered_map(Of K, Double))()
        End Function

        Private Function normalize() As unordered_map(Of K, unordered_map(Of K, Double))
            m.stream().
              foreach(m.on_second(Sub(ByVal v As bind)
                                      v.sum(c)
                                  End Sub))
            Return m.stream().
                     map(m.mapper(Function(ByVal k As K, ByVal b As bind) _
                                          As first_const_pair(Of K, unordered_map(Of K, Double))
                                      Return first_const_pair.emplace_of(k, normalize(k, b, m))
                                  End Function)).
                     collect(Of unordered_map(Of K, unordered_map(Of K, Double)))()
        End Function

        Public Function dump() As model
            Return New model(normalize())
        End Function
    End Class
End Class
