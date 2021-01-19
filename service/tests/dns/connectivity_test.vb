
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.dns

Public NotInheritable Class connectivity_test
    Inherits event_comb_case

    Private Shared ReadOnly bad_hosts() As String

    Shared Sub New()
        bad_hosts = {"this.is.not.a.domain", "this.is.also.not.a.domain"}
    End Sub

    Public Overrides Function create() As event_comb
        Dim r As ref(Of connectivity.result_t) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  r.renew()
                                  ec = connectivity.check(r, bad_hosts)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assertion.is_true(ec.end_result())
                                  assertion.equal(+r, connectivity.result_t.fail)

                                  ec = connectivity.check(r, bad_hosts.append(connectivity.golden_hosts.as_array()))
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assertion.is_true(ec.end_result())
                                  assertion.equal(+r, connectivity.result_t.partial_accessible)

                                  ec = connectivity.check_if_needed(r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assertion.is_true(ec.end_result())
                                  assertion.equal(+r, connectivity.result_t.accessible)
                                  Return goto_end()
                              End Function)
    End Function
End Class
