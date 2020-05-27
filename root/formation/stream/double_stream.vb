﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Partial Public NotInheritable Class double_stream
    Public NotInheritable Class aggregators
        Public Shared Function average() As Func(Of Double, Double, Double)
            Dim c As UInt32 = 0
            Return Function(ByVal l As Double, ByVal r As Double) As Double
                       Return (l * (c + uint32_1) + r) / (c + uint32_2)
                   End Function
        End Function
    End Class

    Private Sub New()
    End Sub
End Class
