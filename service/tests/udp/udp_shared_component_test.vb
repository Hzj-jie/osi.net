
Option Explicit On
Option Infer Off
Option Strict On

#If TODO Then

Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.udp

Public Class udp_shared_component_test
    Inherits event_comb_case

    Private Shared ReadOnly incoming_powerpoint As powerpoint
    Private Shared ReadOnly outgoing_powerpoint As powerpoint
    Private ReadOnly c As collection

    Shared Sub New()
        incoming_powerpoint = powerpoint.creator.[New]().
            with_ipv4().
            with_local_port(rnd_port()).
            create()
        outgoing_powerpoint = powerpoint.creator.[New]().
            with_ipv4().
            with_remote_port(incoming_powerpoint.local_port).
            with_host_or_ip("localhost").
            create()
    End Sub

    Public Sub New()
        MyBase.New()
        c = New collection()
    End Sub

    Private Function start_transporting() As event_comb

    End Function

    Public Overrides Function create() As event_comb
        Const size As Int32 = 10
        Dim ecs() As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ReDim ecs(size - 1)
                                  For i As Int32 = 0 To size - 1
                                      ecs(i) = start_transporting()
                                  Next
                                  Return waitfor(ecs) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ecs.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class

#End If
