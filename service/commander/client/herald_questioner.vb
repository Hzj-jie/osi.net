
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.lock
Imports osi.root.template
Imports osi.root.connector
Imports osi.service.device
Imports osi.service.transmitter

Public NotInheritable Class herald_questioner
    Inherits herald_questioner(Of _true)

    Public Sub New(ByVal h As herald, ByVal timeout_ms As Int64)
        MyBase.New(h, timeout_ms)
    End Sub

    Public Sub New(ByVal d As idevice(Of herald), ByVal timeout_ms As Int64)
        MyBase.New(d, timeout_ms)
    End Sub
End Class

Public Class herald_questioner(Of _ENABLE_AUTO_PING As _boolean)
    Inherits iexecutable_questioner(Of _ENABLE_AUTO_PING)

    Private Shared Sub assert_transmit_mode(ByVal h As herald)
        Dim t As osi.service.transmitter.transmitter.mode_t = Nothing
        t = h.transmit_mode()
        assert(t = osi.service.transmitter.transmitter.mode_t.duplex OrElse
               t = osi.service.transmitter.transmitter.mode_t.send_receive)
    End Sub

    Private ReadOnly h As herald

    Public Sub New(ByVal h As herald, ByVal timeout_ms As Int64)
        MyBase.New(timeout_ms)
        assert(Not h Is Nothing)
        Me.h = h
        assert_transmit_mode(h)
    End Sub

    Public Sub New(ByVal d As idevice(Of herald), ByVal timeout_ms As Int64)
        Me.New(assert_not_nothing_return(d).get(), timeout_ms)
    End Sub

    Protected NotOverridable Overrides Function communicate(ByVal i As command,
                                                            ByVal o As pointer(Of command)) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If i Is Nothing Then
                                      Return False
                                  Else
                                      ec = h.send(i)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      ec = h.sense(timeout_ms)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      ec = h.receive(o)
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
End Class
