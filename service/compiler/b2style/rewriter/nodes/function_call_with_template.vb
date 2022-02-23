
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.rewriters
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Public NotInheritable Class function_call_with_template
        Implements code_gen(Of typed_node_writer)

        Private ReadOnly l As code_gens(Of typed_node_writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of typed_node_writer))
            assert(b IsNot Nothing)
            Me.l = b
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As typed_node_writer) As Boolean _
                Implements code_gen(Of typed_node_writer).build
            assert(n IsNot Nothing)
            Dim extended_type As String = Nothing
            If Not scope.current().template().resolve(l, n.child(0), extended_type) Then
                Return False
            End If
            Return l.typed(Of function_call).build(_namespace.with_global_namespace(extended_type), n, o)
        End Function
    End Class
End Class
