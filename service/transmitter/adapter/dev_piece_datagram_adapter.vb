
Imports osi.root.template
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

<type_attribute()>
Public Class dev_piece_datagram_adapter
    Inherits dev_piece_datagram_adapter(Of _1000)

    Public Sub New(ByVal d As dev_T(Of piece))
        MyBase.New(d)
    End Sub
End Class

<type_attribute()>
Public Class dev_piece_datagram_adapter(Of _PACKET_SIZE As _int64)
    Inherits T_adapter(Of dev_T(Of piece))
    Implements datagram

    Private Shared ReadOnly packet_size As UInt32

    Shared Sub New()
        packet_size = +alloc(Of _PACKET_SIZE)()
        assert(packet_size > 0)
    End Sub

    Private Shared Function attach_type_attribute(ByVal d As dev_T(Of piece)) As type_attribute
        Dim r As type_attribute = Nothing
        r = New type_attribute()
        r.set(datagram_trait.[New](type_attribute.of(d).get()).with_packet_size(packet_size))
        Return r
    End Function

    Public Sub New(ByVal d As dev_T(Of piece))
        MyBase.New(d, attach_type_attribute(d))
    End Sub

    Public Function send(ByVal buff() As Byte,
                         ByVal offset As UInt32,
                         ByVal count As UInt32,
                         ByVal result As ref(Of UInt32)) As event_comb Implements flow_injector.send
        Dim p As piece = Nothing
        Dim ec As event_comb = Nothing
        Dim sent As UInt32 = 0
        Return New event_comb(Function() As Boolean
                                  If count = 0 Then
                                      Return goto_end()
                                  Else
                                      sent = min(count, packet_size)
                                      If piece.create(buff, offset, sent, p) Then
                                          ec = underlying_device.send(p)
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      Else
                                          Return False
                                      End If
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         eva(result, sent) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function receive(ByVal result As ref(Of Byte())) As event_comb Implements block_pump.receive
        Dim p As ref(Of piece) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  p = New ref(Of piece)()
                                  ec = underlying_device.receive(p)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      Return eva(result, ++p) AndAlso
                                             goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function sense(ByVal pending As ref(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements sensor.sense
        Return underlying_device.sense(pending, timeout_ms)
    End Function
End Class
