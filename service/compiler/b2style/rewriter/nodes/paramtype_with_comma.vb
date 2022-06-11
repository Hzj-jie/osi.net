
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Private NotInheritable Class paramtype_with_comma
        Implements code_gen(Of typed_node_writer)

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            If n.descentdant_of("template-type-name") OrElse
               n.descentdant_of("function-name-with-template") Then
                ' Expect to dump only the type name.
                Return code_gen_of(n.child(0)).build(o)
            End If
            Return code_gens().of_all_children(n).build(o)
        End Function
    End Class
End Class
