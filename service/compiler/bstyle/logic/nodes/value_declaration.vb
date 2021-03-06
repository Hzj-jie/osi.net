﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class value_declaration
        Inherits logic_gen_wrapper
        Implements logic_gen

        Private ReadOnly ta As type_alias

        Public Sub New(ByVal b As logic_gens, ByVal ta As type_alias)
            MyBase.New(b)
            assert(Not ta Is Nothing)
            Me.ta = ta
        End Sub

        Public Shared Sub register(ByVal b As logic_gens, ByVal l As logic_rule_wrapper)
            assert(Not b Is Nothing)
            assert(Not l Is Nothing)
            b.register(New value_declaration(b, l.type_alias))
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 2)
            builders.of_define(ta, n.child(1).word().str(), n.child(0).word().str()).to(o)
            Return True
        End Function
    End Class
End Class
