
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class struct_def
        Public ReadOnly nested_structs As vector(Of builders.parameter)
        Public ReadOnly expanded As vector(Of single_data_slot_variable)

        Public Sub New()
            Me.New(New vector(Of builders.parameter)(), New vector(Of single_data_slot_variable)())
        End Sub

        Public Sub New(ByVal nested_structs As vector(Of builders.parameter),
                       ByVal expanded As vector(Of single_data_slot_variable))
            assert(Not nested_structs Is Nothing)
            assert(Not expanded Is Nothing)
            Me.nested_structs = nested_structs
            Me.expanded = expanded
            Me.nested_structs.
               stream().
               foreach(Sub(ByVal p As builders.parameter)
                           assert(Not p Is Nothing)
                           assert(Not p.ref)
                       End Sub)
        End Sub

        Public Shared Function of_single_data_slot_variable(ByVal s As single_data_slot_variable) As struct_def
            Return New struct_def(New vector(Of builders.parameter)(), vector.emplace_of(s))
        End Function

        Public Function with_nested_structs(ByVal type As String, ByVal name As String) As struct_def
            nested_structs.emplace_back(nested_struct(type, name))
            Return Me
        End Function

        Public Shared Function nested_struct(ByVal type As String, ByVal name As String) As builders.parameter
            Dim r As New builders.parameter(scope.current().type_alias()(type), name)
            assert(Not r.ref)
            Return r
        End Function

        Public Function append_prefix(ByVal name As String) As struct_def
            Return New struct_def(
                           nested_structs.stream().
                                          map(Function(ByVal n As builders.parameter) As builders.parameter
                                                  assert(Not n Is Nothing)
                                                  Return n.map_name(Function(ByVal nname As String) As String
                                                                        Return name + "." + nname
                                                                    End Function)
                                              End Function).
                                          collect(Of vector(Of builders.parameter))(),
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
