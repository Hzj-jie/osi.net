
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata

Partial Public NotInheritable Class rewriters
    Public NotInheritable Class leaf
        Implements code_gen(Of typed_node_writer)

        Private ReadOnly l As code_gens(Of typed_node_writer)

        Private Sub New(ByVal b As code_gens(Of typed_node_writer))
            assert(Not b Is Nothing)
            Me.l = b
        End Sub

        Public Shared Function [of](ByVal s As String) As Action(Of code_gens(Of typed_node_writer))
            Return Sub(ByVal l As code_gens(Of typed_node_writer))
                       assert(Not l Is Nothing)
                       assert(Not s.null_or_whitespace())
                       l.register(s, New leaf(l))
                   End Sub
        End Function

        Private Function build(ByVal n As typed_node,
                               ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(n.leaf())
            o.append(n)
            Return True
        End Function
    End Class
End Class
