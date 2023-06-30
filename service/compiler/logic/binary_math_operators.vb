
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class logic
    ' VisibleForTesting
    Public NotInheritable Class _add
        Inherits binary_math_operator

        Public Sub New(ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.add
        End Function

        Public Overloads Shared Function export(ByVal result As String,
                                                ByVal left As String,
                                                ByVal right As String,
                                                ByVal o As vector(Of String)) As Boolean
            ' binary_math_operator does not require size info of either parameters or result.
            Return New _add(result, left, right).build(o)
        End Function
    End Class

    Private NotInheritable Class _subtract
        Inherits binary_math_operator

        Public Sub New(ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.sub
        End Function
    End Class

    Private NotInheritable Class _multiply
        Inherits binary_math_operator

        Public Sub New(ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.mul
        End Function
    End Class

    Private NotInheritable Class _power
        Inherits binary_math_operator

        Public Sub New(ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.pow
        End Function
    End Class

    Private NotInheritable Class _and
        Inherits binary_math_operator

        Public Sub New(ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.and
        End Function
    End Class

    Private NotInheritable Class _or
        Inherits binary_math_operator

        Public Sub New(ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.or
        End Function
    End Class

    Private NotInheritable Class _float_add
        Inherits binary_math_operator

        Public Sub New(ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.fadd
        End Function
    End Class

    Private NotInheritable Class _float_subtract
        Inherits binary_math_operator

        Public Sub New(ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.fsub
        End Function
    End Class

    Private NotInheritable Class _float_multiply
        Inherits binary_math_operator

        Public Sub New(ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.fmul
        End Function
    End Class

    Private NotInheritable Class _float_power
        Inherits binary_math_operator

        Public Sub New(ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.fpow
        End Function
    End Class

    Private NotInheritable Class _float_divide
        Inherits binary_math_operator

        Public Sub New(ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.fdiv
        End Function
    End Class

    Private NotInheritable Class _float_extract
        Inherits binary_math_operator

        Public Sub New(ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.fext
        End Function
    End Class

    Private NotInheritable Class _left_shift
        Inherits binary_math_operator

        Public Sub New(ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.lfs
        End Function
    End Class

    Private NotInheritable Class _right_shift
        Inherits binary_math_operator

        Public Sub New(ByVal result As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(result, left, right)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.rfs
        End Function
    End Class
End Class
