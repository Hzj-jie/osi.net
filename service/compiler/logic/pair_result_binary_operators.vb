
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class logic
    Private NotInheritable Class _divide
        Inherits pair_result_binary_operator

        Public Sub New(ByVal result1 As String,
                       ByVal result2 As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(result1, result2, left, right)
        End Sub

        Protected Overrides Function parameter_restrict(ByVal parameter As variable) As Boolean
            Return True
        End Function

        Protected Overrides Function result_restrict(ByVal result As variable) As Boolean
            assert(Not result Is Nothing)
            'TODO: Should use result.is_variable_size()
            Return True
        End Function

        Protected Overrides Function instruction() As command
            Return command.div
        End Function
    End Class

    Private NotInheritable Class _extract
        Inherits pair_result_binary_operator

        Public Sub New(ByVal result1 As String,
                       ByVal result2 As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(result1, result2, left, right)
        End Sub

        Protected Overrides Function parameter_restrict(ByVal parameter As variable) As Boolean
            Return True
        End Function

        Protected Overrides Function result_restrict(ByVal result As variable) As Boolean
            assert(Not result Is Nothing)
            'TODO: Should use result.is_variable_size()
            Return True
        End Function

        Protected Overrides Function instruction() As command
            Return command.ext
        End Function
    End Class
End Class
