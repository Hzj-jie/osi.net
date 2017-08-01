
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.transmitter

Public Class udp_bytes_dev
    Implements dev_T(Of pair(Of Byte(), const_pair(Of String, UInt16)))

    Private Class indicator
        Implements sync_indicator

        Private ReadOnly d As getter(Of UdpClient)

        Public Sub New(ByVal d As getter(Of UdpClient))
            assert(Not d Is Nothing)
            Me.d = d
        End Sub

        Public Function indicate(ByRef pending As Boolean) As Boolean Implements sync_indicator.indicate
            Dim u As UdpClient = Nothing
            If d.get(u) Then
                assert(Not u Is Nothing)
                Dim b As Int32 = 0
                b = u.buffered_bytes()
                If b < 0 Then
                    Return False
                Else
                    pending = (b > 0)
                    Return True
                End If
            Else
                Return False
            End If
        End Function
    End Class

    Private ReadOnly d As getter(Of UdpClient)
    Private ReadOnly s As sensor

    Public Sub New(ByVal d As getter(Of UdpClient))
        assert(Not d Is Nothing)
        Me.d = d
        Me.s = New indicator_sensor_adapter(New sync_indicator_indicator_adapter(New indicator(d)))
    End Sub

    Public Function receive(ByVal o As pointer(Of pair(Of Byte(), const_pair(Of String, UInt16)))) As event_comb _
                           Implements T_pump(Of pair(Of Byte(), const_pair(Of String, UInt16))).receive
        Dim ec As event_comb = Nothing
        Dim buff As pointer(Of Byte()) = Nothing
        Dim remote As pointer(Of IPEndPoint) = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim udp_client As UdpClient = Nothing
                                  If d.get(udp_client) Then
                                      assert(Not udp_client Is Nothing)
                                      buff = New pointer(Of Byte())()
                                      remote = New pointer(Of IPEndPoint)()
                                      ec = udp_client.receive(buff, remote)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         assert(Not buff.empty()) AndAlso
                                         assert(Not remote.empty()) AndAlso
                                         eva(o, emplace_make_pair(+buff, (+remote).to_string_const_pair())) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function send(ByVal i As pair(Of Byte(), const_pair(Of String, UInt16))) As event_comb _
                        Implements T_injector(Of pair(Of Byte(), const_pair(Of String, UInt16))).send
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If i Is Nothing OrElse i.second Is Nothing OrElse i.second.first Is Nothing Then
                                      Return False
                                  ElseIf isemptyarray(i.first) Then
                                      Return goto_end()
                                  Else
                                      Dim udp_client As UdpClient = Nothing
                                      If d.get(udp_client) Then
                                          assert(Not udp_client Is Nothing)
                                          ec = complete_io_3(i.first,
                                                             uint32_0,
                                                             array_size(i.first),
                                                             Function(ByVal buff() As Byte,
                                                                      ByVal count As UInt32,
                                                                      ByVal result As pointer(Of UInt32)) As event_comb
                                                                 Return udp_client.send(buff,
                                                                                        count,
                                                                                        i.second.first,
                                                                                        i.second.second,
                                                                                        result)
                                                             End Function)
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      Else
                                          Return False
                                      End If
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function sense(ByVal pending As pointer(Of Boolean), ByVal timeout_ms As Int64) As event_comb _
                         Implements sensor.sense
        Return s.sense(pending, timeout_ms)
    End Function
End Class
