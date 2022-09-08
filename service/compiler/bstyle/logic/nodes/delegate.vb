﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports builders = osi.service.compiler.logic.builders

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class _delegate
        Implements code_gen(Of logic_writer)

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Dim ps As vector(Of String) = Nothing
            If n.child_count() = 5 Then
                ps = New vector(Of String)()
            Else
                ps = code_gens().of_all_children(n.child(4)).dump()
            End If
            Dim ta As scope.type_alias_proxy = scope.current().type_alias()
            Dim return_type As String = ta(n.child(1).input_without_ignored())
            ps = ps.stream().
                    map(AddressOf ta.canonical_of).
                    collect_to(Of vector(Of String))()
            Return scope.current().delegates().define(return_type,
                                                      n.child(2).input_without_ignored(),
                                                      builders.parameter_type.from(ps)) AndAlso
                   builders.of_callee_ref(n.child(2).input_without_ignored(), return_type, ps).to(o)
        End Function
    End Class
End Class
