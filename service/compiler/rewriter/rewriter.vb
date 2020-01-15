
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class rewriters
    Inherits code_gens(Of typed_node_writer)

    Public Interface rewriter
        Inherits code_gen(Of typed_node_writer)
    End Interface

    Public MustInherit Class rewriter_wrapper
        Inherits code_gen_wrapper(Of typed_node_writer)

        Protected Sub New(ByVal i As rewriters)
            MyBase.New(i)
        End Sub
    End Class
End Class
