
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.automata

Partial Public NotInheritable Class b3style
    Partial Private NotInheritable Class function_call_with_template
        Public Shared Function without_return(ByVal n As typed_node, ByVal o As logic_writer) As Boolean
            Return build(n, o, AddressOf function_call.with_parameters.without_return)
        End Function
    End Class
End Class
