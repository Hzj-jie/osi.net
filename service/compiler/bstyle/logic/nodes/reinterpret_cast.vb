
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
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
            assert(Not n Is Nothing)
            assert(n.child_count() = 6 OrElse n.child_count() = 7)
            Dim type As String = builders.parameter_type.remove_ref(n.child(4).input_without_ignored())
            Dim name As String = n.child(2).input_without_ignored()
            If n.child_count() = 6 Then
                Return scope.current().variables().redefine(type, name)
            End If
            Return scope.current().variables().redefine_heap(type, name)
        End Function
    End Class
End Class
