
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata

Partial Public NotInheritable Class rewriters
    Inherits code_gens(Of typed_node_writer)

    Public NotInheritable Class leaf
        Inherits rewriter_wrapper
        Implements rewriter

        Private Sub New(ByVal l As rewriters)
            MyBase.New(l)
        End Sub

        Public Shared Function registerer(ByVal s As String) As Action(Of rewriters)
            Return Sub(ByVal l As rewriters)
                       assert(Not l Is Nothing)
                       assert(Not s.null_or_whitespace())
                       l.register(s, New leaf(l))
                   End Sub
        End Function

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(n.leaf())
            o.append(n)
            Return True
        End Function
    End Class
End Class
