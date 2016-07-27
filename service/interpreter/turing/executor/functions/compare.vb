
Imports osi.root.connector
Imports osi.root.utils

Namespace turing.executor
    Public Class compare
        Inherits interrupter

        Protected Overrides Function execute(ByVal inputs() As variable) As variable
            assert(array_size(inputs) = 2)
            Dim d1 As Double = 0
            Dim d2 As Double = 0
            d1 = inputs(0).number()
            d2 = inputs(1).number()
            If d1 > d2 Then
                Return New variable(1)
            ElseIf d1 < d2 Then
                Return New variable(-1)
            Else
                Return New variable(0)
            End If
        End Function
    End Class
End Namespace
