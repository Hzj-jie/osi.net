
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata

Partial Public NotInheritable Class b2style
    Private NotInheritable Class delegate_with_semi_colon
        Implements template.name_node, template.name

        Private Function name_node_of(ByVal n As typed_node) As typed_node Implements template.name_node.of
            assert(Not n Is Nothing)
            Return n.child(0).child(2)
        End Function

        Private Function [of](ByVal n As typed_node) As String Implements template.name.of
            Return template.default_name_of(n)
        End Function
    End Class
End Class
