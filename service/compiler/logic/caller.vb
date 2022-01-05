﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class caller
        Implements exportable

        Private ReadOnly anchors As anchors
        Private ReadOnly types As types
        Private ReadOnly name As String
        Private ReadOnly result As [optional](Of String)
        Private ReadOnly parameters() As String

        Public Sub New(ByVal anchors As anchors,
                       ByVal types As types,
                       ByVal name As String,
                       ByVal result As String,
                       ParamArray ByVal parameters() As String)
            assert(Not anchors Is Nothing)
            assert(Not types Is Nothing)
            assert(Not name.null_or_whitespace())
            assert(result Is Nothing OrElse Not result.null_or_whitespace())
            Me.anchors = anchors
            Me.types = types
            Me.name = name
            Me.result = [optional].of_nullable(result)
            Me.parameters = parameters
            For Each parameter As String In Me.parameters
                assert(Not parameter.null_or_whitespace())
            Next
        End Sub

        Public Sub New(ByVal anchors As anchors,
                       ByVal types As types,
                       ByVal name As String,
                       ParamArray ByVal parameters() As String)
            Me.New(anchors, types, name, Nothing, parameters)
        End Sub

        ' Forward return-value to scope_wrapper.
        Private Function define_return_value(ByVal anchor As anchor, ByVal o As vector(Of String)) As Boolean
            assert(Not o Is Nothing)
            If Not return_value.export(types, name, anchor.return_type, o) Then
                Return False
            End If
            Return True
        End Function

        Private Function parameter_place_holder_of(ByVal i As Int32) As String
            assert(i < parameters.array_size_i())
            Return strcat("@parameter_", i, "_of_", name, "_place_holder")
        End Function

        ' Forward parameters to scope_wrapper.
        Private Function define_parameters(ByVal anchor As anchor,
                                           ByVal o As vector(Of String)) As Boolean
            assert(Not anchor Is Nothing)
            assert(Not o Is Nothing)
            If array_size(parameters) <> anchor.parameters.size() Then
                errors.mismatch_callee_parameters(anchor.name, parameters)
                Return False
            End If
            Dim vars(parameters.Length() - 1) As variable
            For i As Int32 = 0 To parameters.Length() - 1
                If Not variable.of(types, parameters(i), o, vars(i)) Then
                    Return False
                End If
            Next
            For i As Int32 = 0 To parameters.Length() - 1
                Dim parameter_place_holder As String = parameter_place_holder_of(i)
                If Not define.export(types,
                                     parameter_place_holder,
                                     anchor.parameters(CUInt(i)).type,
                                     o) Then
                    Return False
                End If

                Dim target As variable = Nothing
                If Not variable.of(types, parameter_place_holder, o, target) Then
                    Return False
                End If

                If anchor.parameters(CUInt(i)).ref Then
                    If Not move.export(target, vars(i), o) Then
                        Return False
                    End If
                Else
                    If Not copy.export(target, vars(i), o) Then
                        Return False
                    End If
                End If
            Next
            Return True
        End Function

        ' Forward return-value from scope_wrapper.
        Private Function forward_to_result(ByVal anchor As anchor, ByVal o As vector(Of String)) As Boolean
            assert(Not anchor Is Nothing)
            assert(Not o Is Nothing)
            ' The count of parameters should be checked already in define_parameter.
            assert(array_size(parameters) = anchor.parameters.size())
            For i As Int32 = 0 To parameters.array_size_i() - 1
                If Not anchor.parameters(CUInt(i)).ref Then
                    Continue For
                End If
                Dim parameter_place_holder As variable = Nothing
                assert(variable.of(types, parameter_place_holder_of(i), o, parameter_place_holder))
                Dim parameter As variable = Nothing
                assert(variable.of(types, parameters(i), o, parameter))
                If Not move.export(parameter, parameter_place_holder, o) Then
                    Return False
                End If
            Next
            If Not result Then
                Return True
            End If
            Dim result_var As variable = Nothing
            If Not variable.of(types, +result, o, result_var) Then
                errors.variable_undefined(+result)
                Return False
            End If
            Dim return_value_var As variable = Nothing
            assert(return_value.retrieve(types, name, o, return_value_var))
            Return move.export(result_var, return_value_var, o)
        End Function

        Public Function export(ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not o Is Nothing)
            ' rel(array_size(parameters)) is for return value.
            Using New scope_wrapper(o)
                Dim anchor As anchor = Nothing
                If Not anchors.of(name, anchor) Then
                    errors.anchor_undefined(name)
                    Return False
                End If
                assert(Not anchor Is Nothing)

                If Not define_return_value(anchor, o) Then
                    Return False
                End If
                If Not define_parameters(anchor, o) Then
                    Return False
                End If
                o.emplace_back(instruction_builder.str(command.stst))
                o.emplace_back(instruction_builder.str(command.jump, data_ref.abs(anchor.begin)))
                If Not forward_to_result(anchor, o) Then
                    Return False
                End If
            End Using

            Return True
        End Function
    End Class
End Namespace
