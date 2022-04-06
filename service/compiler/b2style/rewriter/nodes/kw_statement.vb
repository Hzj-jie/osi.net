
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Private NotInheritable Class kw_statement
        Implements code_gen(Of typed_node_writer)

        Public Shared ReadOnly instance As New kw_statement()

        Private Sub New()
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Return assert(o.append("""" + n.ancestor_of("sentence").input() + """"))
        End Function
    End Class
End Class

