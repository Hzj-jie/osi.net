
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.dns

Public NotInheritable Class dns_cache_test
    Inherits repeat_event_comb_case_wrapper

    Public Sub New()
        MyBase.New(New dns_cache_case(), 1000)
    End Sub

    Public Overrides Function prepare() As Boolean
        If Not MyBase.prepare() Then
            Return False
        End If
        Dim p As pointer(Of connectivity.result_t) = Nothing
        p.renew()
        async_sync(connectivity.check_if_needed(p))
        dns_cache.clear_cache()
        Return (+p) >= connectivity.result_t.partial_dns_resolvable
    End Function

    Public Overrides Function reserved_processors() As Int16
        Return 2
    End Function

    Private NotInheritable Class dns_cache_case
        Inherits event_comb_case

        Private single_time As Int64

        Public Sub New()
            single_time = npos
        End Sub

        Private Shared Function execute() As event_comb
            Return event_comb.repeat(connectivity.golden_hosts.size(),
                                     Function(ByVal i As UInt32) As event_comb
                                         Dim ec As event_comb = Nothing
                                         Return New event_comb(Function() As Boolean
                                                                   ec = dns_cache.resolve(connectivity.golden_hosts(i),
                                                                                          New pointer(Of IPHostEntry)())
                                                                   Return waitfor(ec) AndAlso
                                                                          goto_next()
                                                               End Function,
                                                              Function() As Boolean
                                                                  Return ec.end_result() AndAlso
                                                                         goto_end()
                                                              End Function)
                                     End Function)
        End Function

        Public Overrides Function create() As event_comb
            Dim n As Int64 = 0
            Dim ec As event_comb = Nothing
            Return New event_comb(Function() As Boolean
                                      n = nowadays.milliseconds()
                                      ec = execute()
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      If single_time = npos Then
                                          single_time = nowadays.milliseconds() - n
                                      Else
                                          expectation.less_or_equal(nowadays.milliseconds() - n, single_time)
                                      End If
                                      Return goto_end()
                                  End Function)
        End Function
    End Class
End Class
