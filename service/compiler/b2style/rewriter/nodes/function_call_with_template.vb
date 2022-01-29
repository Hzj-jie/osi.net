
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.rewriters
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Public NotInheritable Class function_call_with_template
        Implements code_gen(Of typed_node_writer)

        Private ReadOnly l As code_gens(Of typed_node_writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of typed_node_writer))
            assert(Not b Is Nothing)
            Me.l = b
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As typed_node_writer) As Boolean _
                Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(n.child_count() = 6 OrElse n.child_count() = 7)
            Dim extended_type As String = Nothing
            If Not scope.current().template().resolve(l, n, extended_type) Then
                Return False
            End If
            ' extended_type = _namespace.bstyle_format.of(extended_type)
            scope.current().call_hierarchy().to(extended_type)
            Return o.append(extended_type) AndAlso
                   o.append(n.child(4)) AndAlso
                   o.append(n.child(5)) AndAlso
                   If(n.child_count() = 7, o.append(n.child(6)), True)
        End Function
    End Class
End Class
