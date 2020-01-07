﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class cstyle
    Public NotInheritable Class value_declaration
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of value_declaration)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            o.append("define")
            assert(n.child_count() = 2)
            If Not b.of(n.child(1)).build(o) Then
                o.err("@value-declaration name ", n.child(1))
                Return False
            End If
            If Not b.of(n.child(0)).build(o) Then
                o.err("@value-declaration type ", n.child(0))
                Return False
            End If
            Return True
        End Function
    End Class
End Class
