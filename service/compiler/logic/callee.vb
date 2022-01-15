
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
        Private ReadOnly types As types
        Private ReadOnly name As String
        Private ReadOnly type As String
        Private ReadOnly parameters() As builders.parameter
        Private ReadOnly paragraph As paragraph

        Public Sub New(ByVal anchors As anchors,
                       ByVal types As types,
                       ByVal name As String,
                       ByVal type As String,
                       ByVal parameters As unique_ptr(Of pair(Of String, String)()),
                       ByVal paragraph As unique_ptr(Of paragraph))
            assert(Not anchors Is Nothing)
            assert(Not types Is Nothing)
            assert(Not String.IsNullOrEmpty(name))
            Me.anchors = anchors
            Me.types = types
            Me.name = name
            Me.type = type
            Me.parameters = builders.parameter.from_logic_callee_input(parameters.release_or_null())
            Me.paragraph = paragraph.release_or_null()
        End Sub

        ' VisibleForTesting
        Public Sub New(ByVal anchors As anchors,
                       ByVal types As types,
                       ByVal name As String,
                       ByVal type As String,
                       ByVal paragraph As unique_ptr(Of paragraph),
                       ByVal ParamArray parameters() As pair(Of String, String))
            Me.New(anchors, types, name, type, unique_ptr.[New](parameters), paragraph)
        End Sub

        Public Function export(ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not o Is Nothing)
            Dim pos As UInt32 = o.size()
            o.emplace_back("")
            If Not anchors.define(name, o, type, parameters) Then
                Return False
            End If
            ' No need to use scope_wrapper, as the pops are after the rest instruction and have no effect.
            ' Meanwhile return-value and parameters are defined within the scope, but are not pushed, so they should not
            ' be popped.
            scope.current().start_scope()
            Using defer.to(AddressOf scope.current().end_scope)
                ' caller should setup the stack.
                ' Note, variables are using reverse order to match the stack, and here the logic "define" without "push"ing
                ' to use variables from the caller side.
                If Not return_value.define(name, type) Then
                    Return False
                End If
                For i As Int32 = 0 To array_size_i(parameters) - 1
                    If Not scope.current().variables().define_stack(parameters(i).name, parameters(i).type) Then
                        Return False
                    End If
                Next
                If Not paragraph Is Nothing AndAlso Not paragraph.export(o) Then
                    Return False
                End If
                o.emplace_back(instruction_builder.str(command.rest))
                o(pos) = instruction_builder.str(command.jump, data_ref.rel(o.size() - pos))
            End Using
            Return True
        End Function
    End Class
End Namespace
