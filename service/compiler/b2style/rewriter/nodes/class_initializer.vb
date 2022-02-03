
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.rewriters
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Public NotInheritable Class class_initializer
        Implements code_gen(Of typed_node_writer)

        Private ReadOnly l As code_gens(Of typed_node_writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of typed_node_writer))
            assert(Not b Is Nothing)
            Me.l = b
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 4 OrElse n.child_count() = 5)
            Dim class_name As String = n.child(0).input()
            ' construct and destruct are always in global namespace.
            If Not (o.append(n.child(0)) AndAlso
                    o.append(n.child(1)) AndAlso
                    o.append(";") AndAlso
                    o.append(_namespace.bstyle_format.in_global_namespace("construct")) AndAlso
                    o.append("(") AndAlso
                    o.append(n.child(1)) AndAlso
                    If(n.child_count() = 5, o.append(",") AndAlso o.append(n.child(3)), True) AndAlso
                    o.append(")")) Then
                Return False
            End If
            scope.current().when_end_scope(
                Sub()
                    assert(o.append(_namespace.bstyle_format.in_global_namespace("destruct")) AndAlso
                           o.append("(") AndAlso
                           o.append(n.child(1)) AndAlso
                           o.append(");"))
                End Sub)
            scope.current().call_hierarchy().to(_namespace.with_global_namespace("construct"))
            scope.current().call_hierarchy().to(_namespace.with_global_namespace("destruct"))
            Return True
        End Function
    End Class
End Class
