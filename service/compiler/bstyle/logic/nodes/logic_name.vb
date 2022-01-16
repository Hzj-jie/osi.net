﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class logic_name
        Public Shared Function temp_variable(ByVal n As typed_node) As String
            assert(Not n Is Nothing)
            Return strcat("temp_value_@",
                          code_builder.current().nested_build_level(),
                          "@",
                          n.char_start(),
                          "-",
                          n.char_end())
        End Function

        Public Shared Function of_function(ByVal raw_name As String,
                                           ByVal params As vector(Of builders.parameter)) As String
            assert(Not params Is Nothing)
            Return build(raw_name,
                         params.stream().
                                map(Function(ByVal i As builders.parameter) As String
                                        assert(Not i Is Nothing)
                                        Return i.type
                                    End Function).
                              collect(Of vector(Of String))())
        End Function

        Public Shared Function of_callee(ByVal raw_name As String,
                                         ByVal return_type As String,
                                         ByVal parameters As vector(Of builders.parameter),
                                         ByVal paragraph As Func(Of writer, Boolean),
                                         ByVal o As writer) As Boolean
            Dim name As String = of_function(raw_name, parameters)
            If Not scope.current().functions().define(return_type, name) Then
                Return False
            End If
            scope.current().current_function().define(name, return_type, parameters)
            If Not scope.current().variables().define(
                    parameters.stream().
                               map(AddressOf single_data_slot_variable.from_builders_parameter).
                               collect(Of vector(Of single_data_slot_variable))()) Then
                Return False
            End If
            Return builders.of_callee(name,
                                      If(scope.current().structs().defined(return_type),
                                         compiler.logic.scope.type_t.variable_type,
                                         scope.current().type_alias()(return_type)),
                                      parameters.stream().
                                                 map(AddressOf scope.current().type_alias().canonical_of).
                                                 collect(Of vector(Of builders.parameter))(),
                                      paragraph).to(o)
        End Function

        Public Shared Function of_function_call(ByVal raw_name As String,
                                                ByVal parameters As vector(Of String),
                                                ByRef o As String) As Boolean
            Dim types As New vector(Of String)()
            Dim i As UInt32 = 0
            While i < parameters.size()
                Dim type As String = Nothing
                If Not scope.current().variables().resolve(parameters(i), type) Then
                    Return False
                End If
                types.emplace_back(type)
                i += uint32_1
            End While
            o = build(raw_name, types)
            Return True
        End Function

        Private Shared Function build(ByVal raw_name As String, ByVal types As vector(Of String)) As String
            assert(Not types Is Nothing)
            Dim s As New StringBuilder(raw_name)
            Dim i As UInt32 = 0
            While i < types.size()
                assert(Not types(i).contains_any(space_chars))
                s.Append("&").Append(scope.current().type_alias()(types(i)))
                i += uint32_1
            End While
            Return Convert.ToString(s)
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
