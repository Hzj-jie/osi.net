
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class onebound(Of K)
    Public NotInheritable Class trainer

        Private ReadOnly m As New unordered_map(Of K, unordered_map(Of K, Double))()

        Public Function accumulate(ByVal a As K, ByVal b As K, ByVal v As Double) As trainer
            assert(v > 0)
            m(a)(b) += v
            Return Me
        End Function

        Public Function accumulate(ByVal a As K, ByVal b As K) As trainer
            Return accumulate(a, b, 1)
        End Function

        Public Function dump() As model
            Return New model(m)
        End Function
    End Class
End Class
