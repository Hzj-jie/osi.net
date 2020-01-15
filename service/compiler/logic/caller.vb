
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
        Private ReadOnly return_value_place_holder_name As String

        Public Sub New(ByVal anchors As anchors,
                       ByVal types As types,
                       ByVal name As String,
                       ByVal result As String,
                       ByVal parameters As unique_ptr(Of String()))
            assert(Not anchors Is Nothing)
            assert(Not types Is Nothing)
            assert(Not name.null_or_whitespace())
            assert(result Is Nothing OrElse Not result.null_or_whitespace())
            Me.anchors = anchors
            Me.types = types
            Me.name = name
            Me.result = [optional].of_nullable(result)
            Me.parameters = parameters.release_or_null()
            For Each parameter As String In Me.parameters
                assert(Not parameter.null_or_whitespace())
            Next
            Me.return_value_place_holder_name = strcat("@return_value_of_", name, "_place_holder")
        End Sub

        Public Sub New(ByVal anchors As anchors,
                       ByVal types As types,
                       ByVal name As String,
                       ByVal parameters As unique_ptr(Of String()))
            Me.New(anchors, types, name, Nothing, parameters)
        End Sub

        Public Sub New(ByVal anchors As anchors,
                       ByVal types As types,
                       ByVal name As String,
                       ByVal result As String,
                       ParamArray ByVal parameters() As String)
            Me.New(anchors, types, name, result, unique_ptr.[New](parameters))
        End Sub

        Public Sub New(ByVal anchors As anchors,
                       ByVal types As types,
                       ByVal name As String,
                       ParamArray ByVal parameters() As String)
            Me.New(anchors, types, name, Nothing, unique_ptr.[New](parameters))
        End Sub

        ' Forward return-value to scope_wrapper.
        Private Function define_return_value(ByVal sw As scope_wrapper, ByVal o As vector(Of String)) As Boolean
            assert(Not sw Is Nothing)
            assert(Not o Is Nothing)
            Dim result_type As String = Nothing
            If result Then
                Dim var As variable = Nothing
                If Not variable.[New](sw.scope(), types, +result, var) Then
                    Return False
                End If
                result_type = var.type
            Else
                result_type = types.variable_type
            End If
            If Not define.export(anchors, return_value_place_holder_name, result_type, sw.scope(), o) Then
                Return False
            End If
            Return True
        End Function

        ' Forward parameters to scope_wrapper.
        Private Function define_parameters(ByVal sw As scope_wrapper, ByVal o As vector(Of String)) As Boolean
            assert(Not sw Is Nothing)
            assert(Not o Is Nothing)
            Dim callee_params As const_array(Of String) = Nothing
            If Not anchors.parameter_types_of(name, callee_params) Then
                errors.anchor_undefined(name)
                Return False
            End If
            If array_size(parameters) <> callee_params.size() Then
                errors.mismatch_callee_parameters(name, parameters)
                Return False
            End If
            For i As Int32 = 0 To array_size_i(parameters) - 1
                Dim parameter_name_place_holder As String = Nothing
                parameter_name_place_holder = strcat("@parameter_", i, "_of_", name, "_place_holder")
                If Not define.export(anchors,
                                     parameter_name_place_holder,
                                     callee_params(CUInt(i)),
                                     sw.scope(),
                                     o) Then
                    Return False
                End If

                Dim target As variable = Nothing
                If Not variable.[New](sw.scope(), types, parameter_name_place_holder, target) Then
                    Return False
                End If

                Dim var As variable = Nothing
                If Not variable.[New](sw.scope(), types, parameters(i), var) Then
                    Return False
                End If

                Dim ins As String = Nothing
                If Not target.copy_from(var, ins) Then
                    Return False
                End If
                o.emplace_back(ins)
            Next
            Return True
        End Function

        ' Forward return-value from scope_wrapper.
        Private Function forward_to_result(ByVal sw As scope_wrapper,
                                           ByVal o As vector(Of String)) As Boolean
            assert(Not sw Is Nothing)
            assert(Not o Is Nothing)
            If Not result Then
                Return True
            End If
            Dim result_var As variable = Nothing
            assert(variable.[New](sw.scope(), types, +result, result_var))
            Dim return_value_var As variable = Nothing
            assert(variable.[New](sw.scope(), types, return_value_place_holder_name, return_value_var))
            Dim s As String = Nothing
            If Not result_var.move_from(return_value_var, s) Then
                Return False
            End If
            o.emplace_back(s)
            Return True
        End Function

        Public Function export(ByVal scope As scope,
                               ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not scope Is Nothing)
            assert(Not o Is Nothing)
            ' rel(array_size(parameters)) is for return value.
            Using sw As scope_wrapper = New scope_wrapper(scope, o)
                If Not define_return_value(sw, o) Then
                    Return False
                End If
                If Not define_parameters(sw, o) Then
                    Return False
                End If
                o.emplace_back(instruction_builder.str(command.stst))

                Dim pos As UInt32 = 0
                If Not anchors.retrieve(name, pos) Then
                    errors.anchor_undefined(name)
                    Return False
                End If
                o.emplace_back(instruction_builder.str(command.jump, data_ref.abs(pos)))
                If Not forward_to_result(sw, o) Then
                    Return False
                End If
            End Using

            Return True
        End Function
    End Class
End Namespace
