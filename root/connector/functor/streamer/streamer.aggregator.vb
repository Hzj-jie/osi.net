
Option Explicit On
Option Infer Off
Option Strict On

Partial Public Class streamer(Of T)
    Public NotInheritable Class aggregators
        Public Shared ReadOnly sum As Func(Of T, T, T) = Function(ByVal l As T, ByVal r As T) As T
                                                             Return binary_operator.add(l, r)
                                                         End Function

        Private Sub New()
        End Sub
    End Class
End Class
