
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class kw_statement
        Implements code_gen(Of logic_writer)

        Public Shared ReadOnly instance As New kw_statement()

        Private Sub New()
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Return assert(o.append("""" + n.ancestor_of("sentence").input().c_escape() + """"))
        End Function
    End Class
End Class

