
Imports System.Net
Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.device

' Delegates all incoming requests to accepters
Partial Public Class listener
    Implements T_receiver(Of pair(Of Byte(), IPEndPoint))

    Private ReadOnly s As sensor
    Private ReadOnly c As UdpClient

    Public Sub New(ByVal p As powerpoint)
        MyBase.New()
        assert(Not p Is Nothing)
        Me.c = New UdpClient(p.local_port, p.address_family)
        Me.s = as_sensor(Function(ByRef pending As Boolean) As Boolean
                             Dim b As Int32 = 0
                             b = c.buffered_bytes()
                             pending = (b > 0)
                             Return (b >= 0)
                         End Function)
    End Sub

    Public Function sense(ByVal pending As pointer(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements sensor.sense
        Return s.sense(pending, timeout_ms)
    End Function

    Public Function receive(ByVal o As pointer(Of pair(Of Byte(), IPEndPoint))) As event_comb Implements _
                           T_pump(Of pair(Of Byte(), IPEndPoint)).receive
        Dim ec As event_comb = Nothing
        Dim result As pointer(Of Byte()) = Nothing
        Dim source As pointer(Of IPEndPoint) = Nothing
        Return New event_comb(Function() As Boolean
                                  result.renew()
                                  source.renew()
                                  ec = c.receive(result, source)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() AndAlso Not result.empty() AndAlso Not source.empty() Then
                                      Return eva(o, emplace_make_pair(+result, +source)) AndAlso
                                             goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function
End Class
