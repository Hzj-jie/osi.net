
Imports osi.root.connector

Namespace turing.executor
    Public Class [end]
        Implements instruction

        Public Function execute(ByVal processor As processor) As Boolean Implements instruction.execute
            assert(Not processor Is Nothing)
            processor.end()
            Return True
        End Function
    End Class
End Namespace
