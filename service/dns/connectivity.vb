
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

Public NotInheritable Class connectivity
    Public Enum result_t
        fail = 0                       ' No internet access
        partial_dns_resolvable = 1     ' Some golden_hosts can be resolved, but none of them can be accessed
        dns_resolvable = 2             ' All golden_hosts can be resolved, but none of them can be accessed
        partial_accessible = 3         ' Some or all golden_hosts can be resolved, but only some of them can be accessed
        accessible = 4                 ' All golden_hosts can be resolved and accessed
    End Enum

    Public Shared ReadOnly golden_hosts As const_array(Of String)
    Private Shared l_result As result_t
    Private Shared l_ms As Int64

    Shared Sub New()
        ' Except for the example hosts, other six are top six sites analyzed by alexa @ http://www.alexa.com/topsites.
        ' Latest updated on July, 23rd, 2016 09:58 (AM).
        golden_hosts = const_array.[New]({"example.org",
                                          "example.com",
                                          "example.net",
                                          "www.example.org",
                                          "www.example.com",
                                          "www.example.net",
                                          "www.google.com",
                                          "www.youtube.com",
                                          "www.facebook.com",
                                          "www.baidu.com",
                                          "www.yahoo.com",
                                          "www.wikipedia.org"})

        ServicePointManager.Expect100Continue() = True
        For Each i As SecurityProtocolType In {SecurityProtocolType.Tls,
                                               SecurityProtocolType.Ssl3,
                                               direct_cast(Of SecurityProtocolType)(768),     ' tls11
                                               direct_cast(Of SecurityProtocolType)(3072),    ' tls12
                                               direct_cast(Of SecurityProtocolType)(12288)}   ' tls13
            Try
                ServicePointManager.SecurityProtocol() = ServicePointManager.SecurityProtocol() Or i
            Catch ex As NotSupportedException
                raise_error(error_type.warning, "Unsupported security protocol ", i, ", ex ", ex)
            End Try
        Next
    End Sub

    Public Shared Function check_if_needed(ByVal result As pointer(Of result_t),
                                           Optional last_result_expiration_ms As Int64 =
                                               constants.default_connectivity_expiration_time_ms) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If nowadays.low_res_milliseconds() - last_check_ms() >= last_result_expiration_ms Then
                                      ec = check(result)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                                  Return eva(result, last_check_result()) AndAlso
                                             goto_end()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Shared Function check(ByVal result As pointer(Of result_t)) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  result.renew()
                                  ec = check(result, golden_hosts)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If Not ec.end_result() Then
                                      Return False
                                  End If
                                  l_result = +result
                                  l_ms = nowadays.milliseconds()
                                  Return goto_end()
                              End Function)
    End Function

    Public Shared Function check(ByVal result As pointer(Of result_t),
                                 ByVal hosts As const_array(Of String)) As event_comb
        Dim r() As pointer(Of Boolean) = Nothing
        Dim a() As pointer(Of Boolean) = Nothing
        Dim ecs() As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If hosts.null_or_empty() Then
                                      Return eva(result, result_t.fail) AndAlso
                                             goto_end()
                                  End If
                                  ReDim r(CInt(hosts.size()) - 1)
                                  ReDim a(CInt(hosts.size()) - 1)
                                  ReDim ecs(CInt(hosts.size()) - 1)
                                  For i As Int32 = 0 To CInt(hosts.size()) - 1
                                      r(i).renew()
                                      a(i).renew()
                                      ecs(i) = check_host(hosts(CUInt(i)), r(i), a(i))
                                  Next
                                  Return waitfor(ecs) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Dim r_at As Boolean = False
                                  Dim r_af As Boolean = False
                                  Dim a_at As Boolean = False
                                  Dim a_af As Boolean = False
                                  analyze(r, r_at, r_af)
                                  analyze(a, a_at, a_af)
                                  Dim res As result_t = result_t.fail
                                  If r_at Then
                                      If a_at Then
                                          res = result_t.accessible
                                      Else
                                          res = result_t.partial_accessible
                                      End If
                                  ElseIf r_af Then
                                      assert(a_af)
                                      res = result_t.fail
                                  Else
                                      assert(Not a_at)
                                      If a_af Then
                                          res = result_t.partial_dns_resolvable
                                      Else
                                          res = result_t.partial_accessible
                                      End If
                                  End If
                                  Return eva(result, res) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Private Shared Sub analyze(ByVal r() As pointer(Of Boolean), ByRef at As Boolean, ByRef af As Boolean)
        at = True
        af = True
        For i As Int32 = 0 To array_size_i(r) - 1
            If +r(i) Then
                af = False
                If Not at Then
                    Return
                End If
            Else
                at = False
                If Not af Then
                    Return
                End If
            End If
        Next
    End Sub

    Private Shared Function check_host(ByVal host As String,
                                       ByVal dns_resolve_result As pointer(Of Boolean),
                                       ByVal access_result As pointer(Of Boolean)) As event_comb
        assert(Not dns_resolve_result Is Nothing)
        assert(Not access_result Is Nothing)
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  dns_resolve_result.set(False)
                                  access_result.set(False)
                                  ec = get_host_entry(host, Nothing)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If Not ec.end_result() Then
                                      raise_error(error_type.warning, "Failed to resolve DNS of ", host)
                                      Return goto_end()
                                  End If
                                  dns_resolve_result.set(True)
                                  ec = Net.WebRequest.Create(strcat("http://", host)).get_response(
                                           New pointer(Of Net.WebResponse)())
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If Not ec.end_result() Then
                                      raise_error(error_type.warning, "Failed to request http host of ", host)
                                  End If
                                  Return eva(access_result, ec.end_result()) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Shared Function last_check_result() As result_t
        Return l_result
    End Function

    Public Shared Function last_check_ms() As Int64
        Return l_ms
    End Function

    Private Sub New()
    End Sub
End Class
