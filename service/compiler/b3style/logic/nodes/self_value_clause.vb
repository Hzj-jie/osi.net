
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.automata

Partial Public NotInheritable Class b3style
    Private NotInheritable Class self_value_clause
        Implements code_gen(Of logic_writer)

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            Return binary_operation_value.without_return(n, o)
        End Function
    End Class
End Class
