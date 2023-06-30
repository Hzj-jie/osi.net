
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.transmitter

Partial Public Class listener
    Private Shadows Class receiver
        Implements T_receiver(Of pair(Of Byte(), IPEndPoint))

        Private ReadOnly s As sensor
        Private ReadOnly c As UdpClient

        Public Shared Function [New](ByVal p As powerpoint, ByVal local_port As UInt16, ByRef o As receiver) As Boolean
            Dim c As UdpClient = Nothing
            If udp_clients.[New](p, local_port, c) Then
                o = New receiver(c)
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function [New](ByVal p As powerpoint, ByVal local_port As UInt16) As receiver
            Dim o As receiver = Nothing
            assert([New](p, local_port, o))
            Return o
        End Function

        Public Shared Function [New](ByVal p As powerpoint, ByRef o As receiver) As Boolean
            Dim c As UdpClient = Nothing
            If udp_clients.[New](p, c) Then
                o = New receiver(c)
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function [New](ByVal p As powerpoint) As receiver
            Dim o As receiver = Nothing
            assert([New](p, o))
            Return o
        End Function

        Public Sub New(ByVal c As UdpClient)
            MyBase.New()
            assert(Not c Is Nothing)
            Me.c = c
            Me.s = as_sensor(Function(ByRef pending As Boolean) As Boolean
                                 Dim b As Int32 = 0
                                 b = c.buffered_bytes()
                                 pending = (b > 0)
                                 Return (b >= 0)
                             End Function)
        End Sub

        Public Function sense(ByVal pending As ref(Of Boolean),
                              ByVal timeout_ms As Int64) As event_comb Implements sensor.sense
            Return s.sense(pending, timeout_ms)
        End Function

        Public Function receive(ByVal o As ref(Of pair(Of Byte(), IPEndPoint))) As event_comb Implements _
                               T_pump(Of pair(Of Byte(), IPEndPoint)).receive
            Dim ec As event_comb = Nothing
            Dim result As ref(Of Byte()) = Nothing
            Dim source As ref(Of IPEndPoint) = Nothing
            Return New event_comb(Function() As Boolean
                                      result.renew()
                                      source.renew()
                                      ec = c.receive(result, source)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      If ec.end_result() AndAlso Not result.empty() AndAlso Not source.empty() Then
                                          Return eva(o, pair.emplace_of(+result, +source)) AndAlso
                                                 goto_end()
                                      Else
                                          Return False
                                      End If
                                  End Function)
        End Function
    End Class
End Class
