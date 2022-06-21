
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Private NotInheritable Class template
        Implements code_gen(Of typed_node_writer)

        Private Shared Function build(Of T)(ByVal l As code_gens(Of typed_node_writer),
                                            ByVal n As typed_node,
                                            ByVal f As _do_val_val_ref(Of vector(Of String), typed_node, T, Boolean),
                                            ByRef o As T) As Boolean
            assert(Not l Is Nothing)
            assert(Not n Is Nothing)
            assert(Not f Is Nothing)
            assert(n.child_count() = 2)
            Dim name_node As typed_node = n.child(1).child()
            If name_node.type_name.Equals("class") OrElse name_node.type_name.Equals("function") Then
                name_node = name_node.child(1)
            ElseIf name_node.type_name.Equals("delegate-with-semi-colon") Then
                name_node = name_node.child(0).child(2)
            Else
                assert(False)
            End If
            Dim types As vector(Of String) = l.typed(Of template_head)().type_param_list(n.child(0))
            Return f(types, name_node, o)
        End Function

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            Return build(code_gens(),
                         n,
                         Function(ByVal types As vector(Of String),
                                  ByVal name_node As typed_node,
                                  ByRef unused As Int32) As Boolean
                             Dim name As String = template_template.template_name(name_node, types.size())
                             Return scope.current().template().define(name,
                                                                      types,
                                                                      n,
                                                                      name_node)
                         End Function,
                         0)
        End Function

        ' @VisibleForTesting
        ' TODO: Remove this function.
        Public Shared Function build(ByVal l As code_gens(Of typed_node_writer),
                                     ByVal n As typed_node,
                                     ByRef o As template_template) As Boolean
            Return build(l,
                         n,
                         Function(ByVal types As vector(Of String),
                                  ByVal name_node As typed_node,
                                  ByRef x As template_template) As Boolean
                             Return template_template.of(types, n, name_node, x)
                         End Function,
                         o)
        End Function
    End Class
End Class
