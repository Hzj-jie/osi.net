
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation

Public NotInheritable Class weak_event_handler
    Public Shared assert_as_not_pinning_delegate As Boolean

    Shared Sub New()
        assert_as_not_pinning_delegate = isdebugmode()
    End Sub

    Public Shared Sub assert_not_pinning_delegate(ByVal v As Object, ByVal a As [Delegate])
        assert(Not (assert_as_not_pinning_delegate AndAlso
                      delegate_is_pinning_object(a, v)))
    End Sub

    Private Sub New()
    End Sub
End Class

Public Class weak_reference_handler(Of T, AT)
    Implements idelegate
    Protected ReadOnly a As AT
    Private ReadOnly v As weak_pointer(Of T)

    Shared Sub New()
        assert(GetType(AT).is(GetType([Delegate])))
    End Sub

    Protected Sub New(ByVal v As T, ByVal a As AT, ByVal o As [Delegate])
        Me.v = weak_pointer.of(v)
        Me.a = a
        weak_event_handler.assert_not_pinning_delegate(v, o)
    End Sub

    Protected Sub New(ByVal v As T, ByVal a As AT)
        Me.New(v, a, direct_cast(Of [Delegate])(a))
    End Sub

    Public Function valid() As Boolean Implements idelegate.valid
        Return Not a Is Nothing AndAlso v.alive()
    End Function

    Protected Function ready_to_run(ByRef v As T) As Boolean
        Return Not a Is Nothing AndAlso
               Me.v.get(v)
    End Function
End Class

Public Class weak_event_handler(Of T)
    Inherits weak_reference_handler(Of T, Action(Of T))
    Implements iaction

    Public Sub New(ByVal v As T, ByVal a As Action(Of T))
        MyBase.New(v, a)
    End Sub

    Public Sub New(ByVal v As T, ByVal a As Action)
        MyBase.New(v, If(a Is Nothing, Nothing, Sub(ByVal x As T) a()), a)
    End Sub

    Public Sub run() Implements iaction.run
        Dim v As T = Nothing
        If ready_to_run(v) Then
            a(v)
        End If
    End Sub
End Class

Public Class weak_event_handler(Of T1, T2)
    Inherits weak_reference_handler(Of T1, Action(Of T1, T2))
    Implements iaction(Of T2)

    Public Sub New(ByVal v As T1, ByVal a As Action(Of T1, T2))
        MyBase.New(v, a)
    End Sub

    Public Sub New(ByVal v As T1, ByVal a As Action(Of T2))
        MyBase.New(v, If(a Is Nothing, Nothing, Sub(ByVal x As T1, ByVal y As T2) a(y)), a)
    End Sub

    Public Sub run(ByVal v2 As T2) Implements iaction(Of T2).run
        Dim v1 As T1 = Nothing
        If ready_to_run(v1) Then
            a(v1, v2)
        End If
    End Sub
End Class

Public Class weak_event_handler(Of T1, T2, T3)
    Inherits weak_reference_handler(Of T1, Action(Of T1, T2, T3))
    Implements iaction(Of T2, T3)

    Public Sub New(ByVal v As T1, ByVal a As Action(Of T1, T2, T3))
        MyBase.New(v, a)
    End Sub

    Public Sub New(ByVal v As T1, ByVal a As Action(Of T2, T3))
        MyBase.New(v, If(a Is Nothing, Nothing, Sub(ByVal x As T1, ByVal y As T2, ByVal z As T3) a(y, z)), a)
    End Sub

    Public Sub run(ByVal v2 As T2, ByVal v3 As T3) Implements iaction(Of T2, T3).run
        Dim v1 As T1 = Nothing
        If ready_to_run(v1) Then
            a(v1, v2, v3)
        End If
    End Sub
End Class

Public Class weak_event_handler(Of T1, T2, T3, T4)
    Inherits weak_reference_handler(Of T1, Action(Of T1, T2, T3, T4))
    Implements iaction(Of T2, T3, T4)

    Public Sub New(ByVal v As T1, ByVal a As Action(Of T1, T2, T3, T4))
        MyBase.New(v, a)
    End Sub

    Public Sub New(ByVal v As T1, ByVal a As Action(Of T2, T3, T4))
        MyBase.New(v, If(a Is Nothing,
                         Nothing,
                         Sub(ByVal w As T1, ByVal x As T2, ByVal y As T3, ByVal z As T4) a(x, y, z)), a)
    End Sub

    Public Sub run(ByVal v2 As T2, ByVal v3 As T3, ByVal v4 As T4) Implements iaction(Of T2, T3, T4).run
        Dim v1 As T1 = Nothing
        If ready_to_run(v1) Then
            a(v1, v2, v3, v4)
        End If
    End Sub
End Class
