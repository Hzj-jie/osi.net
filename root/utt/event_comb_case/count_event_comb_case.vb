
Imports osi.root.lock
Imports osi.root.procedure

Public MustInherit Class count_event_comb_case
    Inherits event_comb_case

    Private ReadOnly r As running_counter

    Protected Sub New(ByVal compare As Boolean,
                      ByVal scale As Double)
        r = New running_counter(compare, scale)
    End Sub

    Protected Sub New(ByVal compare As Boolean)
        r = New running_counter(compare:=compare)
    End Sub

    Protected Sub New(ByVal scale As Double)
        r = New running_counter(scale:=scale)
    End Sub

    Protected Sub New()
        r = New running_counter()
    End Sub

    Public Sub trigger()
        r.trigger()
    End Sub

    Public Sub inc_run()
        r.inc_run()
    End Sub

    Public Sub inc_finish()
        r.inc_finish()
    End Sub

    Public Function run_times() As Int32
        Return r.run_times()
    End Function

    Public Function finish_times() As Int32
        Return r.finish_times()
    End Function

    Public Function trigger_times() As Int32
        Return r.trigger_times()
    End Function

    Protected MustOverride Function create_case() As event_comb

    Public NotOverridable Overrides Function create() As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  inc_run()
                                  ec = create_case()
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  inc_finish()
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Overrides Function finish() As Boolean
        r.finish()
        Return MyBase.finish()
    End Function
End Class
