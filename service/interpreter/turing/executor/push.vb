
Imports osi.root.connector

Namespace turing.executor
    Public Class push
        Implements instruction

        Private ReadOnly location As location

        Public Sub New(ByVal location As location)
            assert(Not location Is Nothing)
            Me.location = location
        End Sub

        Public Function execute(ByVal processor As processor) As Boolean Implements instruction.execute
            assert(Not processor Is Nothing)
            Dim var As variable = Nothing
            If processor.variable(location, var) Then
                processor.space.emplace_back(var)
                Return processor.move_next()
            Else
                Return False
            End If
        End Function
    End Class
End Namespace
