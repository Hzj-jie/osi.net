
Imports osi.root.connector
Imports osi.root.template
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.device

<type_attribute()>
Public Class mock_datagram_dev(Of _SEND_RANDOM_FAIL As _boolean, _RECEIVE_RANDOM_FAIL As _boolean)
    Inherits mock_dev_T(Of piece)
    Implements datagram

    Private Const packet_size As UInt32 = 1472
    Private Shared ReadOnly send_random_fail As Boolean
    Private Shared ReadOnly receive_random_fail As Boolean

    Shared Sub New()
        send_random_fail = +alloc(Of _SEND_RANDOM_FAIL)()
        receive_random_fail = +alloc(Of _RECEIVE_RANDOM_FAIL)()
        type_attribute.of(Of mock_datagram_dev(Of _SEND_RANDOM_FAIL, _RECEIVE_RANDOM_FAIL))().set(
            datagram_transmitter.[New]() _
                .with_packet_size(packet_size) _
                .with_concurrent_operation(True) _
                .with_transmit_mode(transmitter.mode_t.duplex))
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Sub New(ByVal send_q As qless2(Of piece),
                      ByVal receive_q As qless2(Of piece))
        MyBase.New(send_q, receive_q)
    End Sub

    Protected Shadows Function the_other_end(Of RT As mock_datagram_dev(Of _SEND_RANDOM_FAIL, _RECEIVE_RANDOM_FAIL)) _
                                            (ByVal c As Func(Of qless2(Of piece), qless2(Of piece), RT)) As RT
        Return MyBase.the_other_end(c)
    End Function

    Public Shadows Function the_other_end() As mock_datagram_dev(Of _SEND_RANDOM_FAIL, _RECEIVE_RANDOM_FAIL)
        Return MyBase.the_other_end(
                   Function(x, y) New mock_datagram_dev(Of _SEND_RANDOM_FAIL, _RECEIVE_RANDOM_FAIL)(x, y))
    End Function

    Private Shared Function random_fail() As Boolean
        Return rnd_bool_trues(3)
    End Function

    Public Overloads Function receive(ByVal result As pointer(Of Byte())) As event_comb Implements block_pump.receive
        Return sync_async(Function() As Boolean
                              If receive_random_fail AndAlso random_fail() Then
                                  Return False
                              Else
                                  Dim p As piece = Nothing
                                  If receive_q.pop(p) Then
                                      assert(p.size() <= packet_size)
                                  Else
                                      p = Nothing
                                  End If
                                  Return eva(result, p.export_or_null())
                              End If
                          End Function)
    End Function

    Public Overloads Function send(ByVal buff() As Byte,
                                   ByVal offset As UInt32,
                                   ByVal count As UInt32,
                                   ByVal sent As pointer(Of UInt32)) As event_comb Implements flow_injector.send
        Return sync_async(Function() As Boolean
                              If send_random_fail AndAlso random_fail() Then
                                  Return False
                              Else
                                  Dim p As piece = Nothing
                                  If piece.create(buff, offset, min(count, packet_size), p) Then
                                      Return assert(sync_send(p)) AndAlso
                                             eva(sent, p.count)
                                  Else
                                      Return False
                                  End If
                              End If
                          End Function)
    End Function
End Class
