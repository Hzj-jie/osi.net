
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Partial Private NotInheritable Class _function
        Implements code_gen(Of typed_node_writer)

        Private Function build(ByVal n As typed_node,
                               ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Dim function_name As String = _namespace.bstyle_format.of(n.child(1))
            Using scope.current().start_scope().current_function().define(function_name)
                Dim fo As New typed_node_writer()
                Return code_gens().of_all_children(n).build(fo) AndAlso
                       o.append(scope.current().call_hierarchy().filter(function_name, AddressOf fo.str))
            End Using
        End Function

        Private Shared Function param_types(ByVal n As typed_node) As vector(Of String)
            Return New vector(Of String)()

            ' Following implementation does not work, b2style has no information about the types, the
            ' function_call_with_template cannot create correct parameter types.
#If 0 Then
            assert(Not n Is Nothing)
            If n.child_count() = 5 Then
                Return New vector(Of String)()
            End If
            assert(n.child_count() = 6)
            Return n.child(3).
                     children().
                     map(Function(ByVal node As typed_node) As String
                             assert(Not node Is Nothing)
                             If node.type_name.Equals("param-with-comma") Then
                                 node = node.child(0)
                             End If
                             assert(node.type_name.Equals("param"))
                             Return node.child(0).input_without_ignored()
                         End Function).
                     collect_to(Of vector(Of String))()
#End If
        End Function
    End Class
End Class
