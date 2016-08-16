
Imports System.Net
Imports System.Net.Sockets
Imports osi.root.constants
Imports osi.root.connector
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

    Public Shared Function [New](ByVal p As powerpoint, ByRef c As UdpClient) As Boolean
        If p Is Nothing Then
            Return False
        Else
            Return [New](p.address_family, p.local_port, c)
        End If
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

    Private Function connect(ByVal r As pointer(Of delegator)) As event_comb
        assert(Not p Is Nothing)
        Dim ec As event_comb = Nothing
        Dim result As pointer(Of IPHostEntry) = Nothing
        Return New event_comb(Function() As Boolean
                                  If String.IsNullOrEmpty(p.host_or_ip) Then
                                      Return eva(r, New delegator(Nothing, new_client(Nothing))) AndAlso
                                             goto_end()
                                  Else
                                      result = New pointer(Of IPHostEntry)()
                                      ec = dns.resolve(p.host_or_ip, result, p.response_timeout_ms)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() AndAlso Not result.empty() Then
                                      Dim eps() As IPEndPoint = Nothing
                                      ReDim eps(array_size((+result).AddressList()) - 1)
                                      Dim selected_remote_host As IPAddress = Nothing
                                      For i As Int32 = 0 To array_size((+result).AddressList()) - 1
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
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function
End Class
