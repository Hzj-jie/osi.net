
#If 0 Then
' This is not really helpful.

Imports System.Net
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.procedure
Imports osi.service.dns

Public Class dns_cache_test
    Inherits repeat_event_comb_case_wrapper

    Public Sub New()
        MyBase.New(New dns_cache_case(), 1000)
    End Sub

    Public Overrides Function prepare() As Boolean
        If MyBase.prepare() Then
            Dim p As pointer(Of connectivity.result_t) = Nothing
            p.renew()
            async_sync(connectivity.check_if_needed(p))
            osi.service.dns.clear_cache()
            Return (+p) >= connectivity.result_t.partial_dns_resolvable
        Else
            Return False
        End If
    End Function

    Private Class dns_cache_case
        Inherits event_comb_case

        Private single_time As Int64

        Public Sub New()
            single_time = npos
        End Sub

        Private Shared Function execute() As event_comb
            Dim i As Int32 = 0
            Return event_comb.while(Function(last_ec As event_comb, ByRef [continue] As Boolean) As Boolean
                                        If last_ec.end_result_or_null() Then
                                            i += 1
                                            [continue] = (i < connectivity.golden_hosts.size())
                                            Return True
                                        Else
                                            Return False
                                        End If
                                    End Function,
                                    Function() As event_comb
                                        Dim ec As event_comb = Nothing
                                        Return New event_comb(Function() As Boolean
                                                                  ec = osi.service.dns.resolve(
                                                                           connectivity.golden_hosts(i - 1),
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
                                          assert_less_or_equal(nowadays.milliseconds() - n, single_time)
                                      End If
                                      Return goto_end()
                                  End Function)
        End Function
    End Class
End Class
#End If
