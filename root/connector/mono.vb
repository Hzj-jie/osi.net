
Option Explicit On
Option Infer Off
Option Strict On

Public Module _mono
    Private ReadOnly _on_mono As Boolean

    Sub New()
        _on_mono = (Not Type.GetType("Mono.Runtime") Is Nothing)
    End Sub

    Public Function on_mono() As Boolean
        Return _on_mono
    End Function
End Module
