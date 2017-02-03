
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Namespace logic
    ' Result = ? Parameter
    Public MustInherit Class unary_operator
        Inherits data_ref_operator

        Public Sub New(ByVal types As types, ByVal result As String, ByVal parameter As String)
            MyBase.New(types, result, parameter)
        End Sub

        Protected MustOverride Function result_restrict(ByVal result As variable) As Boolean
        Protected MustOverride Function parameter_restrict(ByVal parameter As variable) As Boolean

        Protected Overrides Function variable_restrict(ByVal i As UInt32, ByVal v As variable) As Boolean
            Select Case i
                Case 0
                    Return result_restrict(v)
                Case 1
                    Return parameter_restrict(v)
                Case Else
                    assert(False)
                    Return False
            End Select
        End Function
    End Class
End Namespace
