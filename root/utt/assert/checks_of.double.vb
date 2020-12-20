
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.template

Partial Public Class checks(Of IS_TRUE_FUNC As __void(Of Boolean, Object()))
    Public Shared Function [of](ByVal i As Double) As double_subject
        Return New double_subject(i)
    End Function

    Public Class double_subject
        Inherits T_subject(Of Double)

        Private Const default_diff As Double = 0.000001

        Public Sub New(ByVal i As Double)
            MyBase.New(i)
        End Sub

        Public Function in_range(ByVal base As Double, ByVal diff As Double) As Boolean
            Return between(base - diff, base + diff)
        End Function

        Public Function in_range(ByVal base As Double) As Boolean
            Return in_range(base, default_diff)
        End Function

        Public Function in_close_range(ByVal base As Double, ByVal diff As Double) As Boolean
            Return between_include(base - diff, base + diff)
        End Function

        Public Function in_close_range(ByVal base As Double) As Boolean
            Return in_close_range(base, default_diff)
        End Function
    End Class
End Class
