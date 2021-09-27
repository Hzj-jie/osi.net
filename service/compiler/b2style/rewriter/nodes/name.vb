
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.rewriters
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Public NotInheritable Class name
        Inherits rewriter_wrapper
        Implements rewriter

        <inject_constructor>
        Public Sub New(ByVal i As rewriters)
            MyBase.New(i)
        End Sub

        Public Shared Sub register(ByVal b As rewriters)
            assert(Not b Is Nothing)
            b.register(Of name)()
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(n.leaf())
            o.append(code_gen_of(Of namespace_)().bstyle_format(n.word().str()))
            Return True
        End Function
    End Class
End Class
