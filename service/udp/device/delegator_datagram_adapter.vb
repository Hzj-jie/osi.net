
Imports System.Net
Imports System.Net.Sockets
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.transmitter

<type_attribute()>
Partial Public Class delegator_datagram_adapter
    Implements datagram

    ' This is a safe value for both ipv4 and ipv6
    Public Const max_packet_size As UInt32 = 65535 - 8 - 40

    Shared Sub New()
        type_attribute.of(Of delegator_datagram_adapter)().set(datagram_trait.[New]().
            with_packet_size(max_packet_size).
            with_transmit_mode(trait.mode_t.duplex))
    End Sub

    Private ReadOnly c As delegator
    Private ReadOnly s As sensor
    Private ReadOnly timeout As transceive_timeout
    Private last_target As IPEndPoint

    Public Sub New(ByVal c As delegator)
        assert(Not c Is Nothing)
        Me.c = c
        Me.s = New indicator_sensor_adapter(New delegator_indicator(c))
        If Not c.p Is Nothing Then
            Me.timeout = c.p.transceive_timeout()
        End If
    End Sub

    Private Shared Function connect_client(ByVal c As UdpClient, ByVal target As IPEndPoint) As UdpClient
        assert(Not c Is Nothing)
        If Not target Is Nothing Then
            c.Connect(target)
        End If
        Return c
    End Function

    ' For test purpose
    Public Sub New(ByVal u As UdpClient,
                   ByVal sources() As IPEndPoint,
                   ByVal target As IPEndPoint,
                   ByVal timeout As transceive_timeout)
        Me.New(New delegator(sources, connect_client(u, target)))
        assert(Me.timeout Is Nothing)
        Me.timeout = timeout
    End Sub

    Public Function receive(ByVal result As ref(Of Byte())) As event_comb Implements block_pump.receive
        Dim ec As event_comb = Nothing
        Dim p As ref(Of IPEndPoint) = Nothing
        Return New event_comb(Function() As Boolean
                                  p = New ref(Of IPEndPoint)()
                                  ec = c.receive(result, p)
                                  Return waitfor(ec, timeout.receive_timeout_ms_or_npos(max_packet_size)) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      last_target = +p
                                      Return goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function send(ByVal buff() As Byte,
                         ByVal offset As UInt32,
                         ByVal count As UInt32,
                         ByVal sent As ref(Of UInt32)) As event_comb Implements flow_injector.send
        Dim ec As event_comb = Nothing
        Dim l As UInt32 = uint32_0
        Return New event_comb(Function() As Boolean
                                  Dim p As piece = Nothing
                                  If piece.create(buff, offset, min(count, max_packet_size), p) Then
                                      sent.renew()
                                      Dim b() As Byte = Nothing
                                      b = p.export(l)
                                      ec = c.send(b, l, last_target, sent)
                                      Return waitfor(ec, timeout.send_timeout_ms_or_npos(count)) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         (+sent) = l AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function sense(ByVal pending As ref(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements sensor.sense
        Return s.sense(pending, timeout_ms)
    End Function
End Class
