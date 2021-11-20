
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public MustInherit Class compare
        Inherits binary_operator

        Public Sub New(ByVal types As types, ByVal result As String, ByVal left As String, ByVal right As String)
            MyBase.New(types, result, left, right)
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
        Implements exportable

        Private ReadOnly types As types
        Private ReadOnly result As String
        Private ReadOnly left As String
        Private ReadOnly right As String

        Public Sub New(ByVal types As types, ByVal result As String, ByVal left As String, ByVal right As String)
            assert(Not types Is Nothing)
            assert(Not String.IsNullOrEmpty(result))
            assert(Not String.IsNullOrEmpty(left))
            assert(Not String.IsNullOrEmpty(right))
            Me.types = types
            Me.result = result
            Me.left = left
            Me.right = right
        End Sub

        Protected MustOverride Function compare() As command

        Public Function export(ByVal o As vector(Of String)) As Boolean Implements exportable.export
            Dim result_var As variable = Nothing
            If Not variable.of(types, result, result_var) OrElse
               Not result_var.is_assignable_from_bool() Then
                Return False
            End If
            Dim left_var As variable = Nothing
            If Not variable.of(types, left, left_var) Then
                Return False
            End If
            Dim right_var As variable = Nothing
            If Not variable.of(types, right, right_var) Then
                Return False
            End If
            o.emplace_back(instruction_builder.str(compare(), result_var, left_var, right_var))
            o.emplace_back(instruction_builder.str(command.jumpif, data_ref.rel(2), result_var))
            o.emplace_back(instruction_builder.str(command.equal, result_var, left_var, right_var))
            Return True
        End Function
    End Class
End Namespace
