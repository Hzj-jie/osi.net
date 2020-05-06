
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
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

        Private Shared Function normalize(ByVal b As bind) As model.bind
            assert(Not b Is Nothing)
            Dim a As Double = 0
            a = b.independence +
                b.followers.
                  stream().
                  map(b.followers.second_selector).
                  aggregate(stream(Of Double).aggregators.sum)
            Return New model.bind(b.independence / a,
                                  b.followers.
                                    stream().
                                    map(b.followers.second_mapper(Function(ByVal i As Double) As Double
                                                                      Return i / a
                                                                  End Function)).
                                    collect(Of unordered_map(Of K, Double))())
        End Function

        Private Function normalize() As unordered_map(Of K, model.bind)
            Return m.stream().
                     map(m.second_mapper(AddressOf normalize)).
                     collect(Of unordered_map(Of K, model.bind))()
        End Function

        Public Function dump() As model
            Return New model(normalize())
        End Function
    End Class
End Class
