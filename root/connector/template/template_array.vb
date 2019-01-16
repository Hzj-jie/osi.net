
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.template

Public NotInheritable Class template_array(Of T, R, F As typed_func(Of R))
    Public Shared ReadOnly v As R
    Public Shared ReadOnly vs() As R

    Shared Sub New()
        Dim x As F = Nothing
        x = alloc(Of F)()
        v = x.at(Of T)()
        vs = {v}
    End Sub

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class template_array(Of T1, T2, R, F As typed_func(Of R))
    Public Shared ReadOnly vs() As R

    Shared Sub New()
        Dim x As F = Nothing
        x = alloc(Of F)()
        vs = {x.at(Of T1)(), x.at(Of T2)()}
    End Sub

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class template_array(Of T1, T2, T3, R, F As typed_func(Of R))
    Public Shared ReadOnly vs() As R

    Shared Sub New()
        Dim x As F = Nothing
        x = alloc(Of F)()
        vs = {x.at(Of T1)(), x.at(Of T2)(), x.at(Of T3)()}
    End Sub

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class template_array(Of T1, T2, T3, T4, R, F As typed_func(Of R))
    Public Shared ReadOnly vs() As R

    Shared Sub New()
        Dim x As F = Nothing
        x = alloc(Of F)()
        vs = {x.at(Of T1)(), x.at(Of T2)(), x.at(Of T3)(), x.at(Of T4)()}
    End Sub

    Private Sub New()
    End Sub
End Class
