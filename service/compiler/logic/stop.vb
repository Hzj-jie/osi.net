
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class _stop
        Implements instruction_gen

        Public Function build(ByVal o As vector(Of String)) As Boolean Implements instruction_gen.build
            assert(Not o Is Nothing)
            o.emplace_back(instruction_builder.str(command.stop))
            Return True
        End Function
    End Class
End Namespace
