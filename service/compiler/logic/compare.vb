
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Namespace logic
    Public MustInherit Class compare
        Inherits binary_operator

        Public Sub New(ByVal types As types, ByVal result As String, ByVal left As String, ByVal right As String)
            MyBase.New(types, result, left, right)
        End Sub

        Protected Overrides Function result_restrict(ByVal result As variable) As Boolean
            assert(Not result Is Nothing)
            Return result.is_assignable_from_bool()
        End Function

        Protected Overrides Function parameter_restrict(ByVal parameter As variable) As Boolean
            Return True
        End Function
    End Class
End Namespace
