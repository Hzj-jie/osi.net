
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class logic
    Private NotInheritable Class _callee_ref
        Implements instruction_gen

        Private ReadOnly name As String
        Private ReadOnly return_type As String
        Private ReadOnly parameters() As builders.parameter_type

        Public Sub New(ByVal name As String,
                       ByVal return_type As String,
                       ByVal ParamArray parameters As String())
            assert(Not name.null_or_empty())
            assert(Not return_type.null_or_empty())
            Me.name = name
            Me.return_type = return_type
            Me.parameters = builders.parameter_type.from(parameters)
        End Sub

        Public Sub New(ByVal name As String,
                       ByVal return_type As String,
                       ByVal parameters As vector(Of String))
            Me.New(name, return_type, +parameters)
        End Sub

        Private Function build(ByVal o As vector(Of String)) As Boolean Implements instruction_gen.build
            Return scope.current().anchor_refs().decl(name, return_type, parameters)
        End Function
    End Class
End Class
