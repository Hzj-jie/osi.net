
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports builders = osi.service.compiler.logic.builders

Partial Public Class scope(Of WRITER As {lazy_list_writer, New},
                              __BUILDER As func_t(Of String, WRITER, Boolean),
                              __CODE_GENS As func_t(Of code_gens(Of WRITER)),
                              T As scope(Of WRITER, __BUILDER, __CODE_GENS, T))
    Public NotInheritable Class struct_t
        Private ReadOnly s As New unordered_map(Of String, struct_def)()

        ' TODO: Support value_definition
        Private Function resolve_type(ByVal type As String,
                                      ByVal name As String,
                                      ByVal d As struct_def) As Boolean
            assert(Not d Is Nothing)
            ' Struct member types are always resolved during the define / build stage, so scope.current() equals to
            ' the scope where the struct_t instance is being defined.
            type = normalized_type.parameter_type_of(type).full_type()
            Dim sub_type As struct_def = Nothing
            If Not s.find(type, sub_type) Then
                d.with_primitive(type, name)
                Return True
            End If
            assert(Not sub_type Is Nothing)
            d.with_nested(type, name)
            d.append(sub_type.append_prefix(name))
            Return True
        End Function

        Public Shared Function create_type_id(ByVal name As String) As builders.parameter
            assert(Not name.null_or_whitespace())
            Return builders.parameter.non_ref(name + "__struct__type__id__type", name + "__struct__type__id")
        End Function

        Public Function define(ByVal name As String,
                               ByVal members As vector(Of builders.parameter),
                               ByVal define_type As Action(Of String, UInt32)) As Boolean
            assert(Not name.null_or_whitespace())
            assert(Not members Is Nothing)  ' Allow empty struct
            assert(Not define_type Is Nothing)
            Dim full_type As String = normalized_type.parameter_type_of(name).full_type()
            assert(Not builders.parameter_type.is_ref_type(full_type))
            Dim type_id As builders.parameter = create_type_id(name)
            define_type(type_id.map_type(AddressOf normalized_type.of).non_ref_type(), uint32_1)
            members = members.CloneT().emplace_with(type_id)
            Dim d As New struct_def()
            Dim i As UInt32 = 0
            While i < members.size()
                assert(Not members(i) Is Nothing)
                assert(Not members(i).ref)
                If Not resolve_type(members(i).non_ref_type(), members(i).name, d) Then
                    Return False
                End If
                i += uint32_1
            End While
            If s.emplace(full_type, d).second() Then
                scope(Of T).current().type_alias().remove(full_type)
                Return True
            End If
            raise_error(error_type.user, "Struct type ", full_type, " has been defined already as: ", s(full_type))
            Return False
        End Function

        Public Function resolve(ByVal type As String,
                                ByVal name As String,
                                ByRef o As struct_def) As Boolean
            assert(Not type.null_or_whitespace())
            If Not s.find(normalized_type.parameter_type_of(type).full_type(), o) Then
                ' Do not log, value_declaration and value_definition always check if a struct is defined before
                ' forwarding the defintion directly to the logic.
                Return False
            End If
            ' name can be null to check the availability of a struct definition.
            If Not name Is Nothing Then
                name = fully_qualified_variable_name.of(name)
                assert(Not name.null_or_whitespace())
                o = o.append_prefix(name)
            End If
            Return True
        End Function
    End Class

    Public Structure struct_proxy
        Public Function define(ByVal type As String,
                               ByVal members As vector(Of builders.parameter),
                               ByVal define_type As Action(Of String, UInt32)) As Boolean
            Return scope(Of T).current().myself().structs().define(type, members, define_type)
        End Function

        Public Function resolve(ByVal type As String,
                                ByVal name As String,
                                ByRef o As struct_def) As Boolean
            Dim s As scope(Of WRITER, __BUILDER, __CODE_GENS, T) = scope(Of T).current()
            While Not s Is Nothing
                If s.myself().structs().resolve(type, name, o) Then
                    Return True
                End If
                s = s.parent
            End While
            Return False
        End Function

        Public Structure type_proxy
            Public Function defined(ByVal type As String) As Boolean
                Return scope(Of T).current().structs().resolve(type, Nothing, Nothing)
            End Function
        End Structure

        Public Function types() As type_proxy
            Return New type_proxy()
        End Function

        Public Structure variable_proxy
            Public Function resolve(ByVal name As String, ByRef o As tuple(Of String, struct_def)) As Boolean
                Dim s As T = scope(Of T).current()
                While Not s Is Nothing
                    Dim type As String = Nothing
                    If Not s.myself().variables().resolve(name, type) Then
                        s = s.parent
                        Continue While
                    End If
                    Dim v As struct_def = Nothing
                    ' The type may not be a struct at all. Note, the struct only applies to the variables after it's
                    ' defined.
                    If Not s.structs().resolve(type, name, v) Then
                        Return False
                    End If
                    o = tuple.of(type, v)
                    Return True
                End While
                Return False
            End Function

            Public Function resolve(ByVal name As String, ByRef v As struct_def) As Boolean
                Dim o As tuple(Of String, struct_def) = Nothing
                If Not resolve(name, o) Then
                    Return False
                End If
                v = o.second()
                Return True
            End Function

            Public Function defined(ByVal name As String) As Boolean
                Return resolve(name, [default](Of struct_def).null)
            End Function
        End Structure

        Public Function variables() As variable_proxy
            Return New variable_proxy()
        End Function
    End Structure
End Class
