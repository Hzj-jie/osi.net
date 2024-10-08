
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
Imports osi.service.dns

Partial Public NotInheritable Class connector
    Public Shared Function [New](ByVal address_family As AddressFamily,
                                 ByVal local_port As UInt16,
                                 ByRef c As UdpClient) As Boolean
        Try
            If local_port = socket_invalid_port Then
                c = New UdpClient(address_family)
            Else
                c = New UdpClient(local_port, address_family)
            End If
            If osi.root.envs.udp_trace Then
                raise_error("new udp [", address_family, "] connection has been created on local port ",
                            local_port)
            End If
            Return True
        Catch ex As Exception
            raise_error(error_type.user,
                        "failed to create udp client on port ",
                        local_port,
                        " with address family ",
                        address_family,
                        ", ex ",
                        ex.Message())
            Return False
        End Try
    End Function

    Public Shared Function [New](ByVal p As powerpoint, ByVal local_port As UInt16, ByRef c As UdpClient) As Boolean
        If p Is Nothing Then
            Return False
        Else
            Return [New](p.address_family, local_port, c)
        End If
    End Function

    Public Shared Function [New](ByVal p As powerpoint, ByRef c As UdpClient) As Boolean
        Return [New](p, p.local_port, c)
    End Function

    Public Shared Function connect(ByVal p As powerpoint) As UdpClient
        Dim c As UdpClient = Nothing
        assert([New](p, c))
        Return c
    End Function

    ' remote_host is one of the resolved address (usually the first one which matches ipv4 setting), or a null.
    Private Function new_client(ByVal remote_host As IPAddress) As UdpClient
        assert(Not p Is Nothing)
        Dim r As UdpClient = Nothing
        If p.local_port = socket_invalid_port Then
            r = New UdpClient(p.address_family)
        Else
            r = New UdpClient(p.local_port, p.address_family)
        End If
        If Not remote_host Is Nothing AndAlso p.remote_port <> socket_invalid_port Then
            r.Connect(New IPEndPoint(remote_host, p.remote_port))
        End If
        Return r
    End Function

    Private Function connect(ByRef r As UdpClient) As Boolean
        Return [New](p, r)
    End Function

    Public Function sources(ByVal result As vector(Of IPEndPoint)) As event_comb
        Dim e As ref(Of IPHostEntry) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  result.clear()
                                  If p.host_or_ip.null_or_empty() Then
                                      Dim r As vector(Of IPEndPoint) = Nothing
                                      r = New vector(Of IPEndPoint)()
                                      r.emplace_back({New IPEndPoint(IPAddress.Any, p.remote_port),
                                                      New IPEndPoint(IPAddress.IPv6Any, p.remote_port)})
                                      Return eva(result, r) AndAlso
                                             goto_end()
                                  Else
                                      e = New ref(Of IPHostEntry)()
                                      ec = dns_cache.resolve(p.host_or_ip, e, p.response_timeout_ms)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() AndAlso
                                     Not e.empty() AndAlso
                                     Not isemptyarray((+e).AddressList()) Then
                                      For i As Int32 = 0 To array_size_i((+e).AddressList()) - 1
                                          result.emplace_back(New IPEndPoint((+e).AddressList()(i), p.remote_port))
                                      Next
                                      Return goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function multiple_accepter(ByVal o As ref(Of listener.multiple_accepter)) As event_comb
        Dim e As vector(Of IPEndPoint) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  e = New vector(Of IPEndPoint)()
                                  ec = sources(e)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      Return eva(o, New listener.multiple_accepter(const_array.of(+e))) AndAlso
                                             goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function udp_dev(ByVal o As ref(Of udp_dev)) As event_comb
        Dim e As vector(Of IPEndPoint) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  e = New vector(Of IPEndPoint)()
                                  ec = sources(e)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      Return eva(o, New udp_dev(p, const_array.of(+e))) AndAlso
                                             goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function connect(ByVal r As ref(Of delegator)) As event_comb
        assert(Not p Is Nothing)
        Dim ec As event_comb = Nothing
        Dim result As ref(Of IPHostEntry) = Nothing
        Return New event_comb(Function() As Boolean
                                  If p.host_or_ip.null_or_empty() Then
                                      Return eva(r, New delegator(Nothing, new_client(Nothing))) AndAlso
                                             goto_end()
                                  End If
                                  result = New ref(Of IPHostEntry)()
                                  ec = dns_cache.resolve(p.host_or_ip, result, p.response_timeout_ms)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If Not ec.end_result() OrElse result.empty() Then
                                      Return False
                                  End If
                                  Dim eps(array_size_i((+result).AddressList()) - 1) As IPEndPoint
                                  Dim selected_remote_host As IPAddress = Nothing
                                  For i As Int32 = 0 To array_size_i((+result).AddressList()) - 1
                                      If Not (+result).AddressList()(i) Is Nothing Then
                                          eps(i) = New IPEndPoint((+result).AddressList()(i), p.remote_port)
                                          If selected_remote_host Is Nothing AndAlso
                                             (+result).AddressList()(i).AddressFamily() = p.address_family Then
                                              selected_remote_host = (+result).AddressList()(i)
                                          End If
                                      End If
                                  Next
                                  If selected_remote_host Is Nothing AndAlso
                                     Not isemptyarray((+result).AddressList()) Then
                                      selected_remote_host = (+result).AddressList()(0)
                                  End If
                                  Return eva(r, New delegator(eps, new_client(selected_remote_host), p)) AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
