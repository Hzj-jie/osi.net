
Imports System.Net
Imports System.Net.Sockets
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

' Delegates all outgoing requests
Public Class speaker
    Private ReadOnly c As UdpClient

    Public Shared Function [New](ByVal p As powerpoint, ByRef o As speaker) As Boolean
        Dim c As UdpClient = Nothing
        If udp_clients.[New](p, c) Then
            o = New speaker(c)
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function [New](ByVal p As powerpoint) As speaker
        Dim o As speaker = Nothing
        assert([New](p, o))
        Return o
    End Function

    ' Test only
    Public Sub New(ByVal c As UdpClient)
        assert(Not c Is Nothing)
        Me.c = c
    End Sub

    Public Function send(ByVal remote As IPEndPoint,
                         ByVal b() As Byte,
                         ByVal offset As UInt32,
                         ByVal count As UInt32,
                         Optional ByVal sent As pointer(Of UInt32) = Nothing) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim p As piece = Nothing
                                  If piece.create(b, offset, count, p) Then
                                      b = p.export(count)
                                      ec = c.send(b,
                                                  min(count, If(c.Client().ipv4(),
                                                                constants.ipv4_packet_size,
                                                                constants.ipv6_packet_size)),
                                                  remote,
                                                  sent)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function send(ByVal remote As IPEndPoint,
                         ByVal b() As Byte,
                         ByVal count As UInt32,
                         Optional ByVal sent As pointer(Of UInt32) = Nothing) As event_comb
        Return send(remote, b, uint32_0, count, sent)
    End Function

    Public Function send(ByVal remote As IPEndPoint,
                         ByVal b() As Byte,
                         Optional ByVal sent As pointer(Of UInt32) = Nothing) As event_comb
        Return send(remote, b, array_size(b), sent)
    End Function

    Public Function send(ByVal remote_host As IPAddress,
                         ByVal remote_port As UInt16,
                         ByVal b() As Byte,
                         ByVal offset As UInt32,
                         ByVal count As UInt32,
                         Optional ByVal sent As pointer(Of UInt32) = Nothing) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If remote_host Is Nothing Then
                                      Return False
                                  Else
                                      ec = send(New IPEndPoint(remote_host, remote_port),
                                                b,
                                                offset,
                                                count,
                                                sent)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function send(ByVal remote_host As IPAddress,
                         ByVal remote_port As UInt16,
                         ByVal b() As Byte,
                         ByVal count As UInt32,
                         Optional ByVal sent As pointer(Of UInt32) = Nothing) As event_comb
        Return send(remote_host, remote_port, b, 0, count, sent)
    End Function

    Public Function send(ByVal remote_host As IPAddress,
                         ByVal remote_port As UInt16,
                         ByVal b() As Byte,
                         Optional ByVal sent As pointer(Of UInt32) = Nothing) As event_comb
        Return send(remote_host, remote_port, b, array_size(b), sent)
    End Function

    Public Function send(ByVal remote_host_or_ip As String,
                         ByVal remote_port As UInt16,
                         ByVal b() As Byte,
                         ByVal offset As UInt32,
                         ByVal count As UInt32,
                         Optional ByVal sent As pointer(Of UInt32) = Nothing) As event_comb
        Dim ec As event_comb = Nothing
        Dim p As pointer(Of IPAddress) = Nothing
        Return New event_comb(Function() As Boolean
                                  p = New pointer(Of IPAddress)()
                                  If c.Client().ipv4() Then
                                      ec = dns.resolve_ipv4(remote_host_or_ip, p)
                                  Else
                                      assert(c.Client().ipv6())
                                      ec = dns.resolve_ipv6(remote_host_or_ip, p)
                                  End If
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() AndAlso Not p.empty() Then
                                      ec = send(+p, remote_port, b, offset, count, sent)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function send(ByVal remote_host_or_ip As String,
                         ByVal remote_port As UInt16,
                         ByVal b() As Byte,
                         ByVal count As UInt32,
                         Optional ByVal sent As pointer(Of UInt32) = Nothing) As event_comb
        Return send(remote_host_or_ip, remote_port, b, 0, count, sent)
    End Function

    Public Function send(ByVal remote_host_or_ip As String,
                         ByVal remote_port As UInt16,
                         ByVal b() As Byte,
                         Optional ByVal sent As pointer(Of UInt32) = Nothing) As event_comb
        Return send(remote_host_or_ip, remote_port, b, array_size(b), sent)
    End Function
End Class
