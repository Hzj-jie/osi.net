
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class kw_line
        Implements code_gen(Of logic_writer)

        Public Shared ReadOnly instance As New kw_line()

        Private Sub New()
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            Return _integer.build(CInt(n.char_start()), o)
        End Function
    End Class
End Class

