
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.dns

Public NotInheritable Class dns_cache_test
    Inherits [case]

    Public Overrides Function prepare() As Boolean
        If Not MyBase.prepare() Then
            Return False
        End If
        Dim p As ref(Of connectivity.result_t) = Nothing
        p.renew()
        async_sync(connectivity.check_if_needed(p))
        dns_cache.clear_cache()
        Return (+p) >= connectivity.result_t.partial_dns_resolvable
    End Function

    Private Shared Function check_cache(ByVal f As Func(Of ref(Of IPHostEntry), event_comb),
                                        ByVal exp As IPHostEntry) As Boolean
        assert(Not f Is Nothing)
        Dim c As ref(Of IPHostEntry) = Nothing
        c.renew()
        ' Inserting to cache is an asynchronous operation.
        assertion.is_true(timeslice_sleep_wait_until(
                Function() async_sync(f(c)),
                minutes_to_milliseconds(1)))
        assertion.is_false(c.empty())
        assertion.equal(exp, +c)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        For i As UInt32 = 0 To connectivity.golden_hosts.size() - uint32_1
            Dim host As String = Nothing
            host = connectivity.golden_hosts(i)
            Dim he As ref(Of IPHostEntry) = Nothing
            he.renew()
            If Not async_sync(dns_cache.resolve(host, he)) Then
                Continue For
            End If

            assertion.is_true(check_cache(Function(c) dns_cache.query_host_to_ip_cache(host, c), +he))
            For j As Int32 = 0 To array_size_i((+he).Aliases()) - 1
                Dim a As String = Nothing
                a = (+he).Aliases()(j)
                assertion.is_true(check_cache(Function(c) dns_cache.query_host_to_ip_cache(a, c), +he))
            Next
            For j As Int32 = 0 To array_size_i((+he).AddressList()) - 1
                Dim ip As String = Nothing
                ip = Convert.ToString((+he).AddressList()(j))
                assertion.is_true(check_cache(Function(c) dns_cache.query_ip_to_host_cache(ip, c), +he))
            Next
        Next
        Return True
    End Function
End Class
