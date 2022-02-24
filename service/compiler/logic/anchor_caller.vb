
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public MustInherit Class anchor_caller
        Implements instruction_gen

        Private ReadOnly cmd As command
        Private ReadOnly result As [optional](Of String)
        Private ReadOnly parameters() As String

        Protected Sub New(ByVal cmd As command, ByVal result As String, ByVal parameters() As String)
            assert(result Is Nothing OrElse Not result.null_or_whitespace())
            assert(Not parameters Is Nothing)
            Me.cmd = cmd
            Me.result = [optional].of_nullable(result)
            Me.parameters = parameters
            For Each parameter As String In Me.parameters
                assert(Not parameter.null_or_whitespace())
            Next
        End Sub

        Protected Sub New(ByVal cmd As command, ByVal parameters() As String)
            Me.New(cmd, Nothing, parameters)
        End Sub

        Protected MustOverride Function retrieve_anchor(ByRef a As scope.anchor) As Boolean

        ' Forward return-value to scope_wrapper.
        Private Shared Function define_return_value(ByVal anchor As scope.anchor,
                                                    ByVal o As vector(Of String)) As Boolean
            assert(Not anchor Is Nothing)
            If Not return_value.export(anchor.name, anchor.return_type, o) Then
                Return False
            End If
            Return True
        End Function

        Private Function parameter_place_holder_of(ByVal anchor As scope.anchor, ByVal i As Int32) As String
            assert(Not anchor Is Nothing)
            assert(i < parameters.array_size_i())
            Return strcat("@parameter_", i, "_of_", anchor.name, "_place_holder")
        End Function

        ' Forward parameters to scope_wrapper.
        Private Function define_parameters(ByVal anchor As scope.anchor,
                                           ByVal o As vector(Of String)) As Boolean
            assert(Not anchor Is Nothing)
            assert(Not o Is Nothing)
            If array_size(parameters) <> anchor.parameters.size() Then
                errors.mismatch_callee_parameters(anchor.name, parameters)
                Return False
            End If
            Dim vars(parameters.Length() - 1) As variable
            For i As Int32 = 0 To parameters.Length() - 1
                If Not variable.of(parameters(i), o, vars(i)) Then
                    Return False
                End If
            Next
            For i As Int32 = 0 To parameters.Length() - 1
                Dim parameter_place_holder As String = parameter_place_holder_of(anchor, i)
                If Not _define.export(parameter_place_holder, anchor.parameters(CUInt(i)).type, o) Then
                    Return False
                End If

                Dim target As variable = Nothing
                If Not variable.of(parameter_place_holder, o, target) Then
                    Return False
                End If

                If anchor.parameters(CUInt(i)).ref Then
                    If Not _move.export(target, vars(i), o) Then
                        Return False
                    End If
                Else
                    If Not _copy.export(target, vars(i), o) Then
                        Return False
                    End If
                End If
            Next
            Return True
        End Function

        ' Forward return-value from scope
        Private Function forward_to_result(ByVal anchor As scope.anchor, ByVal o As vector(Of String)) As Boolean
            assert(Not anchor Is Nothing)
            assert(Not o Is Nothing)
            ' The count of parameters should be checked already in define_parameter.
            assert(array_size(parameters) = anchor.parameters.size())
            For i As Int32 = 0 To parameters.array_size_i() - 1
                If Not anchor.parameters(CUInt(i)).ref Then
                    Continue For
                End If
                Dim parameter_place_holder As variable = Nothing
                assert(variable.of(parameter_place_holder_of(anchor, i), o, parameter_place_holder))
                Dim parameter As variable = Nothing
                assert(variable.of(parameters(i), o, parameter))
                If Not _move.export(parameter, parameter_place_holder, o) Then
                    Return False
                End If
            Next
            If Not result Then
                Return True
            End If
            Dim result_var As variable = Nothing
            If Not variable.of(+result, o, result_var) Then
                errors.variable_undefined(+result)
                Return False
            End If
            Dim return_value_var As variable = Nothing
            assert(return_value.retrieve(anchor.name, o, return_value_var))
            Return _move.export(result_var, return_value_var, o)
        End Function

        Public Function build(ByVal o As vector(Of String)) As Boolean Implements instruction_gen.build
            assert(Not o Is Nothing)
            ' rel(array_size(parameters)) is for return value.
            Using scope.current().start_scope()
                Dim anchor As scope.anchor = Nothing
                If Not retrieve_anchor(anchor) Then
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
                o.emplace_back(instruction_builder.str(cmd, +anchor.begin))
                If Not forward_to_result(anchor, o) Then
                    Return False
                End If
            End Using

            Return True
        End Function
    End Class
End Namespace
