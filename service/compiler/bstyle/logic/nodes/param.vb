
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class param
        Inherits code_gen_wrapper(Of writer)
        Implements code_gen(Of writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of writer))
            MyBase.New(b)
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements code_gen(Of writer).build
            assert(Not n Is Nothing)
            assert(n.child_count() = 2 OrElse n.child_count() = 3)
            struct.forward_in_stack(n.child(0).word().str(),
                                    n.last_child().word().str())
            Dim params As struct_def = Nothing
            If Not scope.current().structs().resolve(n.child(0).word().str(),
                                                     n.last_child().word().str(),
                                                     params) Then
                params = struct_def.of_single_data_slot_variable(
                             New single_data_slot_variable(n.child(0).word().str(),
                                                           n.last_child().word().str()))
            End If
            Dim ps As vector(Of builders.parameter) =
                    params.expanded.
                           stream().
                           map(AddressOf single_data_slot_variable.to_builders_parameter).
                           collect(Of vector(Of builders.parameter))()
            If n.child_count() = 3 Then
                assert(n.child(1).leaf())
                assert(n.child(1).type_name.Equals("reference"))
                ps = ps.stream().
                        map(AddressOf builders.parameter.to_ref).
                        collect(Of vector(Of builders.parameter))()
            End If
            scope.current().params().pack(ps)
            Return True
        End Function
    End Class
End Class
