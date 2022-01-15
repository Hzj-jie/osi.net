
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class _caller_ref
        Inherits anchor_caller
        Implements exportable

        Private ReadOnly name As String

        ' @VisibleForTesting
        Public Sub New(ByVal name As String,
                       ByVal result As String,
                       ParamArray ByVal parameters() As String)
            MyBase.New(command.jump, result, parameters)
            ' TODO: jmpr should be used.
            assert(Not name.null_or_whitespace())
            Me.name = name
        End Sub

        Public Sub New(ByVal name As String,
                       ByVal result As String,
                       ByVal parameters As vector(Of String))
            Me.New(name, result, +parameters)
        End Sub

        Public Sub New(ByVal name As String, ByVal parameters As vector(Of String))
            Me.New(name, Nothing, +parameters)
        End Sub

        Protected Overrides Function retrieve_anchor(ByVal o As vector(Of String),
                                                     ByRef anchor As scope.anchor) As Boolean
            Dim d As scope.variable_t.exported_ref = Nothing
            If Not scope.current().variables().export(name, d) Then
                errors.anchor_ref_undefined(name)
                Return False
            End If
            assert(Not d Is Nothing)
            anchor = scope.current().anchor_refs().of(name).with_begin(d.data_ref)
            Return True
        End Function
    End Class
End Namespace
