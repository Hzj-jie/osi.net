
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.template

Public NotInheritable Class template_array(Of T, R, F As typed_func(Of R))
    Public Shared ReadOnly v As R = alloc(Of F)().at(Of T)()
    Public Shared ReadOnly vs() As R = {v}

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class template_array(Of T1, T2, R, F As typed_func(Of R))
    Public Shared ReadOnly vs() As R = Function() As R()
                                           Dim x As F = alloc(Of F)()
                                           Return {x.at(Of T1)(), x.at(Of T2)()}
                                       End Function()

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class template_array(Of T1, T2, T3, R, F As typed_func(Of R))
    Public Shared ReadOnly vs() As R = Function() As R()
                                           Dim x As F = alloc(Of F)()
                                           Return {x.at(Of T1)(), x.at(Of T2)(), x.at(Of T3)()}
                                       End Function()

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class template_array(Of T1, T2, T3, T4, R, F As typed_func(Of R))
    Public Shared ReadOnly vs() As R = Function() As R()
                                           Dim x As F = alloc(Of F)()
                                           Return {x.at(Of T1)(), x.at(Of T2)(), x.at(Of T3)(), x.at(Of T4)()}
                                       End Function()

    Private Sub New()
    End Sub
End Class
