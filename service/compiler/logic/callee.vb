
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class callee
        Implements exportable

        Private ReadOnly anchors As anchors
        Private ReadOnly name As String
        Private ReadOnly type As String
        Private ReadOnly parameters() As pair(Of String, String)
        Private ReadOnly paragraph As paragraph

        Public Sub New(ByVal anchors As anchors,
                       ByVal name As String,
                       ByVal type As String,
                       ByVal parameters As unique_ptr(Of pair(Of String, String)()),
                       ByVal paragraph As unique_ptr(Of paragraph))
            assert(Not anchors Is Nothing)
            assert(Not String.IsNullOrEmpty(name))
            Me.anchors = anchors
            Me.name = name
            Me.type = type
            Me.parameters = parameters.release_or_null()
            Me.paragraph = paragraph.release_or_null()
        End Sub

        Public Sub New(ByVal anchors As anchors,
                       ByVal name As String,
                       ByVal type As String,
                       ByVal paragraph As unique_ptr(Of paragraph),
                       ByVal ParamArray parameters() As pair(Of String, String))
            Me.New(anchors, name, type, unique_ptr.[New](parameters), paragraph)
        End Sub

        Public Function export(ByVal scope As scope,
                               ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not scope Is Nothing)
            assert(Not o Is Nothing)
            Dim pos As UInt32 = 0
            pos = o.size()
            o.emplace_back(String.Empty)
            If Not anchors.define(name, o, type) Then
                errors.anchor_redefined(name, anchors(name))
                Return False
            End If
            ' No need to use scope_wrapper, as the pops are after the rest instruction and have no effect.
            scope = scope.start_scope()
            ' caller should setup the stack.
            If Not return_value_of.define(scope, name, type) Then
                Return False
            End If
            For i As Int32 = 0 To array_size_i(parameters) - 1
                If Not scope.define(parameters(i).first, parameters(i).second) Then
                    Return False
                End If
            Next
            If Not paragraph Is Nothing AndAlso Not paragraph.export(scope, o) Then
                Return False
            End If
            o.emplace_back(instruction_builder.str(command.rest))
            o(pos) = instruction_builder.str(command.jump, data_ref.rel(o.size() - pos))
            scope.end_scope()
            Return True
        End Function
    End Class
End Namespace
