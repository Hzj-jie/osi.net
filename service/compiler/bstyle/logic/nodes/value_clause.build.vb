
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata

Partial Public NotInheritable Class bstyle
    Partial Private NotInheritable Class value_clause
        Private Function build(ByVal name As typed_node, ByVal value As typed_node, ByVal o As logic_writer) As Boolean
            assert(Not name Is Nothing)
            assert(Not value Is Nothing)
            assert(Not o Is Nothing)
            If name.immediate_descentdant_of("variable-name") AndAlso name.type_name.Equals("raw-variable-name") Then
                Return stack_name_build(name.child(), value, o)
            End If
            If name.type_name.Equals("heap-name") Then
                Return heap_name_build(name, value, o)
            End If
            assert(False, "Unsupported assignee: ", name.type_name, " from [", name.input(), "]")
            Return False
        End Function
    End Class
End Class

