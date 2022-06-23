
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Private NotInheritable Class heap_struct_function_call
        Implements code_gen(Of typed_node_writer)

        Private Function build(ByVal n As typed_node, ByVal o As typed_node_writer) As Boolean _
                Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            Return code_gens().typed(Of function_call).build(heap_struct_name.bstyle_function(n.child(0)), n, o)
        End Function
    End Class
End Class
