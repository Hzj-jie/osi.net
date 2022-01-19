
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    Public NotInheritable Class _callee_ref
        Implements exportable

        Private ReadOnly name As String
        Private ReadOnly return_type As String
        Private ReadOnly parameters() As builders.parameter_type

        Public Sub New(ByVal name As String,
                       ByVal return_type As String,
                       ByVal ParamArray parameters As String())
            assert(Not String.IsNullOrEmpty(name))
            assert(Not String.IsNullOrEmpty(return_type))
            Me.name = name
            Me.return_type = return_type
            Me.parameters = builders.parameter_type.from(parameters)
        End Sub

        Public Sub New(ByVal name As String,
                       ByVal return_type As String,
                       ByVal parameters As vector(Of String))
            Me.New(name, return_type, +parameters)
        End Sub

        Public Function export(ByVal o As vector(Of String)) As Boolean Implements exportable.export
            Return scope.current().anchor_refs().decl(name, return_type, parameters)
        End Function
    End Class
End Namespace
