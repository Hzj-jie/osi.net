
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class logic
    Public MustInherit Class binary_math_operator
        Inherits binary_operator

        Public Sub New(ByVal result As String, ByVal left As String, ByVal right As String)
            MyBase.New(result, left, right)
        End Sub

        Protected Overrides Function parameter_restrict(ByVal parameter As variable) As Boolean
            Return True
        End Function

        Protected Overrides Function result_restrict(ByVal result As variable) As Boolean
            assert(Not result Is Nothing)
            'TODO: Should use result.is_variable_size()
            Return True
        End Function
    End Class
End Class
