
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public Class [return]
        Implements exportable

        Private ReadOnly anchors As anchors
        Private ReadOnly name As String
        Private ReadOnly return_value As String

        Public Sub New(ByVal anchors As anchors, ByVal name As String, Optional ByVal return_value As String = Nothing)
            assert(Not anchors Is Nothing)
            assert(Not String.IsNullOrEmpty(name))
            Me.anchors = anchors
            Me.name = name
            Me.return_value = return_value
        End Sub

        Public Function export(ByVal scope As scope,
                               ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not o Is Nothing)
            If Not String.IsNullOrEmpty(return_value) Then
                Dim var As variable = Nothing
                If Not variable.[New](scope, return_value, var) Then
                    Return False
                End If
                Dim r As variable = Nothing
                If Not return_value_of.retrieve(scope, name, r) Then
                    Return False
                End If
                o.emplace_back(instruction_builder.str(command.mov, r, var))
            End If
            o.emplace_back(instruction_builder.str(command.rest))
            Return True
        End Function
    End Class
End Namespace
