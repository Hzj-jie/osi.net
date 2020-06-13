
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class wordtracer
    Partial Public NotInheritable Class cjk
        Public NotInheritable Class breaking_word
            Public Shared Function distribute(ByVal m As onebound(Of Char).model) _
                                       As unordered_map(Of Char, normal_distribution)
                assert(Not m Is Nothing)
                Dim r As unordered_map(Of Char, vector(Of Double)) = Nothing
                r = New unordered_map(Of Char, vector(Of Double))()
                m.flat_map().foreach(Sub(ByVal p As first_const_pair(Of const_pair(Of Char, Char), Double))
                                         r(p.first.first).emplace_back(p.second)
                                     End Sub)
                Return r.stream().
                         filter(r.second_filter(Function(ByVal v As vector(Of Double)) As Boolean
                                                    Return v.size() >= 5
                                                End Function)).
                         map(r.second_mapper(Function(ByVal v As vector(Of Double)) As normal_distribution
                                                 Return normal_distribution.estimator.estimate(+v)
                                             End Function)).
                         collect(Of unordered_map(Of Char, normal_distribution))()
            End Function

            Private Sub New()
            End Sub
        End Class
    End Class
End Class
