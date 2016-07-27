
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.selector
Imports osi.service.device
Imports osi.service.udp

Public Class udp_connector_test
    Inherits repeat_event_comb_case_wrapper

    Public Sub New()
        MyBase.New(New udp_connector_case(), 100)
    End Sub

    Private Class udp_connector_case
        Inherits event_comb_case

        Private ReadOnly c As connector

        Public Sub New()
            MyBase.New()
            c = New connector(powerpoint.creator.[New]().
                              with_host_or_ip("www.example.org").
                              with_remote_port(10000).create())
        End Sub

        Public Overrides Function create() As event_comb
            Dim r As pointer(Of ref_client) = Nothing
            Dim ec As event_comb = Nothing
            Return New event_comb(Function() As Boolean
                                      Dim d As idevice(Of async_getter(Of ref_client)) = Nothing
                                      If assert_true(c.create(d)) AndAlso
                                         assert_not_nothing(d) AndAlso
                                         assert_not_nothing(d.get()) Then
                                          r = New pointer(Of ref_client)()
                                          ec = d.get().get(r)
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      Else
                                          Return goto_end()
                                      End If
                                  End Function,
                                  Function() As Boolean
                                      If assert_true(ec.end_result()) AndAlso
                                         assert_false(r.empty()) Then
                                          assert_not_nothing((+r).client())
                                          assert_false((+r).sources.null_or_empty())
                                          assert_not_nothing((+r).p)
                                      End If
                                      Return goto_end()
                                  End Function)
        End Function
    End Class
End Class
