
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.procedure
Imports osi.service.dns

Partial Public NotInheritable Class connector
    Private Function connect(ByVal add As IPAddress, ByVal r As ref(Of TcpClient)) As event_comb
        assert(add IsNot Nothing)
        assert(p IsNot Nothing)
        assert(p.is_outgoing)
        Dim ec As event_comb = Nothing
        Dim c As TcpClient = Nothing
        Dim accepted As ref(Of Boolean) = Nothing
        Return New event_comb(Function() As Boolean
                                  c = New TcpClient(p.address_family)
                                  c.set_no_delay(True)
                                  ec = _tcpclient.connect(c, New IPEndPoint(add, p.port))
                                  Return waitfor(ec, p.connecting_timeout_ms) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      accepted = New ref(Of Boolean)()
                                      ec = powerpoint.challenger.[New](p, c)(accepted)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      If tcp_trace Then
                                          raise_error(error_type.warning,
                                                      "failed to connect to ",
                                                      p.identity)
                                      End If
                                      c.shutdown()
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  assert(c IsNot Nothing)
                                  Return ec.end_result() AndAlso
                                         +accepted AndAlso
                                         eva(r, c) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Private Function connect(ByVal r As ref(Of TcpClient)) As event_comb
        assert(p IsNot Nothing)
        assert(p.is_outgoing)
        Dim ec As event_comb = Nothing
        Dim add As ref(Of IPAddress) = Nothing
        Return New event_comb(Function() As Boolean
                                  add = New ref(Of IPAddress)()
                                  ec = If(p.ipv4,
                                          dns_cache.resolve_ipv4(p.host_or_ip, add),
                                          dns_cache.resolve_ipv6(p.host_or_ip, add))
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() AndAlso (+add) IsNot Nothing Then
                                      ec = connect(+add, r)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      If tcp_trace Then
                                          raise_error(error_type.warning,
                                                      "failed to resolve host or ip ",
                                                      p.host_or_ip,
                                                      " to ip address for ",
                                                      p.identity)
                                      End If
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Private Function connect(ByVal r As ref(Of ref_client)) As event_comb
        Dim ec As event_comb = Nothing
        Dim c As ref(Of TcpClient) = Nothing
        Return New event_comb(Function() As Boolean
                                  c = New ref(Of TcpClient)()
                                  ec = connect(c)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         assert(Not c.empty()) AndAlso
                                         eva(r, New ref_client(p, +c)) AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
