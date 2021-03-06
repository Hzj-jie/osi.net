﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class ignore_result_function_call
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
            b.register(New ignore_result_function_call(b, l.type_alias))
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Return logic_gen_of(Of function_call).without_return(n.child(), o)
        End Function
    End Class
End Class
