
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata

Partial Public NotInheritable Class b2style
    Partial Private NotInheritable Class template_type_name
        Implements scope.template_t.name

        Private Function name_of(ByVal n As typed_node, ByRef o As String) As Boolean _
                                Implements scope.template_t.name.of
            assert(Not n Is Nothing)
            assert(n.child_count() = 4)
            o = scope.template_t.name_of(n.child(0), n.child(2).child_count())
            Return True
        End Function
    End Class
End Class

