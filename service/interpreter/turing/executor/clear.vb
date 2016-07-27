
Imports osi.root.connector

Namespace turing.executor
    Public Class clear
        Implements instruction

        Public Function execute(ByVal processor As processor) As Boolean Implements instruction.execute
            assert(Not processor Is Nothing)
            processor.space.clear()
            Return processor.move_next()
            Return True
        End Function
    End Class
End Namespace
