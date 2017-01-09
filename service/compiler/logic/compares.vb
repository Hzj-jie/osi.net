
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public Class less
        Inherits compare

        Public Sub New(ByVal types As types, ByVal left As String, ByVal right As String, ByVal result As String)
            MyBase.New(types, left, right, result)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.less
        End Function
    End Class

    Public Class more
        Inherits compare

        Public Sub New(ByVal types As types, ByVal left As String, ByVal right As String, ByVal result As String)
            MyBase.New(types, right, left, result)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.less
        End Function
    End Class

    Public Class less_or_equal
        Inherits compare

        Public Sub New(ByVal types As types, ByVal left As String, ByVal right As String, ByVal result As String)
            MyBase.New(types, left, right, result)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.leeq
        End Function
    End Class

    Public Class more_or_equal
        Inherits compare

        Public Sub New(ByVal types As types, ByVal left As String, ByVal right As String, ByVal result As String)
            MyBase.New(types, right, left, result)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.leeq
        End Function
    End Class

    Public Class equal
        Inherits compare

        Public Sub New(ByVal types As types, ByVal left As String, ByVal right As String, ByVal result As String)
            MyBase.New(types, left, right, result)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.equal
        End Function
    End Class
End Namespace
