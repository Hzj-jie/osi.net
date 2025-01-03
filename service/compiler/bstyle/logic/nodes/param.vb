
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports builders = osi.service.compiler.logic.builders

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class param
        Implements code_gen(Of logic_writer)

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(n.child_count() = 2)
            Dim type_node As typed_node = n.child(0)
            struct.forward_in_stack(type_node.child(0), n.last_child())
            Dim params As scope.struct_def = Nothing
            If Not scope.current().structs().resolve(scope.type_name.of(type_node.child(0)),
                                                     scope.variable_name.of(n.last_child()),
                                                     params) Then
                params = scope.struct_def.of_primitive(scope.type_name.of(type_node.child(0)),
                                                       scope.variable_name.of(n.last_child()))
            End If
            Dim ps As stream(Of builders.parameter) = params.primitives()
            If type_node.child_count() = 2 Then
                ' assert(type_node.child(1).leaf())  ' b3style uses bit-and as reference.
                assert(type_node.child(1).type_name.Equals("reference"))
                ps = ps.map(AddressOf builders.parameter.to_ref)
            End If
            scope.current().params().pack(ps)
            Return True
        End Function
    End Class
End Class
