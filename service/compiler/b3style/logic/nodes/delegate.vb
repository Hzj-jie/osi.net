
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports builders = osi.service.compiler.logic.builders

Partial Public NotInheritable Class b3style
    Private NotInheritable Class _delegate
        Implements code_gen(Of logic_writer)

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Dim return_type As builders.parameter_type =
                    New builders.parameter_type(n.child(1).input_without_ignored()).map_type(scope.normalized_type.of)
            ' The user side treat the delegate "name" as a type.
            Dim name As builders.parameter_type =
                    New builders.parameter_type(n.child(2).input_without_ignored()).map_type(scope.normalized_type.of)
            Dim ps As vector(Of builders.parameter_type) = Nothing
            If n.child_count() = 5 Then
                ps = New vector(Of builders.parameter_type)()
            Else
                ps = code_gens().of_all_children(n.child(4)).
                                 dump().
                                 stream().
                                 map(AddressOf builders.parameter_type.of).
                                 map(builders.parameter_type.map_type_with(scope.normalized_type.of)).
                                 collect_to(Of vector(Of builders.parameter_type))()
            End If
            Return scope.current().delegates().define(return_type.full_type(),
                                                      name.full_type(),
                                                      +ps) AndAlso
                   builders.of_callee_ref(name.full_type(),
                                          return_type.full_type(),
                                          ps.stream().
                                             map(AddressOf builders.parameter_type.full_type).
                                             collect_to(Of vector(Of String))()).to(o)
        End Function
    End Class
End Class
