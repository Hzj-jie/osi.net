
Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.device
Imports osi.service.selector

Partial Public Class client_flow_adapter
    Private Class client_socket_flow_adapter
        Implements flow

        Private ReadOnly c As TcpClient
        Private ReadOnly single_send_size As UInt32
        Private ReadOnly sensor As sensor
        Private ReadOnly timeout As transceive_timeout

        Public Sub New(ByVal c As TcpClient, ByVal p As powerpoint)
            assert(Not c Is Nothing)
            assert(Not p Is Nothing)
            Me.c = c
            Me.sensor = client_sensor.socket_sensor(c, p)
            Me.single_send_size = c.send_buff_size()
            Me.timeout = p.transceive_timeout()
        End Sub

        'connection_flow_adapter will take care of overall timeout and shutting down connection
        Public Function send(ByVal buff() As Byte,
                             ByVal offset As UInt32,
                             ByVal count As UInt32,
                             ByVal sent As pointer(Of UInt32)) As event_comb Implements flow.send
            Return sync_async(Function() As Boolean
                                  If Not c.alive() OrElse
                                     single_send_size = 0 OrElse
                                     array_size(buff) < offset + count Then
                                      Return False
                                  ElseIf count = 0 Then
                                      Return True
                                  ElseIf c.poll_write() Then
                                      Dim size As UInt32 = 0
                                      size = min(count, single_send_size)
                                      assert(size > 0)
                                      Try
                                          assert(c.Client().Send(buff, offset, size, SocketFlags.None) = size)
                                      Catch
                                          Return False
                                      End Try
                                      timeout.update_send(size)
                                      Return eva(sent, size)
                                  Else
                                      Return Not timeout.send_timeout() AndAlso eva(sent, 0)
                                  End If
                              End Function)
        End Function

        Public Function receive(ByVal buff() As Byte,
                                ByVal offset As UInt32,
                                ByVal count As UInt32,
                                ByVal result As pointer(Of UInt32)) As event_comb Implements flow.receive
            Return sync_async(Function() As Boolean
                                  If Not c.alive() Then
                                      Return False
                                  ElseIf array_size(buff) < offset + count Then
                                      Return False
                                  ElseIf count = 0 Then
                                      Return True
                                  Else
                                      Dim b As Int32 = 0
                                      b = c.buffered_bytes()
                                      If b < 0 Then
                                          Return False
                                      ElseIf b > 0 Then
                                          Dim size As UInt32 = 0
                                          size = min(count, CUInt(b))
                                          assert(size > 0)
                                          Try
                                              assert(c.Client().Receive(buff, offset, size, SocketFlags.None) = size)
                                          Catch
                                              Return False
                                          End Try
                                          timeout.update_receive(size)
                                          Return eva(result, size)
                                      Else
                                          Return Not timeout.receive_timeout() AndAlso eva(result, 0)
                                      End If
                                  End If
                              End Function)
        End Function

        Public Function sense(ByVal pending As pointer(Of Boolean),
                              ByVal timeout_ms As Int64) As event_comb Implements flow.sense
            Return sensor.sense(pending, timeout_ms)
        End Function
    End Class
End Class
