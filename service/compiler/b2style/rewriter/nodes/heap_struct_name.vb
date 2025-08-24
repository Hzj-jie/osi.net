
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Private NotInheritable Class heap_struct_name
        Implements code_gen(Of typed_node_writer)

        Private Shared Function bstyle_format(ByVal n As typed_node) As String
            assert(n.child_count() = 3)
            assert(n.child(0).child_count() = 4)
            ' Convert heap_name[index].value to heap_name.value[index]
            Return streams.of(n.child(0).child(0),
                              n.child(1),
                              n.child(2),
                              n.child(0).child(1),
                              n.child(0).child(2),
                              n.child(0).child(3)).
                           map(Function(ByVal x As typed_node) As String
                                   assert(Not x Is Nothing)
                                   Return x.input_without_ignored()
                               End Function).
                           collect_by(stream(Of String).collectors.to_str("")).
                           ToString()
        End Function

        Private Function build(ByVal n As typed_node, ByVal o As typed_node_writer) As Boolean _
                Implements code_gen(Of typed_node_writer).build
            assert(Not o Is Nothing)
            o.append(bstyle_format(n))
            Return True
        End Function
    End Class
End Class
