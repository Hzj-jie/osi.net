
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Partial Public NotInheritable Class double_stream
    Public NotInheritable Class aggregators
        Public Shared Function average() As Func(Of Double, Double, Double)
            Dim c As UInt32 = 0
            Return Function(ByVal l As Double, ByVal r As Double) As Double
                       c += uint32_1
                       Return (l * c + r) / (c + uint32_1)
                   End Function
        End Function

        ' Likely this function should be moved to stream(Of T).aggregators.
        Public Shared ReadOnly product As Func(Of Double, Double, Double) =
             Function(ByVal l As Double, ByVal r As Double) As Double
                 Return l * r
             End Function

        Private Sub New()
        End Sub
    End Class

    Private Sub New()
    End Sub
End Class
