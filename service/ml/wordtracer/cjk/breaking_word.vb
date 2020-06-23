
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class wordtracer
    Partial Public NotInheritable Class cjk
        Public NotInheritable Class breaking_word
            Public Shared Function normal_distribute(ByVal m As onebound(Of Char).model) _
                                       As unordered_map(Of Char, normal_distribution)
                assert(Not m Is Nothing)
                Dim r As unordered_map(Of Char, vector(Of Double)) = Nothing
                r = New unordered_map(Of Char, vector(Of Double))()
                m.flat_map().foreach(Sub(ByVal p As first_const_pair(Of const_pair(Of Char, Char), Double))
                                         r(p.first.first).emplace_back(p.second)
                                     End Sub)
                Return r.stream().
                         filter(r.second_filter(Function(ByVal v As vector(Of Double)) As Boolean
                                                    Return v.stream().
                                                             collect_by(stream(Of Double).collectors.count_unique()).
                                                             p >= 5
                                                End Function)).
                         map(r.second_mapper(Function(ByVal v As vector(Of Double)) As normal_distribution
                                                 Return normal_distribution.estimator.estimate(+v)
                                             End Function)).
                         collect(Of unordered_map(Of Char, normal_distribution))()
            End Function

            Public Shared Function expo_distribute(ByVal m As onebound(Of Char).model) _
                                       As unordered_map(Of Char, exponential_distribution)
                assert(Not m Is Nothing)
                Dim r As unordered_map(Of Char, vector(Of Double)) = Nothing
                r = New unordered_map(Of Char, vector(Of Double))()
                m.flat_map().foreach(Sub(ByVal p As first_const_pair(Of const_pair(Of Char, Char), Double))
                                         r(p.first.first).emplace_back(p.second)
                                     End Sub)
                Return r.stream().
                         filter(r.second_filter(Function(ByVal v As vector(Of Double)) As Boolean
                                                    Return v.stream().
                                                             collect_by(stream(Of Double).collectors.count_unique()).
                                                             p >= 5
                                                End Function)).
                         map(r.second_mapper(Function(ByVal v As vector(Of Double)) As exponential_distribution
                                                 Return exponential_distribution.estimator.estimate(+v)
                                             End Function)).
                         collect(Of unordered_map(Of Char, exponential_distribution))()
            End Function

            Private Shared Function expo_distribute(ByVal m As onebound(Of Char).model,
                                                    ByVal f As Func(Of exponential_distribution, Double, Double)) _
                                       As unordered_map(Of Char, unordered_map(Of Char, Double))
                assert(Not m Is Nothing)
                assert(Not f Is Nothing)
                Dim r As unordered_map(Of Char, vector(Of const_pair(Of Char, Double))) = Nothing
                r = New unordered_map(Of Char, vector(Of const_pair(Of Char, Double)))()
                m.flat_map().foreach(Sub(ByVal p As first_const_pair(Of const_pair(Of Char, Char), Double))
                                         r(p.first.first).emplace_back(const_pair.of(p.first.second, p.second))
                                     End Sub)
                Return r.stream().
                         map(r.second_mapper(Function(ByVal v As vector(Of const_pair(Of Char, Double))) _
                                                 As unordered_map(Of Char, Double)
                                                 If v.stream().
                                                      map(const_pair(Of Char, Double).second_getter).
                                                      collect_by(stream(Of Double).collectors.count_unique()).p < 5 Then
                                                     Return v.stream().
                                                              map(Function(ByVal i As const_pair(Of Char, Double)) _
                                                                      As first_const_pair(Of Char, Double)
                                                                      Return first_const_pair.of(i.first, 1.0)
                                                                  End Function).
                                                              collect(Of unordered_map(Of Char, Double))()
                                                 End If
                                                 Dim e As exponential_distribution = Nothing
                                                 e = exponential_distribution.
                                                     estimator.
                                                     estimate(v.stream().
                                                                map(const_pair(Of Char, Double).second_getter).
                                                                to_array())
                                                 Return v.stream().
                                                          map(Function(ByVal i As const_pair(Of Char, Double)) _
                                                                  As first_const_pair(Of Char, Double)
                                                                  Return first_const_pair.of(
                                                                             i.first,
                                                                             f(e, i.second))
                                                              End Function).
                                                          collect(Of unordered_map(Of Char, Double))()
                                             End Function)).
                         collect(Of unordered_map(Of Char, unordered_map(Of Char, Double)))()
            End Function

            Public Shared Function cumulative_distributes(ByVal m As onebound(Of Char).model) _
                                       As unordered_map(Of Char, unordered_map(Of Char, Double))
                Return expo_distribute(m,
                                       Function(ByVal e As exponential_distribution, ByVal c As Double) As Double
                                           Return e.cumulative_distribute(c)
                                       End Function)
            End Function

            Public Shared Function expo_possibilities(ByVal m As onebound(Of Char).model) _
                                       As unordered_map(Of Char, unordered_map(Of Char, Double))
                Return expo_distribute(m,
                                       Function(ByVal e As exponential_distribution, ByVal c As Double) As Double
                                           Return e.possibility(c)
                                       End Function)
            End Function

            Public Shared Function cumulative_distributes(
                                       ByVal m As vector(Of unordered_map(Of String, UInt32))) _
                                       As vector(Of unordered_map(Of String, Double))
                Return m.stream().
                         map(Function(ByVal i As unordered_map(Of String, UInt32)) As unordered_map(Of String, Double)
                                 Dim e As exponential_distribution = Nothing
                                 e = exponential_distribution.estimator.estimate(
                                         i.stream().
                                           map(i.second_selector).
                                           map(Function(ByVal x As UInt32) As Double
                                                   Return x
                                               End Function).
                                           to_array())
                                 Return i.stream().
                                          map(Function(ByVal x As first_const_pair(Of String, UInt32)) _
                                                  As first_const_pair(Of String, Double)
                                                  Return first_const_pair.of(x.first, e.cumulative_distribute(x.second))
                                              End Function).
                                          collect(Of unordered_map(Of String, Double))()
                             End Function).
                         collect(Of vector(Of unordered_map(Of String, Double)))()
            End Function

            Private Sub New()
            End Sub
        End Class
    End Class
End Class
