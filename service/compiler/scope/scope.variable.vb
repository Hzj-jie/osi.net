﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public Class scope(Of T As scope(Of T))
    Protected NotInheritable Class variable_t
        Private ReadOnly s As New unordered_map(Of String, String)()

        Public Function try_define(ByVal type As String, ByVal name As String) As Boolean
            assert(Not type.null_or_whitespace())
            assert(Not name.null_or_whitespace())
            ' Types are always resolved during the define / build stage, so scope(Of T).current() equals to the scope
            ' where the variable_t instance Is being defined.
            type = scope(Of T).current().type_alias()(type)
            assert(Not builders.parameter_type.is_ref_type(type))
            ' The name should not be an array with index.
            assert(Not variable.is_heap_name(name))
            Return s.emplace(name, type).second()
        End Function

        Public Function define(ByVal type As String, ByVal name As String) As Boolean
            If try_define(type, name) Then
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
            ' Types are always resolved during the define / build stage, so scope(Of T).current() equals to the scope
            ' where the variable_t instance Is being defined.
            type = scope(Of T).current().type_alias()(type)
            assert(Not builders.parameter_type.is_ref_type(type))
            ' The name should not be an array with index.
            assert(Not variable.is_heap_name(name))
            If s.find(name) = s.end() Then
                Return False
            End If
            s(name) = type
            Return True
        End Function

        Public Function undefine(ByVal name As String) As Boolean
            assert(Not name.null_or_whitespace())
            ' The name should not be an array with index.
            assert(Not variable.is_heap_name(name))
            Return s.erase(name)
        End Function

        Public Function resolve(ByVal name As String, ByRef type As String) As Boolean
            assert(Not name.null_or_whitespace())
            Return s.find(name, type)
        End Function
    End Class

    Public Structure variable_proxy
        Public Function define(ByVal type As String, ByVal name As String) As Boolean
            Return current_accessor().variables().define(type, name)
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
            Dim s As scope(Of T) = scope(Of T).current()
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
            Dim s As scope(Of T) = scope(Of T).current()
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
                name = name.Substring(0, name.IndexOf(character.left_mid_bracket))
            End If

            Dim s As scope(Of T) = scope(Of T).current()
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
End Class
