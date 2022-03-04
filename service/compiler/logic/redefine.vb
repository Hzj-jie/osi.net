
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    Public NotInheritable Class _redefine
        Implements instruction_gen

        Private ReadOnly name As String
        Private ReadOnly type As String

        Public Sub New(ByVal name As String, ByVal type As String)
            assert(Not name.null_or_whitespace())
            assert(Not type.null_or_whitespace())
            Me.name = name
            Me.type = type
        End Sub

        Public Function build(ByVal o As vector(Of String)) As Boolean Implements instruction_gen.build
            Return scope.current().variables().redefine_stack(name, type)
        End Function
    End Class
End Namespace
