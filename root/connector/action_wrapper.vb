
Imports osi.root.delegates

Public Class action_adapter
    Implements iaction

    Private ReadOnly v As Action

    Public Sub New(ByVal v As Action)
        Me.v = v
    End Sub

    Public Sub New()
        Me.New(Nothing)
    End Sub

    Public Sub run() Implements iaction.run
        assert(v IsNot Nothing)
        v()
    End Sub

    Public Function valid() As Boolean Implements iaction.valid
        Return v IsNot Nothing
    End Function
End Class

Public Class delegate_wrapper(Of AT As idelegate, PARA_T)
    Implements idelegate

    Protected ReadOnly a As AT
    Protected ReadOnly p As iparameter_action(Of PARA_T)

    Public Sub New(ByVal a As AT)
        assert(a IsNot Nothing)
        Me.a = a
    End Sub

    Public Sub New(ByVal p As iparameter_action(Of PARA_T))
        assert(p IsNot Nothing)
        Me.p = p
    End Sub

    Public Function valid() As Boolean Implements idelegate.valid
        Return If(a Is Nothing, p.valid(), a.valid())
    End Function
End Class

Public Class action_wrapper
    Inherits delegate_wrapper(Of iaction, parameters)
    Implements iaction, iparameter_action(Of parameters)

    Public Sub New(ByVal a As iaction)
        MyBase.New(a)
    End Sub

    Public Sub New(ByVal p As iparameter_action(Of parameters))
        MyBase.New(p)
    End Sub

    Public Sub run() Implements iaction.run
        If a Is Nothing Then
            p.run(parameters.null)
        Else
            a.run()
        End If
    End Sub

    Public Sub run(ByVal i As parameters) Implements iparameter_action(Of parameters).run
        If a Is Nothing Then
            p.run(i)
        Else
            a.run()
        End If
    End Sub
End Class

Public Class action_wrapper(Of T)
    Inherits delegate_wrapper(Of iaction(Of T), parameters(Of T))
    Implements iaction(Of T), iparameter_action(Of parameters(Of T))

    Public Sub New(ByVal a As iaction(Of T))
        MyBase.New(a)
    End Sub

    Public Sub New(ByVal p As iparameter_action(Of parameters(Of T)))
        MyBase.New(p)
    End Sub

    Public Sub run(ByVal v As T) Implements iaction(Of T).run
        If a Is Nothing Then
            p.run(New parameters(Of T)(v))
        Else
            a.run(v)
        End If
    End Sub

    Public Sub run(ByVal i As parameters(Of T)) Implements iparameter_action(Of parameters(Of T)).run
        If a Is Nothing Then
            p.run(i)
        Else
            a.run(If(i Is Nothing, Nothing, i.v))
        End If
    End Sub
End Class

Public Class action_wrapper(Of T1, T2)
    Inherits delegate_wrapper(Of iaction(Of T1, T2), parameters(Of T1, T2))
    Implements iaction(Of T1, T2), iparameter_action(Of parameters(Of T1, T2))

    Public Sub New(ByVal a As iaction(Of T1, T2))
        MyBase.New(a)
    End Sub

    Public Sub New(ByVal p As iparameter_action(Of parameters(Of T1, T2)))
        MyBase.New(p)
    End Sub

    Public Sub run(ByVal v1 As T1, ByVal v2 As T2) Implements iaction(Of T1, T2).run
        If a Is Nothing Then
            p.run(New parameters(Of T1, T2)(v1, v2))
        Else
            a.run(v1, v2)
        End If
    End Sub

    Public Sub run(ByVal i As parameters(Of T1, T2)) Implements iparameter_action(Of parameters(Of T1, T2)).run
        If a Is Nothing Then
            p.run(i)
        ElseIf i Is Nothing Then
            a.run(Nothing, Nothing)
        Else
            a.run(i.v1, i.v2)
        End If
    End Sub
End Class

Public Class action_wrapper(Of T1, T2, T3)
    Inherits delegate_wrapper(Of iaction(Of T1, T2, T3), parameters(Of T1, T2, T3))
    Implements iaction(Of T1, T2, T3), iparameter_action(Of parameters(Of T1, T2, T3))

    Public Sub New(ByVal a As iaction(Of T1, T2, T3))
        MyBase.New(a)
    End Sub

    Public Sub New(ByVal p As iparameter_action(Of parameters(Of T1, T2, T3)))
        MyBase.New(p)
    End Sub

    Public Sub run(ByVal v1 As T1, ByVal v2 As T2, ByVal v3 As T3) _
                  Implements iaction(Of T1, T2, T3).run
        If a Is Nothing Then
            p.run(New parameters(Of T1, T2, T3)(v1, v2, v3))
        Else
            a.run(v1, v2, v3)
        End If
    End Sub

    Public Sub run(ByVal i As parameters(Of T1, T2, T3)) _
                  Implements iparameter_action(Of parameters(Of T1, T2, T3)).run
        If a Is Nothing Then
            p.run(i)
        ElseIf i Is Nothing Then
            a.run(Nothing, Nothing, Nothing)
        Else
            a.run(i.v1, i.v2, i.v3)
        End If
    End Sub
End Class