
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Private NotInheritable Class function_call_with_template
        Implements code_gen(Of typed_node_writer)

        Public Function build(ByVal n As typed_node, ByVal o As typed_node_writer) As Boolean _
                Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            Dim extended_type As String = Nothing
            If Not scope.current().template().resolve(n.child(0), extended_type) Then
                Return False
            End If
            Return code_gens().typed(Of function_call).build(_namespace.with_global_namespace(extended_type), n, o)
        End Function
    End Class
End Class
