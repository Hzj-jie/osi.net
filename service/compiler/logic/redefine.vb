
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class logic
    Public NotInheritable Class _redefine
        Inherits stack_var_operator

        Private ReadOnly type As String

        Public Sub New(ByVal name As String, ByVal type As String)
            MyBase.New(name)
            assert(Not type.null_or_whitespace())
            Me.type = type
        End Sub

        Protected Overrides Function process(ByVal ptr As variable, ByVal o As vector(Of String)) As Boolean
            assert(Not ptr Is Nothing)
            assert(scope.current().variables().redefine(ptr.name, type))
            Return True
        End Function
    End Class
End Class
