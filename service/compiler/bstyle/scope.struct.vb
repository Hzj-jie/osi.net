
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class scope
        Public NotInheritable Class struct_t
            Private Shared ReadOnly root_types As unordered_set(Of String) = unordered_set.of(
            types.zero_type,
            types.variable_type,
            code_types.biguint,
            code_types.bool,
            code_types.int,
            code_types.string,
            code_types.ufloat
            )
            Private ReadOnly s As New unordered_map(Of String, vector(Of builders.parameter))()

            ' TODO: Support value_definition
            Private Function resolve_type(ByVal type As String,
                                          ByVal name As String,
                                          ByVal o As vector(Of builders.parameter)) As Boolean
                assert(Not o Is Nothing)
                type = scope.current().type_alias()(type)
                Dim sub_type As vector(Of builders.parameter) = Nothing
                If root_types.find(type) <> root_types.end() OrElse Not s.find(type, sub_type) Then
                    o.emplace_back(New builders.parameter(type, name))
                    Return True
                End If
                assert(Not sub_type Is Nothing)
                Dim i As UInt32 = 0
                While i < sub_type.size()
                    Dim full_name As String = strcat(name, ".", sub_type(i).name)
                    If Not resolve_type(sub_type(i).type, full_name, o) Then
                        raise_error(error_type.user,
                                    "Undefined type ",
                                    sub_type(i).type,
                                    " for variable ",
                                    full_name)
                        Return False
                    End If
                    i += uint32_1
                End While
                Return True
            End Function

            Public Function define(ByVal type As String, ByVal members As vector(Of builders.parameter)) As Boolean
                assert(Not type.null_or_whitespace())
                assert(Not members Is Nothing)
                Dim o As New vector(Of builders.parameter)()
                Dim i As UInt32 = 0
                While i < members.size()
                    assert(Not members(i) Is Nothing)
                    If Not resolve_type(members(i).type, members(i).name, o) Then
                        Return False
                    End If
                    i += uint32_1
                End While
                If s.emplace(type, o).second() Then
                    Return True
                End If
                raise_error(error_type.user, "Struct type ", type, " has been defined already as: ", s(type))
                Return False
            End Function

            Public Function resolve(ByVal type As String,
                                    ByVal name As String,
                                    ByRef o As vector(Of builders.parameter)) As Boolean
                If Not s.find(type, o) Then
                    ' raise_error(error_type.user, "Struct type ", type, " has not been defined.")
                    ' Do not log, value_declaration and value_definition always check if a struct is defined before
                    ' forwarding the defintion directly to the logic.
                    Return False
                End If
                o = o.stream().
                      map(Function(ByVal member As builders.parameter) As builders.parameter
                              Return New builders.parameter(member.type, strcat(name, ".", member.name))
                          End Function).
                      collect(Of vector(Of builders.parameter))()
                Return True
            End Function
        End Class

        Public Function structs() As struct_t
            Return s
        End Function
    End Class
End Class
