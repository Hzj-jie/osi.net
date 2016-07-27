
Imports osi.root.connector

Namespace turing.executor
    Public Class [goto]
        Implements instruction

        Private ReadOnly destination As location

        Public Sub New(ByVal destination As location)
            assert(Not destination Is Nothing)
            Me.destination = destination
        End Sub

        Public Function execute(ByVal processor As processor) As Boolean Implements instruction.execute
            assert(Not processor Is Nothing)
            Return processor.move(destination)
        End Function
    End Class
End Namespace
