
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public Class callee
        Implements exportable

        Private ReadOnly anchors As anchors
        Private ReadOnly name As String
        Private ReadOnly parameters() As pair(Of String, String)
        Private ReadOnly paragraph As paragraph

        Public Sub New(ByVal anchors As anchors,
                       ByVal name As String,
                       ByVal parameters As unique_ptr(Of pair(Of String, String)()),
                       ByVal paragraph As paragraph)
            assert(Not anchors Is Nothing)
            assert(Not String.IsNullOrEmpty(name))
            Me.anchors = anchors
            Me.name = name
            Me.parameters = parameters.release_or_null()
        End Sub

        Public Sub New(ByVal anchors As anchors,
                       ByVal name As String,
                       ByVal paragraph As paragraph,
                       ByVal ParamArray parameters() As pair(Of String, String))
            Me.New(anchors, name, unique_ptr.[New](parameters), paragraph)
        End Sub

        Public Function export(ByVal scope As scope,
                               ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not scope Is Nothing)
            assert(Not o Is Nothing)
            If Not anchors.define_begin(name, o) Then
                errors.anchor_redefined(name, anchors.begin(name))
                Return False
            End If
            Using sw As scope_wrapper = New scope_wrapper(scope, o)
                If Not return_value_of.define(scope, name) Then
                    Return False
                End If
                If Not scope_define.define(sw.scope(), strcat("@return_ip_of_", name), types.variable_type) Then
                    Return False
                End If
                For i As Int32 = 0 To array_size_i(parameters) - 1
                    If Not sw.scope().define(parameters(i).first, parameters(i).second) Then
                        errors.redefine(parameters(i).first, parameters(i).second, sw.scope().type(parameters(i).first))
                        Return False
                    End If
                Next
                If Not paragraph.export(sw.scope(), o) Then
                    Return False
                End If
                anchors.define_end(name, o)
            End Using
            o.emplace_back(instruction_builder.str(command.esc))
            Return True
        End Function
    End Class
End Namespace
