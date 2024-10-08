
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class logic
    Private NotInheritable Class _dealloc_heap
        Inherits stack_var_operator

        Public Sub New(ByVal name As String)
            MyBase.New(name)
        End Sub

        Protected Overrides Function process(ByVal ptr As variable, ByVal o As vector(Of String)) As Boolean
            assert(Not ptr Is Nothing)
            assert(Not o Is Nothing)
            o.emplace_back(instruction_builder.str(command.dealloc, ptr))
            Return True
        End Function
    End Class
End Class
