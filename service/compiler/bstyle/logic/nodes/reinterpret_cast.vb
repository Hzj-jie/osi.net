
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class reinterpret_cast
        Implements code_gen(Of logic_writer)

        Public Shared ReadOnly instance As New reinterpret_cast()

        Private Sub New()
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            Return scope.current().variables().redefine(
                       builders.parameter_type.remove_ref(n.child(4).input_without_ignored()),
                       n.child(2).input_without_ignored())
        End Function
    End Class
End Class
