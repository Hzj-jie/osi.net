
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class logic
    Public MustInherit Class compare
        Inherits binary_operator

        Public Sub New(ByVal result As String, ByVal left As String, ByVal right As String)
            MyBase.New(result, left, right)
        End Sub

        Protected Overrides Function result_restrict(ByVal result As variable) As Boolean
            assert(Not result Is Nothing)
            Return result.is_assignable_from_bool()
        End Function

        Protected Overrides Function parameter_restrict(ByVal parameter As variable) As Boolean
            Return True
        End Function
    End Class

    Public MustInherit Class compare_or_equal
        Implements instruction_gen

        Private ReadOnly result As String
        Private ReadOnly left As String
        Private ReadOnly right As String

        Public Sub New(ByVal result As String, ByVal left As String, ByVal right As String)
            assert(Not result.null_or_empty())
            assert(Not left.null_or_empty())
            assert(Not right.null_or_empty())
            Me.result = result
            Me.left = left
            Me.right = right
        End Sub

        Protected MustOverride Function compare() As command

        Private Function build(ByVal o As vector(Of String)) As Boolean Implements instruction_gen.build
            Dim result_var As variable = Nothing
            If Not variable.of(result, o, result_var) OrElse
               Not result_var.is_assignable_from_bool() Then
                Return False
            End If
            Dim left_var As variable = Nothing
            If Not variable.of(left, o, left_var) Then
                Return False
            End If
            Dim right_var As variable = Nothing
            If Not variable.of(right, o, right_var) Then
                Return False
            End If
            o.emplace_back(instruction_builder.str(compare(), result_var, left_var, right_var))
            o.emplace_back(instruction_builder.str(command.jumpif, data_ref.rel(2), result_var))
            o.emplace_back(instruction_builder.str(command.equal, result_var, left_var, right_var))
            Return True
        End Function
    End Class
End Class
