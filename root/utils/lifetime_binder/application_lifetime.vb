
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.lock

Public Class application_lifetime_binder(Of T As Class)
    Inherits lifetime_binder(Of T)

    Public Shared Shadows ReadOnly instance As application_lifetime_binder(Of T) =
        New application_lifetime_binder(Of T)()

    Protected Sub New()
        MyBase.New()
        AddHandler AppDomain.CurrentDomain().ProcessExit, AddressOf clear
    End Sub
End Class

Public NotInheritable Class application_lifetime_binder
    Inherits application_lifetime_binder(Of IDisposable)

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class application_lifetime
    Private Shared ReadOnly e As expiration_controller.settable = create_settable()

    Public Shared Function expiration_controller() As expiration_controller
        Return e
    End Function

    Public Shared Function running() As Boolean
        Return e.alive()
    End Function

    Public Shared Function stopping() As Boolean
        Return e.expired()
    End Function

    Public Shared Function stopping_signal() As Func(Of Boolean)
        Return e.stopping_signal()
    End Function

    Public Shared Sub stopping_handle(ByVal v As Action)
        application_lifetime_binder.instance.insert(make_disposer(Function() 0, disposer:=Sub(i) v()))
    End Sub

    Private Shared Function create_settable() As expiration_controller.settable
        Dim r As expiration_controller.settable = lock.expiration_controller.settable.[New]()
        assert(r.alive())
        stopping_handle(Sub() r.stop())
        Return r
    End Function
End Class
