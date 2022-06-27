
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.automata

Partial Public NotInheritable Class b2style
    Private NotInheritable Class function_name_with_template
        Implements template.name

        Private Function name_of(ByVal n As typed_node, ByRef o As String) As Boolean Implements template.name.of
            Return function_call_with_template.name_of(n, o)
        End Function
    End Class
End Class
