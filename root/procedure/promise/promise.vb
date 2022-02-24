
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
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

    Public Function pending() As Boolean
        Return t.pending()
    End Function

    Public Function fulfilled() As Boolean
        Return t.fulfilled()
    End Function

    Public Function rejected() As Boolean
        Return t.rejected()
    End Function

    Private Sub _resolve(ByVal result As Object)
        t.resolve(result)
    End Sub

    Private Sub _reject(ByVal reason As Object)
        t.reject(reason)
    End Sub

    Public Function [then](ByVal on_resolve As Func(Of Object, Object), ByVal on_reject As Action(Of Object)) As promise
        assert(Not on_resolve Is Nothing)
        If Not appended.mark_in_use() Then
            Return Nothing
        End If
        Dim r As promise = Nothing
        r = New promise()
        t.then(Sub(ByVal result As Object)
                   r._resolve(on_resolve(result))
               End Sub,
               Sub(ByVal reason As Object)
                   If Not on_reject Is Nothing Then
                       on_reject(reason)
                   End If
                   r._reject(reason)
               End Sub)
        Return r
    End Function
End Class
