
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.template
Imports osi.root.utils

<type_attribute()>
Public Class flow_datagram_adapter
    Inherits flow_datagram_adapter(Of _1500)

    Shared Sub New()
        type_attribute.of(Of flow_datagram_adapter)().forward_from(
            type_attribute.of(Of flow_datagram_adapter(Of _1500))())
    End Sub

    Public Sub New(ByVal f As flow)
        MyBase.New(f)
    End Sub

    Public Shared Function [New](ByVal f As flow) As datagram
        Return New flow_datagram_adapter(f)
    End Function
End Class

<type_attribute()>
Public Class flow_datagram_adapter(Of _PACKET_SIZE As _int64)
    Inherits T_adapter(Of flow)
    Implements datagram

    Private Shared ReadOnly packet_size As UInt32

    Shared Sub New()
        packet_size = alloc(Of _PACKET_SIZE).as_uint32()
        assert(packet_size > 0)
    End Sub

    Private Shared Function forward_type_attribute(ByVal f As flow) As type_attribute
        Dim r As type_attribute = Nothing
        r = (New type_attribute()).with_store()
        r.set(datagram_trait.[New](type_attribute.of(f).get(Of trait)()).
                  with_packet_size(packet_size))
        Return r
    End Function

    Public Sub New(ByVal f As flow)
        MyBase.New(f, forward_type_attribute(f))
    End Sub

    Public Function receive(ByVal result As ref(Of Byte())) As event_comb Implements block_pump.receive
        Dim buff() As Byte = Nothing
        Dim received As ref(Of UInt32) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ReDim buff(packet_size - uint32_1)
                                  received = New ref(Of UInt32)()
                                  ec = _pump.receive(underlying_device, buff, received)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If (+received) = uint32_0 Then
                                          buff = Nothing
                                      ElseIf (+received) < array_size(buff) Then
                                          ReDim Preserve buff((+received) - 1)
                                      End If
                                      Return eva(result, buff) AndAlso
                                             goto_end()
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
        Return New event_comb(Function() As Boolean
                                  If offset + count > array_size(buff) Then
                                      Return False
                                  ElseIf count = uint32_0 Then
                                      Return eva(sent, uint32_0) AndAlso
                                             goto_end()
                                  Else
                                      ec = underlying_device.send(buff, offset, min(count, packet_size), sent)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function sense(ByVal pending As ref(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements sensor.sense
        Return underlying_device.sense(pending, timeout_ms)
    End Function
End Class
