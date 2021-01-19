
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.selector
Imports osi.service.device
Imports osi.service.udp

Public NotInheritable Class udp_connector_test
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
                              with_remote_port(10000).
                              with_ipv4().create())
        End Sub

        Public Overrides Function create() As event_comb
            Dim r As ref(Of delegator) = Nothing
            Dim ec As event_comb = Nothing
            Return New event_comb(Function() As Boolean
                                      Dim d As idevice(Of async_getter(Of delegator)) = Nothing
                                      If assertion.is_true(c.create(d)) AndAlso
                                         assertion.is_not_null(d) AndAlso
                                         assertion.is_not_null(d.get()) Then
                                          r = New ref(Of delegator)()
                                          ec = d.get().get(r)
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      Else
                                          Return goto_end()
                                      End If
                                  End Function,
                                  Function() As Boolean
                                      If assertion.is_true(ec.end_result()) AndAlso
                                         assertion.is_false(r.empty()) Then
                                          assertion.is_true((+r).active())
                                          assertion.is_true((+r).fixed_sources())
                                          assertion.is_not_null((+r).p)
                                      End If
                                      Return goto_end()
                                  End Function)
        End Function
    End Class
End Class
