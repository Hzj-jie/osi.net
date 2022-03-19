﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class param
        Implements code_gen(Of logic_writer)

        Private ReadOnly l As code_gens(Of logic_writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of logic_writer))
            assert(Not b Is Nothing)
            Me.l = b
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(n.child_count() = 2)
            Dim type_node As typed_node = n.child(0)
            struct.forward_in_stack(type_node.child(0).word().str(),
                                    n.last_child().word().str())
            Dim params As struct_def = Nothing
            If Not scope.current().structs().resolve(type_node.child(0).input_without_ignored(),
                                                     n.last_child().word().str(),
                                                     params) Then
                params = struct_def.of_primitive(type_node.child(0).word().str(), n.last_child().word().str())
            End If
            Dim ps As stream(Of builders.parameter) = params.primitives()
            If type_node.child_count() = 2 Then
                assert(type_node.child(1).leaf())
                assert(type_node.child(1).type_name.Equals("reference"))
                ps = ps.map(AddressOf builders.parameter.to_ref)
            End If
            scope.current().params().pack(ps)
            Return True
        End Function
    End Class
End Class
