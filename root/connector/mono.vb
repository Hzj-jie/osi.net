
Option Explicit On
Option Infer Off
Option Strict On

Public Module _mono
    Private ReadOnly _on_mono As Boolean = (Not Type.GetType("Mono.Runtime") Is Nothing)

    Public Function on_mono() As Boolean
        Return _on_mono
    End Function
End Module
