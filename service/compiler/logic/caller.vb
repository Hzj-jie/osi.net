
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public Class caller
        Implements exportable

        Private ReadOnly anchors As anchors
        Private ReadOnly name As String
        Private ReadOnly result As String
        Private ReadOnly parameters() As String
        Private ReadOnly return_value_place_holder_name As String

        Public Sub New(ByVal anchors As anchors,
                       ByVal name As String,
                       ByVal result As String,
                       ByVal parameters As unique_ptr(Of String()))
            assert(Not anchors Is Nothing)
            assert(Not String.IsNullOrEmpty(name))
            Me.anchors = anchors
            Me.name = name
            Me.result = result
            Me.parameters = parameters.release_or_null()
            Me.return_value_place_holder_name = strcat("@return_value_of_", name, "_place_holder")
        End Sub

        Public Sub New(ByVal anchors As anchors,
                       ByVal name As String,
                       ByVal result As String,
                       ParamArray ByVal parameters() As String)
            Me.New(anchors, name, result, unique_ptr.[New](parameters))
        End Sub

        Public Function export(ByVal scope As scope,
                               ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not scope Is Nothing)
            assert(Not o Is Nothing)
            ' rel(array_size(parameters)) is for return value.
            Using sw As scope_wrapper = New scope_wrapper(scope, o)
                If Not define.export(return_value_place_holder_name, types.variable_type, sw.scope(), o) Then
                    Return False
                End If
                For i As Int32 = 0 To array_size_i(parameters) - 1
                    If Not define.export(strcat("@parameter_", i, "_of_", name, "_place_holder"),
                                     types.variable_type,
                                     sw.scope(),
                                     o) Then
                        Return False
                    End If
                Next
                For i As Int32 = 0 To array_size_i(parameters) - 1
                    Dim var As variable = Nothing
                    If Not variable.[New](sw.scope(), parameters(i), var) Then
                        Return False
                    End If

                    o.emplace_back(instruction_builder.str(command.cp, data_ref.rel(i), var))
                Next
                o.emplace_back(instruction_builder.str(command.stst))

                Dim pos As UInt32 = 0
                If Not anchors.retrieve(name, pos) Then
                    errors.anchor_undefined(name)
                    Return False
                End If
                o.emplace_back(instruction_builder.str(command.jump, data_ref.abs(pos)))
                If Not String.IsNullOrEmpty(result) Then
                    Dim result_var As variable = Nothing
                    If Not variable.[New](sw.scope(), result, result_var) Then
                        Return False
                    End If
                    Dim return_value_var As variable = Nothing
                    assert(variable.[New](sw.scope(), return_value_place_holder_name, return_value_var))
                    o.emplace_back(instruction_builder.str(command.mov, result_var, return_value_var))
                End If
            End Using

            Return True
        End Function
    End Class
End Namespace
