
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.automata

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class kw_func
        Implements code_gen(Of logic_writer)

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            Return _string.build(scope.current().current_function().signature(), o)
        End Function
    End Class
End Class

