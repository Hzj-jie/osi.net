
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Private NotInheritable Class template
        Implements code_gen(Of typed_node_writer)

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            Return scope.current().template().define(n)
        End Function
    End Class
End Class
