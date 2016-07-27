
Imports osi.root.connector

Namespace fullstack.executor
    Public Class assignment
        Implements sentence

        Private ReadOnly location As location
        Private ReadOnly expression As expression

        Public Sub New(ByVal location As location,
                       ByVal expression As expression)
            assert(Not location Is Nothing)
            assert(Not expression Is Nothing)
            Me.location = location
            Me.expression = expression
        End Sub

        Public Sub execute(ByVal domain As domain) Implements sentence.execute
            location.replace(domain, expression.execute(domain))
        End Sub
    End Class
End Namespace
