
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    Public NotInheritable Class callee_ref
        Implements exportable

        Private ReadOnly name As String
        Private ReadOnly type As String
        Private ReadOnly parameters() As builders.parameter

        Public Sub New(ByVal name As String,
                       ByVal type As String,
                       ByVal ParamArray parameters As pair(Of String, String)())
            assert(Not String.IsNullOrEmpty(name))
            assert(Not String.IsNullOrEmpty(type))
            Me.name = name
            Me.type = type
            Me.parameters = builders.parameter.from_logic_callee_input(parameters)
        End Sub

        Public Sub New(ByVal name As String,
                       ByVal type As String,
                       ByVal parameters As vector(Of pair(Of String, String)))
            Me.New(name, type, +parameters)
        End Sub

        Public Function export(ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(False)
        End Function
    End Class
End Namespace
