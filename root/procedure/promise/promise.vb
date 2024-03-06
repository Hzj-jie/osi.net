
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.lock

Partial Public NotInheritable Class promise
    Private ReadOnly t As thenable
    Private appended As singleentry

    Private Sub New(ByVal t As thenable)
        assert(Not t Is Nothing)
        Me.t = t
    End Sub

    Public Sub New(ByVal executor As Action(Of Action(Of Object), Action(Of Object)))
        Me.New(New auto(executor))
    End Sub

    Private Sub New()
        Me.New(New thenable())
    End Sub

    Public Function [then](ByVal on_resolve As Func(Of Object, Object), ByVal on_reject As Action(Of Object)) As promise
        assert(Not on_resolve Is Nothing)
        If Not appended.mark_in_use() Then
            Return Nothing
        End If
        Dim r As New promise()
        t.then(Sub(ByVal result As Object)
                   r.t.resolve(on_resolve(result))
               End Sub,
               Sub(ByVal reason As Object)
                   If Not on_reject Is Nothing Then
                       on_reject(reason)
                   End If
                   r.t.reject(reason)
               End Sub)
        Return r
    End Function

    ' Nice-to-have shortcuts.

    Public Sub New(ByVal executor As Action(Of Action(Of Object)))
        Me.New(Sub(ByVal resolve As Action(Of Object), ByVal reject As Action(Of Object))
                   assert(Not executor Is Nothing)
                   executor(resolve)
               End Sub)
    End Sub

    Public Sub New(ByVal executor As Func(Of Object))
        Me.New(Sub(ByVal resolve As Action(Of Object), ByVal reject As Action(Of Object))
                   assert(Not executor Is Nothing)
                   resolve(executor())
               End Sub)
    End Sub

    Public Shared Function [of](ByVal executor As Action(Of Action(Of Object), Action(Of Object))) As promise
        Return New promise(executor)
    End Function

    Public Shared Function [of](ByVal executor As Action(Of Action(Of Object))) As promise
        Return New promise(executor)
    End Function

    Public Shared Function [of](ByVal executor As Func(Of Object)) As promise
        Return New promise(executor)
    End Function

    Public Function [then](ByVal on_resolve As Func(Of Object, Object)) As promise
        Return [then](on_resolve, Nothing)
    End Function

    Public Function [then](ByVal on_resolve As Action(Of Object), ByVal on_reject As Action(Of Object)) As promise
        assert(Not on_resolve Is Nothing)
        Return [then](Function(ByVal result As Object) As Object
                          on_resolve(result)
                          Return result
                      End Function,
                      on_reject)
    End Function

    Public Function [then](ByVal on_resolve As Action(Of Object)) As promise
        Return [then](on_resolve, Nothing)
    End Function

    Public Function [then](ByVal on_resolve As Action) As promise
        Return [then](on_resolve.parameter_erasure(Of Object)(), Nothing)
    End Function
End Class
