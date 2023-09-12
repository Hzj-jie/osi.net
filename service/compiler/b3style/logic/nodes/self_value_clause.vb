
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata

Partial Public NotInheritable Class b3style
    Private NotInheritable Class self_value_clause
        Implements code_gen(Of logic_writer)

        Private Const self_prefix As String = "self-"

        Public Function build(ByVal n As typed_node,
                              ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build

        End Function
    End Class
End Class
