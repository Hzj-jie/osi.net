
Imports System.Net
Imports System.Net.Sockets
Imports System.Reflection
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.device

<type_attribute()>
Partial Public Class client_datagram_adapter
    Implements datagram

    Public Const max_packet_size As UInt32 = 65535 - 8 - 20

    Shared Sub New()
        type_attribute.of(Of client_datagram_adapter)().set(datagram_transmitter.[New]().
            with_packet_size(max_packet_size).
            with_transmit_mode(transmitter.mode_t.duplex))
    End Sub

    Private ReadOnly c As ref_client
    Private ReadOnly target As IPEndPoint
    Private ReadOnly s As sensor
    Private ReadOnly timeout As transceive_timeout
    Private last_target As IPEndPoint

    Public Sub New(ByVal c As ref_client)
        assert(Not c Is Nothing)
        Me.c = c
        ' max_packet_size is usually larger than MTU (1500 for ethernet), so we allow datagrams to be fragmented.
        c.no_refer_client().DontFragment() = False
        ' We do not want icmp reset connect signal to make any trouble.
        c.no_refer_client().disable_icmp_reset()
        Me.s = New indicator_sensor_adapter(New client_indicator(c.no_refer_client()))
        If Not c.p Is Nothing Then
            Me.timeout = c.p.transceive_timeout()
        End If
    End Sub

    Public Sub New(ByVal u As UdpClient,
                   ByVal sources() As IPEndPoint,
                   ByVal target As IPEndPoint,
                   ByVal timeout As transceive_timeout)
        Me.New(New ref_client(sources, assert_not_nothing_return(u)))
        Me.target = target
        assert(Me.timeout Is Nothing)
        Me.timeout = timeout
    End Sub

    Private Shared Function match_endpoint(ByVal received_from As IPEndPoint, ByVal expected As IPEndPoint) As Boolean
        assert(Not received_from Is Nothing)
        If expected Is Nothing Then
            Return False
        End If
        If received_from.Equals(expected) Then
            Return True
        Else
            Dim rp As UInt16 = uint16_0
            Dim ep As UInt16 = uint16_0
            rp = received_from.Port()
            ep = expected.Port()
            If rp = ep OrElse ep = uint16_0 Then
                ' If port has not been set, treat them as equal.
                Dim ea As IPAddress = Nothing
                ea = expected.Address()
                If ea.Equals(IPAddress.Any) OrElse ea.Equals(IPAddress.IPv6Any) Then
                    Return True
                End If

                Dim ra As IPAddress = Nothing
                ra = received_from.Address()
                If ra.Equals(ea) Then
                    Return True
                Else
                    Dim rb() As Byte = Nothing
                    Dim eb() As Byte = Nothing
                    rb = ra.GetAddressBytes()
                    eb = ea.GetAddressBytes()
                    If array_size(rb) = array_size(eb) Then
                        assert(Not isemptyarray(rb))
                        For i As UInt32 = uint32_0 To array_size(rb) - uint32_1
                            ' Make it simpler, if one byte is 255, treat it as broadcast mask.
                            If rb(i) <> eb(i) AndAlso eb(i) <> max_uint8 Then
                                Return False
                            End If
                        Next
                        Return True
                    Else
                        Return False
                    End If
                End If
            Else
                Return False
            End If
        End If
    End Function

    Public Function receive(ByVal result As pointer(Of Byte())) As event_comb Implements block_pump.receive
        Dim ec As event_comb = Nothing
        Dim p As pointer(Of IPEndPoint) = Nothing
        Return New event_comb(Function() As Boolean
                                  If Not c.sources.null_or_empty() Then
                                      p = New pointer(Of IPEndPoint)()
                                  End If
                                  ec = c.client().receive(result, p)
                                  Return waitfor(ec, timeout.receive_timeout_ms_or_npos(max_packet_size)) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If c.sources.null_or_empty() Then
                                          Return goto_end()
                                      Else
                                          assert(Not +p Is Nothing)
                                          For i As UInt32 = uint32_0 To c.sources.size() - uint32_1
                                              If match_endpoint((+p), c.sources(i)) Then
                                                  last_target = c.sources(i)
                                                  Return goto_end()
                                              End If
                                          Next
                                          ' Drop datagrams from unknown sources.
                                          Return goto_begin()
                                      End If
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function send(ByVal buff() As Byte,
                         ByVal offset As UInt32,
                         ByVal count As UInt32,
                         ByVal sent As pointer(Of UInt32)) As event_comb Implements flow_injector.send
        Dim ec As event_comb = Nothing
        Dim l As UInt32 = uint32_0
        Return New event_comb(Function() As Boolean
                                  Dim p As piece = Nothing
                                  If piece.create(buff, offset, min(count, max_packet_size), p) Then
                                      sent.renew()
                                      Dim b() As Byte = Nothing
                                      b = p.export(l)
                                      If Not target Is Nothing Then
                                          ec = c.client().send(b, l, target, sent)
                                      ElseIf c.client().active() Then
                                          ec = c.client().send(b, l, sent)
                                      Else
                                          assert(Not last_target Is Nothing)
                                          ec = c.client().send(b, l, last_target, sent)
                                      End If
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

    Public Function sense(ByVal pending As pointer(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements sensor.sense
        Return s.sense(pending, timeout_ms)
    End Function
End Class
