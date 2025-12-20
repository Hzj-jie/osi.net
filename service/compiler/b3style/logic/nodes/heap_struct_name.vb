
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata

Partial Public NotInheritable Class b3style
    Private NotInheritable Class heap_struct_name
        Implements code_gen(Of logic_writer)

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Dim t As tuple(Of String, typed_node) = b2style.heap_struct_name.logic_order(n)
            Return heap_name.as_raw_variable_name(t.second(), t.first(), o)
        End Function
    End Class
End Class
