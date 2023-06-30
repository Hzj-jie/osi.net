
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class logic
    Private NotInheritable Class _append
        Inherits unary_operator

        Public Sub New(ByVal result As String, ByVal parameter As String)
            MyBase.New(result, parameter)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.app
        End Function

        Protected Overrides Function parameter_restrict(ByVal parameter As variable) As Boolean
            Return True
        End Function

        Protected Overrides Function result_restrict(ByVal result As variable) As Boolean
            assert(Not result Is Nothing)
            Return result.is_variable_size()
        End Function
    End Class

    Private NotInheritable Class _not
        Inherits unary_operator

        Public Sub New(ByVal result As String, ByVal parameter As String)
            MyBase.New(result, parameter)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.not
        End Function

        Protected Overrides Function parameter_restrict(ByVal parameter As variable) As Boolean
            Return True
        End Function

        Protected Overrides Function result_restrict(ByVal result As variable) As Boolean
            assert(Not result Is Nothing)
            Return result.is_assignable_from_bool()
        End Function
    End Class

    ' VisibleForTesting
    Public NotInheritable Class _sizeof
        Inherits unary_operator

        Public Sub New(ByVal result As String, ByVal parameter As String)
            MyBase.New(result, parameter)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.sizeof
        End Function

        Protected Overrides Function parameter_restrict(ByVal parameter As variable) As Boolean
            Return True
        End Function

        Protected Overrides Function result_restrict(ByVal result As variable) As Boolean
            assert(Not result Is Nothing)
            Return result.is_assignable_from_uint32()
        End Function
    End Class

    Private NotInheritable Class _empty
        Inherits unary_operator

        Public Sub New(ByVal result As String, ByVal parameter As String)
            MyBase.New(result, parameter)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.empty
        End Function

        Protected Overrides Function parameter_restrict(ByVal parameter As variable) As Boolean
            Return True
        End Function

        Protected Overrides Function result_restrict(ByVal result As variable) As Boolean
            assert(Not result Is Nothing)
            Return result.is_assignable_from_bool()
        End Function
    End Class
End Class
