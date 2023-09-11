
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata

Partial Public NotInheritable Class b3style
    Private NotInheritable Class binary_operation_value
        Implements code_gen(Of logic_writer)

        Public Function build(ByVal n As typed_node,
                              ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 3)
            Dim function_name As String =
                    scope.current_namespace_t.in_global_namespace(
                    scope.current_namespace_t.with_namespace("b2style", n.child(1).type_name))
            value_list.build(Sub(ByVal a As Action(Of typed_node))
                                 assert(Not a Is Nothing)
                                 a(n.child(0))
                                 a(n.child(2))
                             End Sub,
                             o)
            Return function_call.build(function_name, o)
        End Function
    End Class
End Class
