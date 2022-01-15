
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class less
        Inherits compare

        Public Sub New(ByVal result As String, ByVal left As String, ByVal right As String)
            MyBase.New(result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.less
        End Function
    End Class

    Public NotInheritable Class more
        Inherits compare

        Public Sub New(ByVal result As String, ByVal left As String, ByVal right As String)
            MyBase.New(result, right, left)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.less
        End Function
    End Class

    Public NotInheritable Class equal
        Inherits compare

        Public Sub New(ByVal result As String, ByVal left As String, ByVal right As String)
            MyBase.New(result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.equal
        End Function
    End Class

    Public NotInheritable Class less_or_equal
        Inherits compare_or_equal

        Public Sub New(ByVal result As String, ByVal left As String, ByVal right As String)
            MyBase.New(result, left, right)
        End Sub

        Protected Overrides Function compare() As command
            Return command.less
        End Function
    End Class

    Public NotInheritable Class more_or_equal
        Inherits compare_or_equal

        Public Sub New(ByVal result As String, ByVal left As String, ByVal right As String)
            MyBase.New(result, right, left)
        End Sub

        Protected Overrides Function compare() As command
            Return command.less
        End Function
    End Class

    Public NotInheritable Class float_less
        Inherits compare

        Public Sub New(ByVal result As String, ByVal left As String, ByVal right As String)
            MyBase.New(result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.fless
        End Function
    End Class

    Public NotInheritable Class float_more
        Inherits compare

        Public Sub New(ByVal result As String, ByVal left As String, ByVal right As String)
            MyBase.New(result, right, left)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.fless
        End Function
    End Class

    Public NotInheritable Class float_equal
        Inherits compare

        Public Sub New(ByVal result As String, ByVal left As String, ByVal right As String)
            MyBase.New(result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.fequal
        End Function
    End Class

    Public NotInheritable Class float_less_or_equal
        Inherits compare_or_equal

        Public Sub New(ByVal result As String, ByVal left As String, ByVal right As String)
            MyBase.New(result, left, right)
        End Sub

        Protected Overrides Function compare() As command
            Return command.fless
        End Function
    End Class

    Public NotInheritable Class float_more_or_equal
        Inherits compare_or_equal

        Public Sub New(ByVal result As String, ByVal left As String, ByVal right As String)
            MyBase.New(result, right, left)
        End Sub

        Protected Overrides Function compare() As command
            Return command.fless
        End Function
    End Class
End Namespace
