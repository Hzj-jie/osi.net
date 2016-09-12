
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
    Private ReadOnly pump As slimqless2_event_sync_T_pump(Of Byte())
    Private ReadOnly receiver As event_sync_T_pump_T_receiver_adapter(Of Byte())
    Private ReadOnly accepter As listener.multiple_accepter
    Private ReadOnly buff_size As atomic_int32

    Shared Sub New()
        assert(constants.ipv6_packet_size <= constants.ipv4_packet_size)
        type_attribute.of(Of udp_dev).set(datagram_transmitter.[New]().
                                          with_packet_size(constants.ipv6_packet_size).
                                          with_transmit_mode(transmitter.mode_t.duplex))
    End Sub

    Public Sub New(ByVal p As powerpoint, ByVal sources As const_array(Of IPEndPoint))
        assert(Not p Is Nothing)
        Me.p = p
        Me.s = speakers.[New](p)
        Me.pump = New slimqless2_event_sync_T_pump(Of Byte())()
        Me.receiver = event_sync_T_pump_T_receiver_adapter.[New](Me.pump)
        Me.accepter = New listener.multiple_accepter(sources)
        Me.buff_size = New atomic_int32()
        AddHandler accepter.received, AddressOf push_queue
        assert(listeners.[New](p).attach(accepter))
    End Sub

    Private Sub push_queue(ByVal b() As Byte, ByVal remote As IPEndPoint)
        If Me.buff_size.add(array_size(b)) > p.max_receive_buffer_size Then
            pop_queue(Nothing)
        End If
        Me.pump.emplace(b)
    End Sub

    Private Function pop_queue(ByRef b() As Byte) As Boolean
        If Me.pump.receive(b) Then
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
        Return receiver.receive(result)
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
        Return receiver.sense(pending, timeout_ms)
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
