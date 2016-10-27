
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.service.transmitter

<type_attribute()>
Public Class mock_flow_dev
    Implements flow

    Shared Sub New()
        type_attribute.of(Of mock_flow_dev)().set(osi.service.transmitter.transmitter.[New]().
            with_transmit_mode(osi.service.transmitter.transmitter.mode_t.duplex))
    End Sub

    Private ReadOnly receive_buff() As Byte
    Private ReadOnly send_buff() As Byte
    Private ReadOnly random_zero As Boolean
    Private ReadOnly random_failure As Boolean
    Private receive_position As UInt32
    Private send_position As UInt32

    Public Sub New(ByVal read_buff() As Byte,
                   ByVal write_buff_size As UInt32,
                   ByVal random_zero As Boolean,
                   ByVal random_failure As Boolean)
        Me.receive_buff = read_buff
        ReDim Me.send_buff(write_buff_size - 1)
        Me.random_zero = random_zero
        Me.random_failure = random_failure
        Me.receive_position = 0
        Me.send_position = 0
    End Sub

    Public Sub New(ByVal write_buff_size As UInt32,
                   ByVal random_zero As Boolean,
                   ByVal random_failure As Boolean)
        Me.New(next_bytes(rnd_int(4096, 8192)),
               write_buff_size,
               random_zero,
               random_failure)
    End Sub

    Public Function receive_consistent(ByVal receive_buff() As Byte) As Boolean
        Return memcmp(receive_buff, Me.receive_buff) = 0
    End Function

    Public Function send_consistent(ByVal send_buff() As Byte) As Boolean
        Return memcmp(send_buff, Me.send_buff) = 0
    End Function

    Public Function send(ByVal buff() As Byte,
                         ByVal offset As UInt32,
                         ByVal count As UInt32,
                         ByVal sent As pointer(Of UInt32)) As event_comb Implements flow.send
        Return sync_async(Function() As Boolean
                              If random_zero AndAlso
                                 rnd_bool_trues(3) Then
                                  Return eva(sent, uint32_0)
                              ElseIf random_failure AndAlso
                                     rnd_bool_trues(3) Then
                                  If rnd_bool() Then
                                      Return False
                                  Else
                                      Return eva(sent, uint32_0)
                                  End If
                              ElseIf array_size(buff) < offset + count Then
                                  Return False
                              Else
                                  Dim s As Int32 = 0
                                  s = min(count, array_size(Me.send_buff) - Me.send_position)
                                  assert(s >= 0)
                                  If s > 0 Then
                                      memcpy(Me.send_buff, Me.send_position, buff, offset, s)
                                      Me.send_position += s
                                  End If
                                  Return eva(sent, s)
                              End If
                          End Function)
    End Function

    Public Function receive(ByVal buff() As Byte,
                            ByVal offset As UInt32,
                            ByVal count As UInt32,
                            ByVal result As pointer(Of UInt32)) As event_comb Implements flow.receive
        Return sync_async(Function() As Boolean
                              If random_zero AndAlso
                                 rnd_bool_trues(3) Then
                                  Return eva(result, uint32_0)
                              ElseIf random_failure AndAlso
                                     rnd_bool_trues(3) Then
                                  If rnd_bool() Then
                                      Return False
                                  Else
                                      Return eva(result, uint32_0)
                                  End If
                              ElseIf array_size(buff) < offset + count Then
                                  Return False
                              Else
                                  Dim s As Int32 = 0
                                  s = min(count, array_size(Me.receive_buff) - Me.receive_position)
                                  assert(s >= 0)
                                  If s > 0 Then
                                      memcpy(buff, offset, Me.receive_buff, Me.receive_position, s)
                                      Me.receive_position += s
                                  End If
                                  Return eva(result, s)
                              End If
                          End Function)
    End Function

    Public Function sense(ByVal pending As pointer(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements flow.sense
        Return sync_async(Sub()
                              eva(pending, receive_position < array_size(receive_buff))
                          End Sub)
    End Function
End Class
