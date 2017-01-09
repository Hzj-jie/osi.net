
Imports osi.root.connector
Imports osi.service.interpreter.primitive

Namespace logic
    Public Class append_slice
        Inherits unary_operator

        Public Sub New(ByVal types As types, ByVal result As String, ByVal parameter As String)
            MyBase.New(types, result, parameter)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.sapp
        End Function

        Protected Overrides Function parameter_restrict(ByVal parameter As variable) As Boolean
            Return True
        End Function

        Protected Overrides Function result_restrict(ByVal result As variable) As Boolean
            assert(Not result Is Nothing)
            Return result.is_variable_size()
        End Function
    End Class

    Public Class cut
        Inherits binary_operator

        Public Sub New(ByVal types As types, ByVal result As String, ByVal left As String, ByVal right As String)
            MyBase.New(types, result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.cut
        End Function

        Protected Overrides Function left_restrict(ByVal left As variable) As Boolean
            assert(Not left Is Nothing)
            Return left.is_variable_size()
        End Function

        Protected Overrides Function right_restrict(ByVal right As variable) As Boolean
            assert(Not right Is Nothing)
            Return right.is_assignable_to_uint32()
        End Function

        Protected Overrides Function result_restrict(ByVal result As variable) As Boolean
            assert(Not result Is Nothing)
            Return result.is_variable_size()
        End Function
    End Class

    Public Class cut_slice

    End Class
End Namespace
