
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Private NotInheritable Class root_type
        Implements code_gen(Of typed_node_writer)

        Private Function build(ByVal n As typed_node,
                               ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            scope.current().root_type_injector()._new(o)
            Return code_gen_of(n.child()).build(o)
        End Function
    End Class
End Class