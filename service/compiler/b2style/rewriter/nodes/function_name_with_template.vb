
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata

Partial Public NotInheritable Class b2style
    Private NotInheritable Class function_name_with_template
        Implements template.name

        Private Function name_of(ByVal n As typed_node, ByRef o As String) As Boolean Implements template.name.of
            assert(Not n Is Nothing)
            assert(n.child_count() = 4)
            Dim t As tuple(Of String, String) = Nothing
            If function_call.split_struct_function(n.child(0).input_without_ignored(), t) Then
                o = template.name_of(t.second(), n.child(2).child_count())
            Else
                o = template.name_of(n.child(0), n.child(2).child_count())
            End If
            Return True
        End Function
    End Class
End Class
