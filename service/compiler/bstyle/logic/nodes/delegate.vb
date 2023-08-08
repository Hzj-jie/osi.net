
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports builders = osi.service.compiler.logic.builders

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class _delegate
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
            Dim return_type As String = scope.normalized_type.of(n.child(1).input_without_ignored()).full_type()
            ' The user side treat the delegate "name" as a type.
            Dim name As String = scope.normalized_type.of(n.child(2).input_without_ignored()).full_type()
            ps = ps.stream().
                    map(AddressOf scope.normalized_type.of).
                    map(AddressOf builders.parameter_type.full_type).
                    collect_to(Of vector(Of String))()
            Return scope.current().delegates().define(return_type,
                                                      name,
                                                      builders.parameter_type.from(ps)) AndAlso
                   builders.of_callee_ref(scope.normalized_type.logic_type_of(name),
                                          scope.normalized_type.logic_type_of(return_type),
                                          ps.stream().
                                             map(AddressOf scope.normalized_type.logic_type_of).
                                             collect_to(Of vector(Of String))()).to(o)
        End Function
    End Class
End Class
