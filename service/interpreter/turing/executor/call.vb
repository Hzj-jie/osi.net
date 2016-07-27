
Imports osi.root.connector

Namespace turing.executor
    Public Class [call]
        Implements instruction

        Private ReadOnly id As UInt32

        Public Sub New(ByVal id As UInt32)
            Me.id = id
        End Sub

        Public Function execute(ByVal processor As processor) As Boolean Implements instruction.execute
            assert(Not processor Is Nothing)
            Return processor.interrupt(id) AndAlso
                   processor.move_next()
        End Function
    End Class
End Namespace
