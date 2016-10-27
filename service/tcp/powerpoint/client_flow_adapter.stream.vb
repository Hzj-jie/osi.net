
Imports System.IO
Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.transmitter

Partial Public Class client_flow_adapter
    Private Class client_stream_flow_adapter
        Implements flow

        Private ReadOnly c As TcpClient
        Private ReadOnly sf As stream_flow_adapter
        Private ReadOnly sensor As sensor

        Public Sub New(ByVal c As TcpClient, ByVal p As powerpoint)
            assert(Not c Is Nothing)
            assert(Not p Is Nothing)
            Me.c = c
            ' TODO: Cannot send header_data_increment to stream_flow_adapter.
            Me.sf = New stream_flow_adapter(c.stream(), p.send_rate_sec, p.receive_rate_sec, False)
            Me.sensor = client_sensor.stream_sensor(c, p)
        End Sub

        Public Function send(ByVal buff() As Byte,
                             ByVal offset As UInt32,
                             ByVal count As UInt32,
                             ByVal sent As pointer(Of UInt32)) As event_comb Implements flow.send
            Return sf.send(buff, offset, count, sent)
        End Function

        Public Function receive(ByVal buff() As Byte,
                                ByVal offset As UInt32,
                                ByVal count As UInt32,
                                ByVal result As pointer(Of UInt32)) As event_comb Implements flow.receive
            Return sf.receive(buff, offset, count, result)
        End Function

        Public Function sense(ByVal pending As pointer(Of Boolean),
                              ByVal timeout_ms As Int64) As event_comb Implements flow.sense
            Return sensor.sense(pending, timeout_ms)
        End Function
    End Class
End Class