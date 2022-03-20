
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class logic
    ' Result = p1 ? p2 ? p3
    Public MustInherit Class ternary_operator
        Inherits data_ref_operator

        Public Sub New(ByVal result As String,
                       ByVal p1 As String,
                       ByVal p2 As String,
                       ByVal p3 As String)
            MyBase.New(result, p1, p2, p3)
        End Sub

        Protected MustOverride Function result_restrict(ByVal result As variable) As Boolean

        Protected Overridable Function parameter_restrict(ByVal p As variable) As Boolean
            assert(False)
            Return False
        End Function

        Protected Overridable Function parameter1_restrict(ByVal p1 As variable) As Boolean
            Return parameter_restrict(p1)
        End Function

        Protected Overridable Function parameter2_restrict(ByVal p2 As variable) As Boolean
            Return parameter_restrict(p2)
        End Function

        Protected Overridable Function parameter3_restrict(ByVal p3 As variable) As Boolean
            Return parameter_restrict(p3)
        End Function

        Protected Overrides Function variable_restrict(ByVal i As UInt32, ByVal v As variable) As Boolean
            Select Case i
                Case 0
                    Return result_restrict(v)
                Case 1
                    Return parameter1_restrict(v)
                Case 2
                    Return parameter2_restrict(v)
                Case 3
                    Return parameter3_restrict(v)
                Case Else
                    assert(False)
                    Return False
            End Select
        End Function
    End Class
End Class
