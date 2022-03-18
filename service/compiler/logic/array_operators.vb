
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class logic
    Public NotInheritable Class _append_slice
        Inherits unary_operator

        Public Sub New(ByVal result As String, ByVal parameter As String)
            MyBase.New(result, parameter)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.sapp
        End Function

        Protected Overrides Function parameter_restrict(ByVal parameter As variable) As Boolean
            Return True
        End Function

        Protected Overrides Function result_restrict(ByVal result As variable) As Boolean
            assert(Not result Is Nothing)
            ' TODO: Should be Return result.is_variable_size()?
            Return True
        End Function
    End Class

    Public NotInheritable Class _cut_slice
        Inherits binary_operator

        Public Sub New(ByVal result As String, ByVal left As String, ByVal right As String)
            MyBase.New(result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.scut
        End Function

        Protected Overrides Function left_restrict(ByVal left As variable) As Boolean
            assert(Not left Is Nothing)
            ' TODO: Should be Return left.is_variable_size()?
            Return True
        End Function

        Protected Overrides Function right_restrict(ByVal right As variable) As Boolean
            assert(Not right Is Nothing)
            Return right.is_assignable_to_uint32()
        End Function

        Protected Overrides Function result_restrict(ByVal result As variable) As Boolean
            assert(Not result Is Nothing)
            ' TODO: Should be Return result.is_variable_size()?
            Return True
        End Function
    End Class

    Public NotInheritable Class _cut
        Inherits binary_operator

        Public Sub New(ByVal result As String, ByVal left As String, ByVal right As String)
            MyBase.New(result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.cut
        End Function

        Protected Overrides Function left_restrict(ByVal left As variable) As Boolean
            assert(Not left Is Nothing)
            ' TODO: Should be Return left.is_variable_size()?
            Return True
        End Function

        Protected Overrides Function right_restrict(ByVal right As variable) As Boolean
            assert(Not right Is Nothing)
            Return right.is_assignable_to_uint32()
        End Function

        Protected Overrides Function result_restrict(ByVal result As variable) As Boolean
            assert(Not result Is Nothing)
            ' TODO: Should be Return result.is_variable_size()?
            Return True
        End Function
    End Class

    Public NotInheritable Class _cut_len
        Inherits ternary_operator

        Public Sub New(ByVal result As String,
                       ByVal p1 As String,
                       ByVal p2 As String,
                       ByVal p3 As String)
            MyBase.New(result, p1, p2, p3)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.cutl
        End Function

        Protected Overrides Function result_restrict(ByVal result As variable) As Boolean
            assert(Not result Is Nothing)
            ' TODO: Should be Return result.is_variable_size()?
            Return True
        End Function

        Protected Overrides Function parameter1_restrict(ByVal p1 As variable) As Boolean
            Return True
        End Function

        Protected Overrides Function parameter2_restrict(ByVal p2 As variable) As Boolean
            assert(Not p2 Is Nothing)
            Return p2.is_assignable_to_uint32()
        End Function

        Protected Overrides Function parameter3_restrict(ByVal p3 As variable) As Boolean
            assert(Not p3 Is Nothing)
            Return p3.is_assignable_to_uint32()
        End Function
    End Class

    Public NotInheritable Class _clear
        Inherits unary_subroutine

        Public Sub New(ByVal parameter As String)
            MyBase.New(parameter)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.clr
        End Function

        Protected Overrides Function parameter_restrict(ByVal parameter As variable) As Boolean
            assert(Not parameter Is Nothing)
            Return parameter.is_variable_size()
        End Function
    End Class
End Class
