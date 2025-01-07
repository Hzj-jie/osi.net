
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports builders = osi.service.compiler.logic.builders
Imports typed_node = osi.service.automata.typed_node
Imports variable = osi.service.compiler.logic.variable

Partial Public Class scope(Of WRITER As {lazy_list_writer, New},
                              __BUILDER As func_t(Of String, WRITER, Boolean),
                              __CODE_GENS As func_t(Of code_gens(Of WRITER)),
                              T As scope(Of WRITER, __BUILDER, __CODE_GENS, T))
    Protected NotInheritable Class variable_t
        ' name -> type
        Private ReadOnly s As New unordered_map(Of String, String)()

        Private Function define(ByVal type As String,
                                ByVal name As String,
                                ByVal insert As Func(Of String, String, Boolean)) As Boolean
            assert(Not type.null_or_whitespace())
            assert(Not insert Is Nothing)
            ' TODO: May consider using builders.parameter.
            ' Types are always resolved during the define / build stage, so scope(Of T).current() equals to the scope
            ' where the variable_t instance Is being defined.
            type = normalized_type.parameter_type_of(type).full_type()
            assert(Not type.null_or_whitespace())
            name = fully_qualified_variable_name.of(name)
            assert(Not name.null_or_whitespace())
            assert(Not builders.parameter_type.is_ref_type(type))
            ' The name should not be an array with index.
            assert(Not variable.is_heap_name(name))
            Return insert(name, type)
        End Function

        Public Function define(ByVal type As String, ByVal name As String) As Boolean
            If define(type, name, Function(ByVal n As String, ByVal t As String) As Boolean
                                      Return s.emplace(n, t).second()
                                  End Function) Then
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
            Return define(type, name, Function(ByVal n As String, ByVal t As String) As Boolean
                                          If s.find(n) = s.end() Then
                                              Return False
                                          End If
                                          s(n) = t
                                          Return True
                                      End Function)
        End Function

        ' When finding the variable in a function, there are two candidates, 1) the variable defined in the function
        ' itself, 2) the variable in the same namespace / root scope. Prefer the one in the function itself, or in
        ' another word, the raw name itself.
        Private Shared Function find(Of RT)(ByVal name As String,
                                            ByVal f As _do_val_ref(Of String, RT, Boolean),
                                            ByRef o As RT) As Boolean
            assert(Not name.null_or_whitespace())
            assert(Not f Is Nothing)
            Dim fn As String = current_namespace_t.of(name)
            assert(Not fn.null_or_whitespace())
            If f(name, o) Then
                Return True
            End If
            If fn.Equals(name) Then
                Return False
            End If
            Return f(fn, o)
        End Function

        Public Function undefine(ByVal name As String) As Boolean
            Return find(name,
                        Function(ByVal n As String, ByRef o As Int32) As Boolean
                            ' The name should not be an array with index.
                            assert(Not variable.is_heap_name(n))
                            Return s.erase(n)
                        End Function,
                        0)
        End Function

        Public Function resolve(ByVal name As String, ByRef type As String) As Boolean
            Return find(name,
                        Function(ByVal n As String, ByRef o As String) As Boolean
                            Return s.find(n, o)
                        End Function,
                        type)
        End Function
    End Class

    Public Structure variable_proxy
        Public Function define(ByVal type As String, ByVal name As String) As Boolean
            Return scope(Of T).current().myself().variables().define(type, name)
        End Function

        Public Function define(ByVal n As typed_node) As Boolean
            assert(Not n Is Nothing)
            assert(n.child_count() >= 2)
            Return define(type_name.of(n.child(0)), variable_name.of(n.child(1)))
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
            Dim s As scope(Of WRITER, __BUILDER, __CODE_GENS, T) = scope(Of T).current()
            While Not s Is Nothing
                If s.myself().variables().redefine(type, name) Then
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

        Public Function undefine(ByVal name As String) As Boolean
            If variable.is_heap_name(name) Then
                raise_error(error_type.user, "Undefine works for heap name without index, but got ", name)
                Return False
            End If
            Dim s As scope(Of WRITER, __BUILDER, __CODE_GENS, T) = scope(Of T).current()
            While Not s Is Nothing
                If s.myself().variables().undefine(name) Then
                    Return True
                End If
                s = s.parent
            End While
            raise_error(error_type.user, "Variable ", name, " has not been defined yet.")
            Return False
        End Function

        Public Function define(ByVal vs As vector(Of builders.parameter)) As Boolean
            assert(Not vs Is Nothing)
            Dim i As UInt32 = 0
            While i < vs.size()
                assert(Not vs(i) Is Nothing)
                If Not define(vs(i).unrefed_type(), vs(i).name) Then
                    Return False
                End If
                i += uint32_1
            End While
            ' If vs is empty, always return true.
            Return True
        End Function

        Public Function defined(ByVal name As String) As Boolean
            Return try_resolve(name, Nothing, Nothing)
        End Function

        Private Function try_resolve(ByVal name As String,
                                     ByRef type As String,
                                     ByVal signature As ref(Of function_signature)) As Boolean
            ' logic_name.of_function_call requires type of the parameter to set function name.
            If variable.is_heap_name(name) Then
                name = name.Substring(0, name.IndexOf(character.left_mid_bracket))
            End If

            Dim s As scope(Of WRITER, __BUILDER, __CODE_GENS, T) = scope(Of T).current()
            While Not s Is Nothing
                If Not s.myself().variables().resolve(name, type) Then
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

        Public Function type_of(ByVal name As String, ByRef type As String) As Boolean
            Return resolve(name, type, Nothing)
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

        Public Shared Function define() As Func(Of typed_node, Boolean)
            Return Function(ByVal n As typed_node) As Boolean
                       Return current().variables().define(n)
                   End Function
        End Function
    End Structure
End Class
