﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata

Partial Public NotInheritable Class b2style
    Private NotInheritable Class delegate_with_semi_colon
        Implements template.name_node, template.name

        Private Function name_node_of(ByVal n As typed_node,
                                      ByRef o As typed_node) As Boolean Implements template.name_node.of
            assert(Not n Is Nothing)
            o = n.child(0).child(2)
            Return True
        End Function

        Private Function name_of(ByVal n As typed_node, ByRef o As String) As Boolean Implements template.name.of
            o = template.name_of(template.name_node_of(n), template.type_param_count(n))
            Return True
        End Function
    End Class
End Class