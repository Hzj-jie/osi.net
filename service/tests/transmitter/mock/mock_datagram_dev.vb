
Imports osi.root.connector
Imports osi.root.template
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.transmitter

<type_attribute()>
Public Class mock_datagram_dev
    Inherits mock_datagram_dev(Of _false, _false)

    Shared Sub New()
        type_attribute.of(Of mock_datagram_dev)().forward_from(Of mock_datagram_dev(Of _false, _false))()
    End Sub

    Private Sub New(ByVal send_pump As slimqless2_event_sync_T_pump(Of piece),
                    ByVal receive_pump As slimqless2_event_sync_T_pump(Of piece))
        MyBase.New(send_pump, receive_pump)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Public Shadows Function the_other_end() As mock_datagram_dev
        Return New mock_datagram_dev(receive_pump, send_pump)
    End Function
End Class

<type_attribute()>
Public Class mock_datagram_dev(Of RANDOM_SEND_FAILURE As _boolean, RANDOM_RECEIVE_FAILURE As _boolean)
    Inherits mock_dev_T(Of piece, RANDOM_SEND_FAILURE, RANDOM_RECEIVE_FAILURE)
    Implements datagram

    Private Const packet_size As UInt32 = 1472

    Public NotInheritable Class packet_size_t
        Inherits _int64

        Protected Overrides Function at() As Int64
            Return packet_size
        End Function
    End Class

    Shared Sub New()
        type_attribute.of(Of mock_datagram_dev(Of RANDOM_SEND_FAILURE, RANDOM_RECEIVE_FAILURE))().set(
            datagram_transmitter.[New](
                osi.service.transmitter.transmitter _
                    .from_type(Of mock_dev_T(Of piece, RANDOM_SEND_FAILURE, RANDOM_RECEIVE_FAILURE))()) _
                    .with_packet_size(packet_size))
    End Sub

    Private ReadOnly datagram_dev As datagram

    Protected Sub New(ByVal send_pump As slimqless2_event_sync_T_pump(Of piece),
                      ByVal receive_pump As slimqless2_event_sync_T_pump(Of piece))
        MyBase.New(send_pump, receive_pump)
        datagram_dev = New dev_piece_datagram_adapter(Of packet_size_t)(Me)
    End Sub

    Public Sub New()
        Me.New(new_pump(), new_pump())
    End Sub

    Public Shadows Function the_other_end() As mock_datagram_dev(Of RANDOM_SEND_FAILURE, RANDOM_RECEIVE_FAILURE)
        Return New mock_datagram_dev(Of RANDOM_SEND_FAILURE, RANDOM_RECEIVE_FAILURE)(receive_pump, send_pump)
    End Function

    Public Overloads Function receive(ByVal result As pointer(Of Byte())) As event_comb Implements block_pump.receive
        Return datagram_dev.receive(result)
    End Function

    Public Overloads Function send(ByVal buff() As Byte,
                                   ByVal offset As UInt32,
                                   ByVal count As UInt32,
                                   ByVal sent As pointer(Of UInt32)) As event_comb Implements flow_injector.send
        Return datagram_dev.send(buff, offset, count, sent)
    End Function
End Class
