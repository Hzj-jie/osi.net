
Imports osi.root.template
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.service.device

Public Class pool_responder(Of CONTINUOUS As _boolean)
    Inherits iexecutable_responder(Of CONTINUOUS)

    Private ReadOnly p As idevice_pool(Of herald)

    Public Sub New(ByVal p As idevice_pool(Of herald),
                   ByVal pending_request_timeout_ms As Int64,
                   ByVal e As executor,
                   ByVal stopping As Func(Of Boolean))
        MyBase.New(pending_request_timeout_ms, e, If(stopping Is Nothing,
                                                     Function() As Boolean
                                                         Return p.expired()
                                                     End Function,
                                                     stopping))
        assert(Not p Is Nothing)
        Me.p = p
    End Sub

    Public Sub New(ByVal p As idevice_pool(Of herald),
                   ByVal pending_request_timeout_ms As Int64,
                   ByVal e As executor)
        Me.New(p, pending_request_timeout_ms, e, Nothing)
    End Sub

    Public NotOverridable Overrides Function respond() As event_comb
        assert(False)
        Return Nothing
    End Function

    Public NotOverridable Overrides Function respond_all() As Boolean
        Return p.get_all(Function(d As idevice(Of herald)) As event_comb
                             Return create(d).respond()
                         End Function,
                         AddressOf expired)
    End Function

    Public NotOverridable Overrides Function respond_one_of() As event_comb
        Dim ec As event_comb = Nothing
        Dim d As idevice(Of herald) = Nothing
        Return New event_comb(Function() As Boolean
                                  If p.get(d) Then
                                      ec = create(d).respond()
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
