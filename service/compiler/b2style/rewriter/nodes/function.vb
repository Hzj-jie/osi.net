
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Private NotInheritable Class _function
        Implements code_gen(Of typed_node_writer), template.name_node, template.name

        Public Function build(ByVal n As typed_node,
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

        Private Function name_node_of(ByVal n As typed_node) As typed_node Implements template.name_node.of
            Return template.default_name_node_of(n).child(1)
        End Function

        Private Function [of](ByVal n As typed_node) As String Implements template.name.of
            Return template.default_name_of(n)
        End Function
    End Class
End Class
