﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata

Partial Public NotInheritable Class b2style
    Private NotInheritable Class template_head
        Public Function type_param_list(ByVal n As typed_node) As vector(Of String)
            assert(Not n Is Nothing)
            assert(n.child_count() = 4)
            Return code_gens().of_all_children(n.child(2)).dump()
        End Function
    End Class
End Class
