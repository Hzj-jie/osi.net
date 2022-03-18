
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class logic
    ' VisibleForTesting
    Public NotInheritable Class _caller
        Inherits anchor_caller

        Private ReadOnly name As String

        ' @VisibleForTesting
        Public Sub New(ByVal name As String, ByVal result As String, ParamArray ByVal parameters() As String)
            MyBase.New(command.jump, result, parameters)
            assert(Not name.null_or_whitespace())
            Me.name = name
        End Sub

        Public Sub New(ByVal name As String, ByVal result As String, ByVal parameters As vector(Of String))
            Me.New(name, result, +parameters)
        End Sub

        Public Sub New(ByVal name As String, ByVal parameters As vector(Of String))
            Me.New(name, Nothing, +parameters)
        End Sub

        Protected Overrides Function retrieve_anchor(ByRef anchor As scope.anchor) As Boolean
            If scope.current().anchors().of(name, anchor) Then
                Return True
            End If
            errors.anchor_undefined(name)
            Return False
        End Function
    End Class
End Class
