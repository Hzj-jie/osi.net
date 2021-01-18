
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

            Private Shared Function expo_map(ByVal f As Func(Of exponential_distribution, Double, Double),
                                             Optional ByVal threshold As Double = 0) _
                                        As Func(Of vector(Of const_pair(Of Char, Double)),
                                                   unordered_map(Of Char, Double))
                Return Function(ByVal v As vector(Of const_pair(Of Char, Double))) As unordered_map(Of Char, Double)
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
                                        filter(Function(ByVal p As first_const_pair(Of Char, Double)) As Boolean
                                                   Return p.second >= threshold
                                               End Function).
                                    collect(Of unordered_map(Of Char, Double))()
                       End Function
            End Function

            Private Shared Function expo_cumulative_map(Optional ByVal threshold As Double = 0) _
                                        As Func(Of vector(Of const_pair(Of Char, Double)), unordered_map(Of Char, Double))
                Return expo_map(Function(ByVal e As exponential_distribution, ByVal c As Double) As Double
                                    Return e.cumulative_distribute(c)
                                End Function,
                                threshold)
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
                         map(r.second_mapper(expo_map(f))).
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
                                 Dim es As unordered_map(Of String, exponential_distribution) =
                                     i.stream().
                                       map(Function(ByVal p As first_const_pair(Of String, UInt32)) _
                                               As first_const_pair(Of String, UInt32)
                                               Return first_const_pair.emplace_of(p.first.remove_last(1), p.second)
                                           End Function).
                                       collect_by(stream(Of String).collectors.values(Of UInt32)()).
                                       stream().
                                       map(Function(ByVal p As first_const_pair(Of String, vector(Of UInt32))) _
                                               As first_const_pair(Of String, exponential_distribution)
                                               Return first_const_pair.emplace_of(
                                                          p.first,
                                                          exponential_distribution.estimator.estimate(
                                                              p.second.
                                                                stream().
                                                                map(Function(ByVal x As UInt32) As Double
                                                                        Return x
                                                                    End Function).
                                                                to_array()
                                                          )
                                                      )
                                           End Function).
                                       collect(Of unordered_map(Of String, exponential_distribution))()
                                 Return i.stream().
                                          map(Function(ByVal x As first_const_pair(Of String, UInt32)) _
                                                  As first_const_pair(Of String, Double)
                                                  Return first_const_pair.of(x.first,
                                                                             es(x.first.remove_last(1)).
                                                                                 cumulative_distribute(x.second))
                                              End Function).
                                          concat(es.stream().
                                                    map(Function(
                                                            ByVal p As _
                                                                first_const_pair(Of String, exponential_distribution)) _
                                                                As first_const_pair(Of String, Double)
                                                            Return first_const_pair.emplace_of(p.first, p.second.lambda)
                                                        End Function)).
                                          collect(Of unordered_map(Of String, Double))()
                             End Function).
                         collect(Of vector(Of unordered_map(Of String, Double)))()
            End Function

            Public Shared Function bi_directional_expo_cumulative(ByVal m As onebound(Of Char).model) _
                                       As unordered_map(Of Char, unordered_map(Of Char, Double))
                assert(Not m Is Nothing)
                Dim f As unordered_map(Of Char, vector(Of const_pair(Of Char, Double))) = Nothing
                f.[New]()
                Dim b As unordered_map(Of Char, vector(Of const_pair(Of Char, Double))) = Nothing
                b.[New]()
                m.flat_map().foreach(Sub(ByVal p As first_const_pair(Of const_pair(Of Char, Char), Double))
                                         f(p.first.first).emplace_back(const_pair.of(p.first.second, p.second))
                                         b(p.first.second).emplace_back(const_pair.of(p.first.first, p.second))
                                     End Sub)
                Dim fm As unordered_map(Of Char, unordered_map(Of Char, Double)) =
                    f.stream().
                      map(f.second_mapper(expo_cumulative_map(0.5))).
                      collect(Of unordered_map(Of Char, unordered_map(Of Char, Double)))()
                Dim bm As unordered_map(Of Char, unordered_map(Of Char, Double)) =
                    b.stream().
                      map(b.second_mapper(expo_cumulative_map(0.5))).
                      collect(Of unordered_map(Of Char, unordered_map(Of Char, Double)))()
                Return fm.stream().
                          map(Function(ByVal p As first_const_pair(Of Char, unordered_map(Of Char, Double))) _
                                  As first_const_pair(Of Char, unordered_map(Of Char, Double))
                                  Return first_const_pair.emplace_of(
                                             p.first,
                                             p.second.
                                               stream().
                                               map(Function(ByVal p2 As first_const_pair(Of Char, Double)) _
                                                       As first_const_pair(Of Char, Double)
                                                       Return first_const_pair.emplace_of(
                                                                  p2.first,
                                                                  p2.second * bm(p2.first)(p.first))
                                                   End Function).
                                               filter(p.second.second_filter(Function(ByVal d As Double) As Boolean
                                                                                 Return d > 0
                                                                             End Function)).
                                               collect(Of unordered_map(Of Char, Double))())
                              End Function).
                          filter(fm.second_filter(Function(ByVal m2 As unordered_map(Of Char, Double)) As Boolean
                                                      Return Not m2.empty()
                                                  End Function)).
                          collect(Of unordered_map(Of Char, unordered_map(Of Char, Double)))()
            End Function

            Private Sub New()
            End Sub
        End Class
    End Class
End Class
