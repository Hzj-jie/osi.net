
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class onebound(Of K)
    Public NotInheritable Class trainer
        Public NotInheritable Class bind
            Public ending As Double
            Public ReadOnly successors As unordered_map(Of K, Double)
            Public ReadOnly predecessors As unordered_map(Of K, Double)
            Private successor_sum As Double
            Private predecessor_sum As Double

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

            Public Sub sum()
                successor_sum = sum(successors) + ending
                predecessor_sum = sum(predecessors)
            End Sub

            Private Shared Function exclusive_average(ByVal m As unordered_map(Of K, Double),
                                                      ByVal i As K,
                                                      ByVal sum As Double) As Double
                Dim it As unordered_map(Of K, Double).iterator = Nothing
                it = m.find(i)
                assert(it <> m.end())
                assert((+it).second <= sum)
                Return (+it).second / (sum - (+it).second)
            End Function

            Public Function successor_exclusive_average(ByVal i As K) As Double
                Return exclusive_average(successors, i, successor_sum)
            End Function

            Public Function predecessor_exclusive_average(ByVal i As K) As Double
                Return exclusive_average(predecessors, i, predecessor_sum)
            End Function
        End Class

        Private ReadOnly m As unordered_map(Of K, bind)

        Public Sub New()
            m = New unordered_map(Of K, bind)()
        End Sub

        Public Function accumulate(ByVal a As K, ByVal b As K, ByVal v As Double) As trainer
            assert(v > 0)
            m(a).successors(b) += v
            m(b).predecessors(a) += v
            Return Me
        End Function

        Public Function accumulate(ByVal a As K, ByVal b As K) As trainer
            Return accumulate(a, b, 1)
        End Function

        Public Function accumulate(ByVal a As K, ByVal v As Double) As trainer
            assert(v > 0)
            m(a).ending += v
            Return Me
        End Function

        Public Function accumulate(ByVal a As K) As trainer
            Return accumulate(a, 1)
        End Function

        Private Shared Function normalize(ByVal k As K,
                                          ByVal b As bind,
                                          ByVal m As unordered_map(Of K, bind)) As unordered_map(Of K, Double)
            assert(Not b Is Nothing)
            Return b.successors.
                     stream().
                     map(b.successors.mapper(
                             Function(ByVal successor As K, ByVal v As Double) As first_const_pair(Of K, Double)
                                 Return first_const_pair.emplace_of(successor,
                                                                    min(1.0,
                                                                        b.successor_exclusive_average(successor) *
                                                                        m(successor).predecessor_exclusive_average(k)))
                             End Function)).
                     collect(Of unordered_map(Of K, Double))()
        End Function

        Private Function normalize() As unordered_map(Of K, unordered_map(Of K, Double))
            m.stream().
              foreach(m.on_second(Sub(ByVal v As bind)
                                      v.sum()
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
