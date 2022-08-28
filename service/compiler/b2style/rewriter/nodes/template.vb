
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Public NotInheritable Class template
        Implements code_gen(Of typed_node_writer)

        Private Function build(ByVal n As typed_node,
                               ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            Dim name As String = Nothing
            Dim t As scope.template_template = Nothing
            Return scope.template_builder.of(n, name, t) AndAlso
                   scope.current().template().define(name, t)
        End Function
    End Class
End Class
