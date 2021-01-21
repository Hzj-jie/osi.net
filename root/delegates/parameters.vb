
Option Explicit On
Option Infer Off
Option Strict On

'placeholder
Public Class parameters
    Public Shared ReadOnly null As parameters

    Shared Sub New()
        null = New parameters()
    End Sub

    Private Sub New()
    End Sub
End Class

Public Class parameters(Of T)
    Public ReadOnly v As T

    Public Sub New(ByVal v As T)
        Me.v = v
    End Sub
End Class

Public Class parameters(Of T1, T2)
    Public ReadOnly v1 As T1
    Public ReadOnly v2 As T2

    Public Sub New(ByVal v1 As T1, ByVal v2 As T2)
        Me.v1 = v1
        Me.v2 = v2
    End Sub
End Class

Public Class parameters(Of T1, T2, T3)
    Public ReadOnly v1 As T1
    Public ReadOnly v2 As T2
    Public ReadOnly v3 As T3

    Public Sub New(ByVal v1 As T1, ByVal v2 As T2, ByVal v3 As T3)
        Me.v1 = v1
        Me.v2 = v2
        Me.v3 = v3
    End Sub
End Class
