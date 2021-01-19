
Imports osi.root.template
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.service.device

Public NotInheritable Class pool_questioner
    Inherits pool_questioner(Of _true)

    Public Sub New(ByVal p As idevice_pool(Of herald), ByVal timeout_ms As Int64)
        MyBase.New(p, timeout_ms)
    End Sub
End Class

Public Class pool_questioner(Of _ENABLE_AUTO_PING As _boolean)
    Inherits iexecutable_questioner(Of _ENABLE_AUTO_PING)

    Private ReadOnly p As idevice_pool(Of herald)

    Public Sub New(ByVal p As idevice_pool(Of herald), ByVal timeout_ms As Int64)
        MyBase.New(timeout_ms)
        assert(Not p Is Nothing)
        Me.p = p
    End Sub

    Protected NotOverridable Overrides Function communicate(ByVal request As command,
                                                            ByVal response As ref(Of command)) As event_comb
        Dim ec As event_comb = Nothing
        Dim d As idevice(Of herald) = Nothing
        Return New event_comb(Function() As Boolean
                                  If p.get(d) Then
                                      ec = create(d)(request, response)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  p.release(d)
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
