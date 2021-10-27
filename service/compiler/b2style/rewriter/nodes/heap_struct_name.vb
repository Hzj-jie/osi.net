
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Public NotInheritable Class heap_struct_name
        Implements rewriter

        Private Shared ReadOnly instance As New heap_struct_name()

        Public Shared Sub register(ByVal b As rewriters)
            assert(Not b Is Nothing)
            b.register(instance)
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As typed_node_writer) As Boolean _
                Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 3)
            assert(n.child(0).child_count() = 4)
            o.append(streams.of(n.child(0).child(0),
                                n.child(1),
                                n.child(2),
                                n.child(0).child(1),
                                n.child(0).child(2),
                                n.child(0).child(3)).
                             map(Function(ByVal x As typed_node) As String
                                     assert(Not x Is Nothing)
                                     Return x.input()
                                 End Function).
                             collect_by(stream(Of String).collectors.to_str(empty_string)))
            Return True
        End Function
    End Class
End Class
