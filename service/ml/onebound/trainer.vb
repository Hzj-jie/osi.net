
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class onebound
    Partial Public NotInheritable Class typed(Of K)
        Public NotInheritable Class trainer
            Private ReadOnly m As unordered_map(Of K, unordered_map(Of K, Double))

            Public Sub New()
                m = New unordered_map(Of K, unordered_map(Of K, Double))()
            End Sub

            Public Sub accumulate(ByVal a As K, ByVal b As K, ByVal v As Double)
                assert(v > 0)
                m(a)(b) += v
            End Sub

            Public Function dump() As model
                Return New model(m)
            End Function
        End Class
    End Class
End Class
