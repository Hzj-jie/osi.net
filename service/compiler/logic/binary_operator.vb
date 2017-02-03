
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Namespace logic
    ' Result = Left ? Right
    Public MustInherit Class binary_operator
        Inherits data_ref_operator

        Public Sub New(ByVal types As types, ByVal result As String, ByVal left As String, ByVal right As String)
            MyBase.New(types, result, left, right)
        End Sub

        Protected MustOverride Function result_restrict(ByVal result As variable) As Boolean

        Protected Overridable Function left_restrict(ByVal left As variable) As Boolean
            Return parameter_restrict(left)
        End Function

        Protected Overridable Function right_restrict(ByVal right As variable) As Boolean
            Return parameter_restrict(right)
        End Function

        Protected Overridable Function parameter_restrict(ByVal parameter As variable) As Boolean
            assert(False)
            Return False
        End Function

        Protected NotOverridable Overrides Function variable_restrict(ByVal i As UInt32, ByVal v As variable) As Boolean
            Select Case i
                Case 0
                    Return result_restrict(v)
                Case 1
                    Return left_restrict(v)
                Case 2
                    Return right_restrict(v)
                Case Else
                    assert(False)
                    Return False
            End Select
        End Function
    End Class
End Namespace
