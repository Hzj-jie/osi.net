
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
        Private ReadOnly parameters() As builders.parameter

        Public Sub New(ByVal name As String,
                       ByVal return_type As String,
                       ByVal ParamArray parameters As pair(Of String, String)())
            assert(Not String.IsNullOrEmpty(name))
            assert(Not String.IsNullOrEmpty(return_type))
            Me.name = name
            Me.return_type = return_type
            Me.parameters = builders.parameter.from_logic_callee_input(parameters)
        End Sub

        Public Sub New(ByVal name As String,
                       ByVal return_type As String,
                       ByVal parameters As vector(Of pair(Of String, String)))
            Me.New(name, return_type, +parameters)
        End Sub

        Public Function export(ByVal o As vector(Of String)) As Boolean Implements exportable.export
            If Not _define.export(name, scope.type_t.ptr_type, o) Then
                Return False
            End If
            scope.current().anchor_refs().define(name, return_type, parameters)
            Return True
        End Function
    End Class
End Namespace
