
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.template
Imports osi.service.device
Imports osi.service.transmitter

Public Class herald_responder(Of CONTINUOUS As _boolean)
    Inherits iexecutable_responder(Of CONTINUOUS)

    Private Shared Sub assert_transmit_mode(ByVal h As herald)
        Dim t As trait.mode_t = Nothing
        t = h.transmit_mode()
        assert(t = trait.mode_t.duplex OrElse
               t = trait.mode_t.receive_send)
    End Sub

    Private ReadOnly instance As herald

    Public Sub New(ByVal h As herald,
                   ByVal pending_request_timeout_ms As Int64,
                   ByVal e As executor,
                   ByVal stopping As Func(Of Boolean))
        MyBase.New(pending_request_timeout_ms, e, stopping)
        assert(Not h Is Nothing)
        Me.instance = h
        assert_transmit_mode(h)
    End Sub

    Public Sub New(ByVal h As herald,
                   ByVal pending_request_timeout_ms As Int64,
                   ByVal e As executor)
        Me.New(h, pending_request_timeout_ms, e, Nothing)
    End Sub

    Public Sub New(ByVal h As idevice(Of herald),
                   ByVal pending_request_timeout_ms As Int64,
                   ByVal e As executor,
                   ByVal stopping As Func(Of Boolean))
        Me.New(assert_which.of(h).is_not_null().get(), pending_request_timeout_ms, e, stopping)
    End Sub

    Public Sub New(ByVal h As idevice(Of herald),
                   ByVal pending_request_timeout_ms As Int64,
                   ByVal e As executor)
        Me.New(h, pending_request_timeout_ms, e, Nothing)
    End Sub

    Private Function execute(ByVal i As command, ByVal o As command) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If o Is Nothing Then
                                      Return False
                                  Else
                                      If i Is Nothing OrElse
                                         i.empty() Then
                                          o.attach(constants.response.invalid_request)
                                          Return goto_end()
                                      ElseIf i.action_is(constants.action.ping) Then
                                          o.attach(constants.response.success)
                                          Return goto_end()
                                      Else
                                          ec = e.execute(i, o)
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      End If
                                  End If
                              End Function,
                              Function() As Boolean
                                  If Not o.has_action() Then
                                      o.attach(If(ec.end_result(),
                                                  constants.response.success,
                                                  constants.response.failure))
                                  End If
                                  Return goto_end()
                              End Function)
    End Function

    Private Function respond_once() As event_comb
        assert(Not instance Is Nothing)
        Dim ec As event_comb = Nothing
        Dim i As ref(Of command) = Nothing
        Dim o As command = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = instance.sense(pending_request_timeout_ms)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      i = New ref(Of command)()
                                      ec = instance.receive(i)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      o = New command()
                                      ec = execute(+i, o)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      assert(Not o.empty())
                                      ec = instance.send(o)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public NotOverridable Overrides Function respond() As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = respond_once()
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If continuous AndAlso Not expired() Then
                                          Return goto_begin()
                                      Else
                                          Return goto_end()
                                      End If
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public NotOverridable Overrides Function respond_one_of() As event_comb
        assert(False)
        Return Nothing
    End Function

    Public NotOverridable Overrides Function respond_all() As Boolean
        assert(False)
        Return Nothing
    End Function
End Class
