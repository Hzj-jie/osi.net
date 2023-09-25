
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Private NotInheritable Class name
        Implements code_gen(Of typed_node_writer)

        Private Function build(ByVal n As typed_node,
                               ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            If n.type_name.Equals("name") AndAlso n.descentdant_of("value-declaration", "struct-body") Then
                ' Ignore namespace prefix for variables within the structure.
                o.append(_namespace.bstyle_format.in_global_namespace(n.input_without_ignored()))
                Return True
            End If
            If n.type_name.Equals("raw-type-name") AndAlso
               n.descentdant_of("paramtype") AndAlso
               (n.descentdant_of("template-type-name") OrElse n.descentdant_of("function-name-with-template")) Then
                ' Local type name should be used when expanding templates.
                o.append(scope.current_namespace_t.of(n.input_without_ignored()))
                Return True
            End If
            o.append(_namespace.bstyle_format.of(n))
            Return True
        End Function
    End Class
End Class
