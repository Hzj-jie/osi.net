
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.lock
Imports osi.root.template
Imports osi.root.connector
Imports osi.service.device

Public NotInheritable Class creator_questioner
    Inherits creator_questioner(Of _true)

    Public Sub New(ByVal h As Func(Of herald), ByVal timeout_ms As Int64)
        MyBase.New(h, timeout_ms)
    End Sub

    Public Sub New(ByVal d As Func(Of idevice(Of herald)), ByVal timeout_ms As Int64)
        MyBase.New(d, timeout_ms)
    End Sub
End Class

Public Class creator_questioner(Of _ENABLE_AUTO_PING As _boolean)
    Inherits iexecutable_questioner(Of _ENABLE_AUTO_PING)

    Private ReadOnly h As Func(Of herald)

    Public Sub New(ByVal h As Func(Of herald), ByVal timeout_ms As Int64)
        MyBase.New(timeout_ms)
        assert(Not h Is Nothing)
        Me.h = h
    End Sub

    Public Sub New(ByVal d As Func(Of idevice(Of herald)), ByVal timeout_ms As Int64)
        Me.New(Function() As herald
                   assert(Not d Is Nothing)
                   Dim v As idevice(Of herald) = Nothing
                   v = d()
                   assert(Not v Is Nothing)
                   Return v.get()
               End Function,
               timeout_ms)
    End Sub

    Protected NotOverridable Overrides Function communicate(ByVal i As command,
                                                            ByVal o As ref(Of command)) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim h As herald = Nothing
                                  h = Me.h()
                                  If h Is Nothing Then
                                      Return False
                                  Else
                                      ec = create(h)(i, o)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
