
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class scope
        Private NotInheritable Class variable_t
            Private ReadOnly s As New unordered_map(Of String, String)()

            Public Function define(ByVal type As String, ByVal name As String) As Boolean
                assert(Not type.null_or_whitespace())
                assert(Not name.null_or_whitespace())
                ' Types are always resolved during the define / build stage, so scope.current() equals to the scope
                ' where the variable_t instance Is being defined.
                type = scope.current().type_alias()(type)
                ' The name should not be an array with index.
                assert(Not variable.is_heap_name(name))
                If s.emplace(name, type).second() Then
                    Return True
                End If
                raise_error(error_type.user,
                            "Variable ",
                            name,
                            " (new type ",
                            type,
                            ") has been defined already as ",
                            s(name))
                Return False
            End Function

            Public Function redefine(ByVal type As String, ByVal name As String) As Boolean
                assert(Not type.null_or_whitespace())
                assert(Not name.null_or_whitespace())
                ' Types are always resolved during the define / build stage, so scope.current() equals to the scope
                ' where the variable_t instance Is being defined.
                type = scope.current().type_alias()(type)
                ' The name should not be an array with index.
                assert(Not variable.is_heap_name(name))
                If s.find(name) = s.end() Then
                    Return False
                End If
                s(name) = type
                Return True
            End Function

            Public Function resolve(ByVal name As String, ByRef type As String) As Boolean
                assert(Not name.null_or_whitespace())
                Return s.find(name, type)
            End Function
        End Class

        Public Structure variable_proxy
            Public ReadOnly s As scope

            Public Sub New(ByVal s As scope)
                assert(Not s Is Nothing)
                Me.s = s
            End Sub

            Private Shared Function heap_name_of(ByVal name As String) As String
                assert(Not name.null_or_whitespace())
                Return strcat(name, "@")
            End Function

            Public Function define(ByVal type As String, ByVal name As String) As Boolean
                Return s.v.define(type, name)
            End Function

            Public Function redefine(ByVal type As String, ByVal name As String) As Boolean
                If variable.is_heap_name(name) Then
                    raise_error(error_type.user,
                                "Redefine works for heap name without index, but got ",
                                name,
                                " (new type ",
                                type,
                                ")")
                    Return False
                End If
                Dim s As scope = Me.s
                While Not s Is Nothing
                    If s.v.redefine(type, name) OrElse s.v.redefine(type, heap_name_of(name)) Then
                        Return True
                    End If
                    s = s.parent
                End While
                raise_error(error_type.user,
                            "Variable ",
                            name,
                            " (new type ",
                            type,
                            ") has not been defined yet.")
                Return False
            End Function

            Public Function define_heap(ByVal type As String, ByVal name As String) As Boolean
                Return define(compiler.logic.scope.type_t.ptr_type, name) AndAlso
                       define(type, heap_name_of(name))
            End Function

            Public Function define(ByVal vs As vector(Of single_data_slot_variable)) As Boolean
                assert(Not vs Is Nothing)
                Dim i As UInt32 = 0
                While i < vs.size()
                    assert(Not vs(i) Is Nothing)
                    If Not define(vs(i).type, vs(i).name) Then
                        Return False
                    End If
                    i += uint32_1
                End While
                ' If vs is empty, always return true.
                Return True
            End Function

            Public Function try_resolve(ByVal name As String, ByRef type As String) As Boolean
                Return try_resolve(name, type, Nothing)
            End Function

            Private Function try_resolve(ByVal name As String,
                                         ByRef type As String,
                                         ByVal signature As ref(Of function_signature)) As Boolean
                ' logic_name.of_function_call requires type of the parameter to set function name.
                If variable.is_heap_name(name) Then
                    name = heap_name_of(name.Substring(0, name.IndexOf(character.left_mid_bracket)))
                End If

                Dim s As scope = Me.s
                While Not s Is Nothing
                    If Not s.v.resolve(name, type) Then
                        s = s.parent
                        Continue While
                    End If
                    If Not signature Is Nothing Then
                        Dim f As function_signature = Nothing
                        If s.delegates().retrieve(type, f) Then
                            assert(Not f Is Nothing)
                            signature.set(f)
                        End If
                    End If
                    Return True
                End While
                Return False
            End Function

            Public Function resolve(ByVal name As String, ByRef type As String) As Boolean
                Return resolve(name, type, Nothing)
            End Function

            Public Function resolve(ByVal name As String,
                                    ByRef type As String,
                                    ByVal signature As ref(Of function_signature)) As Boolean
                If try_resolve(name, type, signature) Then
                    Return True
                End If
                raise_error(error_type.user, "Variable ", name, " has not been defined.")
                Return False
            End Function
        End Structure

        Public Function variables() As variable_proxy
            Return New variable_proxy(Me)
        End Function
    End Class
End Class
