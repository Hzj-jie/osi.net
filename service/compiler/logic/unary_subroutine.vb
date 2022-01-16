
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Namespace logic
    Public MustInherit Class unary_subroutine
        Inherits data_ref_operator

        Public Sub New(ByVal parameter As String)
            MyBase.New(parameter)
        End Sub

        Protected MustOverride Function parameter_restrict(ByVal parameter As variable) As Boolean

        Protected NotOverridable Overrides Function variable_restrict(ByVal i As UInt32, ByVal v As variable) As Boolean
            Select Case i
                Case 0
                    Return parameter_restrict(v)
                Case Else
                    assert(False)
                    Return False
            End Select
        End Function
    End Class
End Namespace
