
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata

Partial Public NotInheritable Class b3style
    Private NotInheritable Class self_value_clause
        Implements code_gen(Of logic_writer)

        Private Const self_prefix As String = "self-"

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(n.child_count() = 3)
            Return value_clause.build(n.child(0),
                                      Function() As Boolean
                                          Return binary_operation_value.build(
                                                         n.child(0),
                                                         n.child(1).child().type_name.Substring(self_prefix.Length()),
                                                         n.child(2),
                                                         o)
                                      End Function,
                                      o)
        End Function
    End Class
End Class
