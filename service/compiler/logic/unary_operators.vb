
Imports osi.root.connector
Imports osi.service.interpreter.primitive

Namespace logic
    Public Class append
        Inherits unary_operator

        Public Sub New(ByVal types As types, ByVal result As String, ByVal parameter As String)
            MyBase.New(types, result, parameter)
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

    Public Class [not]
        Inherits unary_operator

        Public Sub New(ByVal types As types, ByVal result As String, ByVal parameter As String)
            MyBase.New(types, result, parameter)
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

    Public Class sizeof
        Inherits unary_operator

        Public Sub New(ByVal types As types, ByVal result As String, ByVal parameter As String)
            MyBase.New(types, result, parameter)
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

    Public Class empty
        Inherits unary_operator

        Public Sub New(ByVal types As types, ByVal result As String, ByVal parameter As String)
            MyBase.New(types, result, parameter)
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
End Namespace
