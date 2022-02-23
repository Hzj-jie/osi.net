
Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.transmitter
Imports envs = osi.root.envs

<type_attribute()>
Partial Public Class client_flow_adapter
    Implements flow

    Shared Sub New()
        type_attribute.of(Of client_flow_adapter)().forward_from(Of ref_client)()
    End Sub

    Private ReadOnly c As ref_client
    Private ReadOnly p As powerpoint
    Private ReadOnly f As flow

    Public Sub New(ByVal c As ref_client, ByVal p As powerpoint)
        assert(c IsNot Nothing)
        assert(p IsNot Nothing)
        Me.c = c
        Me.p = p
        Me.f = If(constants.use_socket,
                  New client_socket_flow_adapter(+c, p),
                  New client_stream_flow_adapter(+c, p))
    End Sub

    Public Sub New(ByVal c As TcpClient, ByVal p As powerpoint)
        Me.New(New ref_client(c), p)
    End Sub

    Private Function finish(ByVal ec As event_comb, ByVal m As String) As Boolean
        assert(ec IsNot Nothing)
        If ec.end_result() Then
            Return goto_end()
        Else
            If envs.tcp_trace Then
                raise_error("failed to ", m, " data to ", c.id, " @ ", p.identity)
            End If
            c.client().shutdown()
            Return False
        End If
    End Function

    Public Function send(ByVal buff() As Byte,
                         ByVal offset As UInt32,
                         ByVal count As UInt32,
                         ByVal sent As ref(Of UInt32)) As event_comb Implements flow.send
        c.update_refer_ms()
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = f.send(buff, offset, count, sent)
                                  assert(ec IsNot Nothing)
                                  Return waitfor(ec, p.send_timeout_ms(count)) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If envs.tcp_detail_trace Then
                                      raise_error(c.id, " sent ", bytes_str(buff, offset, +sent))
                                  End If
                                  Return finish(ec, "send")
                              End Function)
    End Function

    Public Function receive(ByVal buff() As Byte,
                            ByVal offset As UInt32,
                            ByVal count As UInt32,
                            ByVal result As ref(Of UInt32)) As event_comb _
                           Implements flow.receive
        c.update_refer_ms()
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = f.receive(buff, offset, count, result)
                                  Return waitfor(ec, p.receive_timeout_ms(count)) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If envs.tcp_detail_trace Then
                                      raise_error(c.id, " received ", bytes_str(buff, offset, +result))
                                  End If
                                  Return finish(ec, "receive")
                              End Function)
    End Function

    Public Function sense(ByVal pending As ref(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements flow.sense
        c.update_refer_ms()
        Return f.sense(pending, timeout_ms)
    End Function
End Class
