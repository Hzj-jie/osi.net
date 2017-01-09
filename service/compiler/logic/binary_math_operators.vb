
Imports osi.service.interpreter.primitive

Namespace logic
    Public Class add
        Inherits binary_math_operator

        Public Sub New(ByVal types As types, ByVal result As String, ByVal left As String, ByVal right As String)
            MyBase.New(types, result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.add
        End Function
    End Class

    Public Class subtract
        Inherits binary_math_operator

        Public Sub New(ByVal types As types, ByVal result As String, ByVal left As String, ByVal right As String)
            MyBase.New(types, result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.sub
        End Function
    End Class

    Public Class power
        Inherits binary_math_operator

        Public Sub New(ByVal types As types, ByVal result As String, ByVal left As String, ByVal right As String)
            MyBase.New(types, result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.pow
        End Function
    End Class

    Public Class [and]
        Inherits binary_math_operator

        Public Sub New(ByVal types As types, ByVal result As String, ByVal left As String, ByVal right As String)
            MyBase.New(types, result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.and
        End Function
    End Class

    Public Class [or]
        Inherits binary_math_operator

        Public Sub New(ByVal types As types, ByVal result As String, ByVal left As String, ByVal right As String)
            MyBase.New(types, result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.or
        End Function
    End Class
End Namespace
