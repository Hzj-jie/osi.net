
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.rewriters
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Public NotInheritable Class paragraph
        Inherits rewriter_wrapper
        Implements rewriter

        <inject_constructor>
        Public Sub New(ByVal b As rewriters)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As rewriters)
            assert(Not b Is Nothing)
            b.register(Of paragraph)()
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Using New scope_wrapper()
                Return l.of(n.child()).build(o)
            End Using
        End Function
    End Class
End Class
