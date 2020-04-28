
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class add
        Inherits binary_math_operator

        Public Sub New(ByVal types As types,
                       ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(types, result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.add
        End Function
    End Class

    Public NotInheritable Class subtract
        Inherits binary_math_operator

        Public Sub New(ByVal types As types,
                       ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(types, result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.sub
        End Function
    End Class

    Public NotInheritable Class multiply
        Inherits binary_math_operator

        Public Sub New(ByVal types As types,
                       ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(types, result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.mul
        End Function
    End Class

    Public NotInheritable Class power
        Inherits binary_math_operator

        Public Sub New(ByVal types As types,
                       ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(types, result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.pow
        End Function
    End Class

    Public NotInheritable Class [and]
        Inherits binary_math_operator

        Public Sub New(ByVal types As types,
                       ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(types, result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.and
        End Function
    End Class

    Public NotInheritable Class [or]
        Inherits binary_math_operator

        Public Sub New(ByVal types As types,
                       ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(types, result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.or
        End Function
    End Class

    Public NotInheritable Class float_add
        Inherits binary_math_operator

        Public Sub New(ByVal types As types,
                       ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(types, result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.fadd
        End Function
    End Class

    Public NotInheritable Class float_subtract
        Inherits binary_math_operator

        Public Sub New(ByVal types As types,
                       ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(types, result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.fsub
        End Function
    End Class

    Public NotInheritable Class float_multiply
        Inherits binary_math_operator

        Public Sub New(ByVal types As types,
                       ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(types, result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.fmul
        End Function
    End Class

    Public NotInheritable Class float_power
        Inherits binary_math_operator

        Public Sub New(ByVal types As types,
                       ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(types, result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.fpow
        End Function
    End Class

    Public NotInheritable Class float_divide
        Inherits binary_math_operator

        Public Sub New(ByVal types As types,
                       ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(types, result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.fdiv
        End Function
    End Class

    Public NotInheritable Class float_extract
        Inherits binary_math_operator

        Public Sub New(ByVal types As types,
                       ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(types, result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.fext
        End Function
    End Class

    Public NotInheritable Class left_shift
        Inherits binary_math_operator

        Public Sub New(ByVal types As types,
                       ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(types, result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.lfs
        End Function
    End Class

    Public NotInheritable Class right_shift
        Inherits binary_math_operator

        Public Sub New(ByVal types As types,
                       ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(types, result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.rfs
        End Function
    End Class
End Namespace
