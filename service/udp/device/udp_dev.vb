
Imports System.Net
Imports osi.root.connector
Imports osi.root.event
Imports osi.root.lock
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.device

' A udp device sends to and receives from a specific remote host (ip + port, v4 or v6),
' and sends from and receives to a specific local port.
' So remote ip + remote port + local port can identify a device.
' TODO: Uses slimqless2_event_sync_T_pump / event_sync_T_pump_T_receiver
<type_attribute()>
Public Class udp_dev
    Implements datagram

    Private ReadOnly p As powerpoint
    Private ReadOnly s As speaker
    Private ReadOnly accepter As listener.multiple_accepter
    Private ReadOnly q As slimqless2(Of Byte())
    Private ReadOnly received_event As signal_event
    Private ReadOnly buff_size As atomic_int32
    Private ReadOnly sensor As sensor

    Shared Sub New()
        assert(constants.ipv6_packet_size <= constants.ipv4_packet_size)
        type_attribute.of(Of udp_dev).set(datagram_transmitter.[New]().
                                          with_packet_size(constants.ipv6_packet_size).
                                          with_transmit_mode(transmitter.mode_t.duplex))
    End Sub

    Public Sub New(ByVal p As powerpoint, ByVal accepter As listener.multiple_accepter)
        assert(Not accepter Is Nothing)
        assert(Not p Is Nothing)
        assert(listeners.[New](p).attach(accepter))

        Me.p = p
        Me.s = speakers.[New](p)
        Me.accepter = accepter
        Me.q = New slimqless2(Of Byte())()
        Me.received_event = New signal_event()
        Me.buff_size = New atomic_int32()
        Me.sensor = as_sensor(Function() As Boolean
                                  Return Not q.empty()
                              End Function)

        AddHandler accepter.received, AddressOf push_queue
    End Sub

    Private Sub push_queue(ByVal b() As Byte, ByVal remote As IPEndPoint)
        If Me.buff_size.add(array_size(b)) > p.max_receive_buffer_size Then
            pop_queue(Nothing)
        End If
        Me.q.emplace(b)
        Me.received_event.mark()
    End Sub

    Private Function pop_queue(ByRef b() As Byte) As Boolean
        If q.pop(b) Then
            assert(Me.buff_size.add(-CInt(array_size(b))) >= 0)
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub close()
        assert(listeners.[New](p).detach(accepter))
    End Sub

    Public Function receive(ByVal result As pointer(Of Byte())) As event_comb Implements block_pump.receive
        Return New event_comb(Function() As Boolean
                                  Dim b() As Byte = Nothing
                                  If pop_queue(b) Then
                                      If q.empty() Then
                                          received_event.unmark()
                                          If Not q.empty() Then
                                              received_event.mark()
                                          End If
                                      End If
                                      Return eva(result, b) AndAlso
                                             goto_end()
                                  Else
                                      Return waitfor(received_event)
                                  End If
                              End Function)
    End Function

    Public Function send(ByVal buff() As Byte,
                         ByVal offset As UInt32,
                         ByVal count As UInt32,
                         ByVal sent As pointer(Of UInt32)) As event_comb Implements flow_injector.send
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim remote As IPEndPoint = Nothing
                                  If accepter.first_source(remote) Then
                                      ec = s.send(remote, buff, offset, count, sent)
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

    Public Function sense(ByVal pending As pointer(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements sensor.sense
        Return sensor.sense(pending, timeout_ms)
    End Function

    Public Shared Operator +(ByVal this As udp_dev) As idevice(Of datagram)
        assert(Not this Is Nothing)
        Return this.make_device(closer:=Sub(i As udp_dev)
                                            assert(Not i Is Nothing)
                                            i.close()
                                        End Sub,
                                identifier:=Function(i As udp_dev) As String
                                                assert(Not i Is Nothing)
                                                Return i.p.identity
                                            End Function)
    End Operator
End Class
