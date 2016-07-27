﻿
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.dns

Public Class connectivity_test
    Inherits event_comb_case

    Private Shared ReadOnly bad_hosts() As String

    Shared Sub New()
        bad_hosts = {"this.is.not.a.domain", "this.is.also.not.a.domain"}
    End Sub

    Public Overrides Function create() As event_comb
        Dim r As pointer(Of connectivity.result_T) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  r.renew()
                                  ec = connectivity.check(r, bad_hosts)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  assert_equal(+r, connectivity.result_t.fail)

                                  ec = connectivity.check(r, bad_hosts.append(connectivity.golden_hosts.as_array()))
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  assert_equal(+r, connectivity.result_t.partial_accessible)

                                  ec = connectivity.check_if_needed(r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  assert_equal(+r, connectivity.result_t.accessible)
                                  Return goto_end()
                              End Function)
    End Function
End Class
