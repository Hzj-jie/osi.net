
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata

Partial Public NotInheritable Class b3style
    Private NotInheritable Class binary_operation_value
        Implements code_gen(Of logic_writer)

        Public Shared Function operation_function_name(ByVal op As String) As String
            assert(Not op.null_or_whitespace())
            Return scope.namespace_t.fully_qualified_name("b2style", op.Replace("-"c, "_"c))
        End Function

        Private Shared Function build(ByVal n As typed_node,
                                      ByVal fc As Func(Of String, logic_writer, Boolean),
                                      ByVal o As logic_writer) As Boolean
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(Not fc Is Nothing)
            assert(n.child_count() = 3)
            Return value_list.build(Sub(ByVal a As Action(Of typed_node))
                                        assert(Not a Is Nothing)
                                        a(n.child(0))
                                        a(n.child(2))
                                    End Sub,
                                    o) AndAlso
                   fc(operation_function_name(n.child(1).type_name), o)
        End Function

        Public Shared Function without_return(ByVal n As typed_node,
                                              ByVal o As logic_writer) As Boolean
            Return build(n, AddressOf function_call.without_return, o)
        End Function

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            Return build(n, AddressOf function_call.build, o)
        End Function
    End Class
End Class
