
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.device
Imports osi.service.selector
Imports osi.service.transmitter

' A udp device sends to and receives from a specific remote host (ip + port, v4 or v6),
' and sends from and receives to a specific local port.
' So remote ip + remote port / local port can identify a device.
<type_attribute()>
Public Class udp_dev
    Implements datagram

    Private ReadOnly p As powerpoint
    Private ReadOnly local_port As UInt16   ' If this udp_dev is valid, this local_port is always valid.
    Private ReadOnly s As speaker
    Private ReadOnly pump As slimqless2_event_sync_T_pump(Of Byte())
    Private ReadOnly receiver As event_sync_T_pump_T_receiver_adapter(Of Byte())
    Private ReadOnly accepter As listener.multiple_accepter
    Private ReadOnly buff_size As atomic_int32

    Shared Sub New()
        assert(constants.ipv6_packet_size <= constants.ipv4_packet_size)
        type_attribute.of(Of udp_dev).set(datagram_trait.[New]().
                                          with_packet_size(constants.ipv6_packet_size).
                                          with_transmit_mode(trait.mode_t.duplex))
    End Sub

    Public Sub New(ByVal p As powerpoint, ByVal sources As const_array(Of IPEndPoint), ByVal buff() As Byte)
        assert(Not p Is Nothing)
        Me.p = p
        If p.local_port = socket_invalid_port Then
            If Not udp_clients.next(p, Me.local_port, Nothing) Then
                Me.local_port = socket_invalid_port
            End If
        Else
            Me.local_port = p.local_port
        End If
        If valid() Then
            Me.s = speakers.[New](p, local_port)
            Me.pump = New slimqless2_event_sync_T_pump(Of Byte())()
            Me.receiver = event_sync_T_pump_T_receiver_adapter.[New](Me.pump)
            Me.accepter = New listener.multiple_accepter(sources)
            Me.buff_size = New atomic_int32()
            AddHandler accepter.received, AddressOf push_queue
            If Not buff Is Nothing Then
                push_queue(buff, Nothing)
            End If
            assert(listeners.[New](p, local_port).attach(accepter))
        End If
    End Sub

    Public Sub New(ByVal p As powerpoint, ByVal sources As const_array(Of IPEndPoint))
        Me.New(p, sources, Nothing)
    End Sub

    Private Sub push_queue(ByVal b() As Byte, ByVal remote As IPEndPoint)
        If Me.buff_size.add(array_size_i(b)) > p.max_receive_buffer_size Then
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

    Public Function valid() As Boolean
        Return local_port <> socket_invalid_port
    End Function

    Private Sub close()
        If valid() Then
            assert(listeners.[New](p, local_port).detach(accepter))
        End If
    End Sub

    Public Function receive(ByVal result As ref(Of Byte())) As event_comb Implements block_pump.receive
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If valid() Then
                                      ec = receiver.receive(result)
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

    Public Function send(ByVal buff() As Byte,
                         ByVal offset As UInt32,
                         ByVal count As UInt32,
                         ByVal sent As ref(Of UInt32)) As event_comb Implements flow_injector.send
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If valid() Then
                                      Dim remote As IPEndPoint = Nothing
                                      If accepter.first_source(remote) Then
                                          ec = s.send(remote, buff, offset, count, sent)
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      Else
                                          Return False
                                      End If
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function sense(ByVal pending As ref(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements sensor.sense
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If valid() Then
                                      ec = receiver.sense(pending, timeout_ms)
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

    Private Shared Function validator(ByVal i As udp_dev) As Boolean
        assert(Not i Is Nothing)
        Return i.valid()
    End Function

    Private Shared Sub closer(ByVal i As udp_dev)
        assert(Not i Is Nothing)
        i.close()
    End Sub

    Private Shared Function identifier(ByVal i As udp_dev) As String
        assert(Not i Is Nothing)
        Return i.p.identity
    End Function

    Public Shared Operator +(ByVal this As udp_dev) As idevice(Of udp_dev)
        assert(Not this Is Nothing)
        Return this.make_device(validator:=AddressOf validator,
                                closer:=AddressOf closer,
                                identifier:=AddressOf identifier)
    End Operator

    Public Shared Function make_device(ByVal this As async_getter(Of udp_dev)) As idevice(Of async_getter(Of udp_dev))
        Return this.make_device(validator:=AddressOf osi.service.udp.udp_dev.validator,
                                closer:=AddressOf osi.service.udp.udp_dev.closer,
                                identifier:=AddressOf osi.service.udp.udp_dev.identifier)
    End Function
End Class
