
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.cache
Imports osi.service.dns.constants

Partial Public NotInheritable Class dns_cache
    Private Shared ReadOnly h2ip As icache2(Of String, IPHostEntry)
    Private Shared ReadOnly ip2h As icache2(Of String, IPHostEntry)

    Shared Sub New()
        default_init()
        mapheap_cache2(h2ip, max_size, retire_ticks, False)
        mapheap_cache2(ip2h, max_size, retire_ticks, False)
    End Sub

    Public Shared Sub clear_cache()
        assert_begin(h2ip.clear())
        assert_begin(ip2h.clear())
    End Sub

    Public Shared Function query_host_to_ip_cache(ByVal s As String,
                                                  ByVal result As ref(Of IPHostEntry)) As event_comb
        Return h2ip.get(s, result)
    End Function

    Public Shared Function query_ip_to_host_cache(ByVal s As String,
                                                  ByVal result As ref(Of IPHostEntry)) As event_comb
        Return ip2h.get(s, result)
    End Function

    Private Shared Function operate(ByVal s As String,
                                    ByVal result As ref(Of IPHostEntry),
                                    ByVal c As icache2(Of String, IPHostEntry),
                                    ByVal timeout_ms As Int64,
                                    ByVal force_resolve As Boolean) As event_comb
        assert(Not c Is Nothing)
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If String.IsNullOrEmpty(s) Then
                                      Return False
                                  End If
                                  Dim add As IPAddress = Nothing
                                  If Not force_resolve AndAlso
                                     IPAddress.TryParse(s, add) Then
                                      Dim he As IPHostEntry = Nothing
                                      he = New IPHostEntry()
                                      he.AddressList() = {add}
                                      Return eva(result, he) AndAlso
                                             goto_end()
                                  End If
                                  result.renew()
                                  ec = c.get(s, result)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      Return goto_end()
                                  End If
                                  ec = get_host_entry(s, result)
                                  Return waitfor(ec, timeout_ms) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If Not ec.end_result() OrElse
                                     result.empty() Then
                                      raise_error(error_type.warning, "failed to resolve name ", s, " to ip")
                                      Return False
                                  End If
                                  assert_begin(c.set(s, +result))
                                  assert_begin(h2ip.set((+result).HostName(), +result))
                                  For i As Int32 = 0 To array_size_i((+result).Aliases()) - 1
                                      assert_begin(h2ip.set((+result).Aliases()(i), (+result)))
                                  Next
                                  For i As Int32 = 0 To array_size_i((+result).AddressList()) - 1
                                      assert_begin(ip2h.set(Convert.ToString((+result).AddressList()(i)), (+result)))
                                  Next
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function ipv4_selector(ByVal address As IPAddress) As Boolean
        Return Not address Is Nothing AndAlso address.AddressFamily() = Sockets.AddressFamily.InterNetwork
    End Function

    Private Shared Function ipv6_selector(ByVal address As IPAddress) As Boolean
        Return Not address Is Nothing AndAlso address.AddressFamily() = Sockets.AddressFamily.InterNetworkV6
    End Function

    Public Shared Function resolve(ByVal hostname_or_address As String,
                                   ByVal result As ref(Of IPHostEntry),
                                   Optional ByVal timeout_ms As Int64 = default_resolve_timeout_ms) As event_comb
        Return operate(hostname_or_address, result, h2ip, timeout_ms, False)
    End Function

    Public Shared Function resolve(ByVal hostname_or_address As String,
                                   ByVal result As ref(Of IPAddress),
                                   Optional ByVal timeout_ms As Int64 = default_resolve_timeout_ms,
                                   Optional ByVal selector As Func(Of IPAddress, Boolean) = Nothing) As event_comb
        Dim p As ref(Of IPHostEntry) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  p = New ref(Of IPHostEntry)()
                                  ec = resolve(hostname_or_address, p, timeout_ms)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      assert(Not +p Is Nothing)
                                      For i As Int32 = 0 To array_size_i((+p).AddressList()) - 1
                                          If selector Is Nothing OrElse selector((+p).AddressList()(i)) Then
                                              Return eva(result, (+p).AddressList()(i)) AndAlso
                                                     goto_end()
                                          End If
                                      Next
                                  End If
                                  Return False
                              End Function)
    End Function

    Public Shared Function resolve_ipv4(ByVal hostname_or_address As String,
                                        ByVal result As ref(Of IPAddress),
                                        Optional ByVal timeout_ms As Int64 = default_resolve_timeout_ms) As event_comb
        Return resolve(hostname_or_address, result, timeout_ms, AddressOf ipv4_selector)
    End Function

    Public Shared Function resolve_ipv6(ByVal hostname_or_address As String,
                                        ByVal result As ref(Of IPAddress),
                                        Optional ByVal timeout_ms As Int64 = default_resolve_timeout_ms) As event_comb
        Return resolve(hostname_or_address, result, timeout_ms, AddressOf ipv6_selector)
    End Function

    Public Shared Function resolve(ByVal hostname_or_address As String,
                                   ByVal result As ref(Of String),
                                   Optional ByVal timeout_ms As Int64 = default_resolve_timeout_ms,
                                   Optional ByVal selector As Func(Of IPAddress, Boolean) = Nothing) As event_comb
        Dim p As ref(Of IPAddress) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  p = New ref(Of IPAddress)()
                                  ec = resolve(hostname_or_address, p, timeout_ms, selector)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      assert(Not +p Is Nothing)
                                      Return eva(result, Convert.ToString(+p)) AndAlso
                                             goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Shared Function resolve_ipv4(ByVal hostname_or_address As String,
                                        ByVal result As ref(Of String),
                                        Optional ByVal timeout_ms As Int64 = default_resolve_timeout_ms) As event_comb
        Return resolve(hostname_or_address, result, timeout_ms, AddressOf ipv4_selector)
    End Function

    Public Shared Function resolve_ipv6(ByVal hostname_or_address As String,
                                        ByVal result As ref(Of String),
                                        Optional ByVal timeout_ms As Int64 = default_resolve_timeout_ms) As event_comb
        Return resolve(hostname_or_address, result, timeout_ms, AddressOf ipv6_selector)
    End Function

    Public Shared Function reverse_lookup(ByVal address As String,
                                          ByVal result As ref(Of IPHostEntry),
                                          Optional ByVal timeout_ms As Int64 = default_resolve_timeout_ms) As event_comb
        Return operate(address, result, ip2h, timeout_ms, True)
    End Function

    Public Shared Function reverse_lookup(ByVal address As IPAddress,
                                          ByVal result As ref(Of IPHostEntry),
                                          Optional ByVal timeout_ms As Int64 = default_resolve_timeout_ms) As event_comb
        Return reverse_lookup(Convert.ToString(address), result, timeout_ms)
    End Function

    Public Shared Function reverse_lookup(ByVal address As String,
                                          ByVal result As ref(Of String),
                                          Optional ByVal timeout_ms As Int64 = default_resolve_timeout_ms,
                                          Optional ByVal selector As Func(Of String, Boolean) = Nothing) As event_comb
        Dim p As ref(Of IPHostEntry) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  p = New ref(Of IPHostEntry)()
                                  ec = reverse_lookup(address, p, timeout_ms)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      assert(Not +p Is Nothing)
                                      If selector Is Nothing OrElse selector((+p).HostName()) Then
                                          Return eva(result, (+p).HostName()) AndAlso
                                                 goto_end()
                                      End If
                                      assert(Not selector Is Nothing)
                                      For i As Int32 = 0 To array_size_i((+p).Aliases()) - 1
                                          If selector((+p).Aliases()(i)) Then
                                              Return eva(result, (+p).Aliases()(i)) AndAlso
                                                     goto_end()
                                          End If
                                      Next
                                  End If
                                  Return False
                              End Function)
    End Function

    Public Shared Function reverse_lookup(ByVal address As IPAddress,
                                          ByVal result As ref(Of String),
                                          Optional ByVal timeout_ms As Int64 = default_resolve_timeout_ms,
                                          Optional ByVal selector As Func(Of String, Boolean) = Nothing) As event_comb
        Return reverse_lookup(Convert.ToString(address), result, timeout_ms, selector)
    End Function
End Class
