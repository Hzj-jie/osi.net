
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public Class [stop]
        Implements exportable

        Public Function export(ByVal scope As scope,
                               ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not o Is Nothing)
            o.emplace_back(instruction_builder.str(command.stop))
            Return True
        End Function
    End Class
End Namespace
