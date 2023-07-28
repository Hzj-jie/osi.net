
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class logic
    ' VisibleForTesting
    Public NotInheritable Class _callee
        Implements instruction_gen

        Private ReadOnly name As String
        Private ReadOnly type As String
        Private ReadOnly parameters() As builders.parameter
        Private ReadOnly paragraph As paragraph

        Public Sub New(ByVal name As String,
                       ByVal type As String,
                       ByVal parameters As vector(Of pair(Of String, String)),
                       ByVal paragraph As paragraph)
            Me.New(name, type, paragraph, +parameters)
        End Sub

        ' VisibleForTesting
        Public Sub New(ByVal name As String,
                       ByVal type As String,
                       ByVal paragraph As paragraph,
                       ByVal ParamArray parameters() As pair(Of String, String))
            assert(Not name.null_or_empty())
            assert(Not type.null_or_empty())
            assert(Not paragraph Is Nothing)
            Me.name = name
            Me.type = type
            Me.parameters = builders.parameter.from(parameters)
            Me.paragraph = paragraph
        End Sub

        Private Function build(ByVal o As vector(Of String)) As Boolean Implements instruction_gen.build
            assert(Not o Is Nothing)
            Dim pos As UInt32 = o.size()
            o.emplace_back("")
            If Not scope.current().anchors().define(name, o, type, parameters) Then
                Return False
            End If
            Using scope.current().start_scope()
                ' caller should setup the stack.
                ' Note, variables are using reverse order to match the stack, and here the logic "define" without
                ' "push"ing to use variables from the caller side.
                If Not return_value.define(name, type) Then
                    Return False
                End If
                For i As Int32 = 0 To array_size_i(parameters) - 1
                    If Not _define.forward(parameters(i).name, parameters(i).unrefed_type(), o) Then
                        Return False
                    End If
                Next
                If Not paragraph.build(o) Then
                    Return False
                End If
                o.emplace_back(instruction_builder.str(command.rest))
                o(pos) = instruction_builder.str(command.jump, data_ref.rel(o.size() - pos))
            End Using
            Return True
        End Function
    End Class
End Class
