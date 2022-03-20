﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.rewriters
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Private NotInheritable Class class_initializer
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
            If Not l.of(n.child(0)).build(o) OrElse
               Not l.of(n.child(1)).build(o) OrElse
               Not o.append(";") Then
                Return False
            End If
            ' Functions are always in global namespace.
            ' The last semi-colon is haniled by class_initializer_with_semi_colon.
            ' But add another one to avoid potential risk.
            If Not (o.append(_namespace.bstyle_format.in_global_namespace("construct")) AndAlso
                    o.append("(") AndAlso
                    o.append(_namespace.bstyle_format.of(n.child(1))) AndAlso
                    If(n.child_count() = 5, o.append(",") AndAlso l.of(n.child(3)).build(o), True) AndAlso
                    o.append(");")) Then
                Return False
            End If
            scope.current().when_end_scope(
                Sub()
                    assert(o.append(_namespace.bstyle_format.in_global_namespace("destruct")) AndAlso
                           o.append("(") AndAlso
                           o.append(_namespace.bstyle_format.of(n.child(1))) AndAlso
                           o.append(");"))
                End Sub)
            scope.current().call_hierarchy().to(_namespace.with_global_namespace("construct"))
            scope.current().call_hierarchy().to(_namespace.with_global_namespace("destruct"))
            Return True
        End Function
    End Class
End Class
