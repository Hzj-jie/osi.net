
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.interpreter.primitive

Namespace logic
    Public Class divide
        Inherits pair_result_binary_operator

        Public Sub New(ByVal types As types,
                       ByVal result1 As String,
                       ByVal result2 As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(types, result1, result2, left, right)
        End Sub

        Protected Overrides Function parameter_restrict(ByVal parameter As variable) As Boolean
            Return True
        End Function

        Protected Overrides Function result_restrict(ByVal result As variable) As Boolean
            assert(Not result Is Nothing)
            Return result.is_variable_size()
        End Function

        Protected Overrides Function instruction() As command
            Return command.div
        End Function
    End Class

    Public Class extract
        Inherits pair_result_binary_operator

        Public Sub New(ByVal types As types,
                       ByVal result1 As String,
                       ByVal result2 As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(types, result1, result2, left, right)
        End Sub

        Protected Overrides Function parameter_restrict(ByVal parameter As variable) As Boolean
            Return True
        End Function

        Protected Overrides Function result_restrict(ByVal result As variable) As Boolean
            assert(Not result Is Nothing)
            Return result.is_variable_size()
        End Function

        Protected Overrides Function instruction() As command
            Return command.ext
        End Function
    End Class
End Namespace
