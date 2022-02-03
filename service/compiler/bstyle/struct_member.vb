
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    ' TODO: Remove, in favor of builders.parameter.
    Public NotInheritable Class struct_member
        Public ReadOnly type As String
        Public ReadOnly name As String

        Public Sub New(ByVal type As String, ByVal name As String)
            assert(Not type.null_or_whitespace())
            assert(Not name.null_or_whitespace())
            type = scope.current().type_alias()(type)

            Me.type = type
            Me.name = name
        End Sub

        Public Shared Function from_builders_parameter(ByVal p As builders.parameter) As struct_member
            assert(Not p Is Nothing)
            assert(Not p.ref)
            Return New struct_member(p.type, p.name)
        End Function
    End Class

    Public NotInheritable Class struct_def
        Public ReadOnly nested_structs As vector(Of struct_member)
        Public ReadOnly expanded As vector(Of single_data_slot_variable)

        Public Sub New()
            Me.New(New vector(Of struct_member)(), New vector(Of single_data_slot_variable)())
        End Sub

        Public Sub New(ByVal nested_structs As vector(Of struct_member),
                       ByVal expanded As vector(Of single_data_slot_variable))
            assert(Not nested_structs Is Nothing)
            assert(Not expanded Is Nothing)
            Me.nested_structs = nested_structs
            Me.expanded = expanded
        End Sub

        Public Shared Function of_single_data_slot_variable(ByVal s As single_data_slot_variable) As struct_def
            Return New struct_def(New vector(Of struct_member)(), vector.emplace_of(s))
        End Function

        Public Function append_prefix(ByVal name As String) As struct_def
            Return New struct_def(
                           nested_structs.stream().
                                          map(Function(ByVal n As struct_member) As struct_member
                                                  assert(Not n Is Nothing)
                                                  Return New struct_member(n.type, strcat(name, ".", n.name))
                                              End Function).
                                          collect(Of vector(Of struct_member))(),
                           expanded.stream().
                                    map(Function(ByVal n As single_data_slot_variable) As single_data_slot_variable
                                            assert(Not n Is Nothing)
                                            Return New single_data_slot_variable(n.type, strcat(name, ".", n.name))
                                        End Function).
                                    collect(Of vector(Of single_data_slot_variable))())
        End Function

        Public Sub append(ByVal r As struct_def)
            assert(Not r Is Nothing)
            nested_structs.emplace_back(r.nested_structs)
            expanded.emplace_back(r.expanded)
        End Sub
    End Class
End Class
