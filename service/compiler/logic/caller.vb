
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public Class caller
        Implements exportable

        Private ReadOnly anchors As anchors
        Private ReadOnly name As String
        Private ReadOnly parameters() As String

        Public Sub New(ByVal anchors As anchors, ByVal name As String, ByVal parameters As unique_ptr(Of String()))
            assert(Not anchors Is Nothing)
            assert(Not String.IsNullOrEmpty(name))
            Me.anchors = anchors
            Me.name = name
            Me.parameters = parameters.release_or_null()
        End Sub

        Public Sub New(ByVal anchors As anchors, ByVal name As String, ParamArray ByVal parameters() As String)
            Me.New(anchors, name, unique_ptr.[New](parameters))
        End Sub

        Public Function export(ByVal scope As scope,
                               ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not scope Is Nothing)
            assert(Not o Is Nothing)
            ' rel(array_size(parameters)) is the return address of "esc".
            ' rel(array_size(parameters) + 1) is for return value.
            For i As Int32 = 0 To 2 + array_size_i(parameters) - 1
                o.emplace_back(instruction_builder.str(command.push))
            Next
            If Not return_value_of.define(scope, name) Then
                Return False
            End If
            For i As Int32 = 0 To CInt(array_size(parameters)) - 1
                Dim var As variable = Nothing
                If Not variable.[New](scope, parameters(i), var) Then
                    Return False
                End If

                o.emplace_back(instruction_builder.str(command.mov, data_ref.rel(i), var.ref))
            Next
            o.emplace_back(instruction_builder.str(command.cpnip, data_ref.rel(array_size(parameters))))

            Dim pos As UInt32 = 0
            If Not anchors.retrieve_begin(name, pos) Then
                errors.anchor_undefined(name)
                Return False
            End If
            o.emplace_back(instruction_builder.str(command.jump, data_ref.abs(pos)))
            o.emplace_back(instruction_builder.str(command.pop))

            Return True
        End Function
    End Class
End Namespace
