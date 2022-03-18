
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class logic
    Public NotInheritable Class _undefine
        Inherits stack_var_operator

        Public Sub New(ByVal name As String)
            MyBase.New(name)
        End Sub

        Protected Overrides Function process(ByVal ptr As variable, ByVal o As vector(Of String)) As Boolean
            assert(Not ptr Is Nothing)
            assert(scope.current().variables().undefine(ptr.name))
            Return True
        End Function
    End Class
End Class
