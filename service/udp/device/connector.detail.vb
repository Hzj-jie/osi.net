
Imports System.Net
Imports System.Net.Sockets
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.dns

Partial Public NotInheritable Class connector
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

    Private Function connect(ByVal r As pointer(Of ref_client)) As event_comb
        assert(Not p Is Nothing)
        Dim ec As event_comb = Nothing
        Dim result As pointer(Of IPHostEntry) = Nothing
        Return New event_comb(Function() As Boolean
                                  If String.IsNullOrEmpty(p.host_or_ip) Then
                                      Return eva(r, New ref_client(Nothing, new_client(Nothing))) AndAlso
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
                                      Return eva(r, New ref_client(eps, new_client(selected_remote_host), p)) AndAlso
                                             goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function
End Class
