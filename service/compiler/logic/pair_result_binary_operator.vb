
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Namespace logic
    ' Result1, Result2 = Left ? Right
    Public MustInherit Class pair_result_binary_operator
        Inherits data_ref_operator

        Public Sub New(ByVal types As types,
                       ByVal result1 As String,
                       ByVal result2 As String,
                       ByVal left As String,
                       ByVal right As String)
            MyBase.New(types, result1, result2, left, right)
        End Sub

        Protected Overridable Function result1_restrict(ByVal result1 As variable) As Boolean
            Return result_restrict(result1)
        End Function

        Protected Overridable Function result2_restrict(ByVal result2 As variable) As Boolean
            Return result_restrict(result2)
        End Function

        Protected Overridable Function result_restrict(ByVal result As variable) As Boolean
            assert(False)
            Return False
        End Function

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

        Protected Overrides Function variable_restrict(ByVal i As UInt32, ByVal v As variable) As Boolean
            Select Case i
                Case 0
                    Return result1_restrict(v)
                Case 1
                    Return result2_restrict(v)
                Case 2
                    Return left_restrict(v)
                Case 3
                    Return right_restrict(v)
                Case Else
                    assert(False)
                    Return False
            End Select
        End Function
    End Class
End Namespace
