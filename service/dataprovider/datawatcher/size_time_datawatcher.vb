
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.service.dataprovider.constants.size_time_datawatcher
Imports osi.service.dataprovider.constants.trigger_datawatcher

Public MustInherit Class size_time_datawatcher
    Inherits compare_datawatcher

    Protected Sub New(Optional ByVal interval_ms As Int64 = default_interval_ms)
        MyBase.New(field_count, interval_ms)
    End Sub

    Protected NotOverridable Overrides Function run_watch() As event_comb
        Dim ec As event_comb = Nothing
        Dim sz As ref(Of UInt64) = Nothing
        Dim tm As ref(Of UInt64) = Nothing
        Return New event_comb(Function() As Boolean
                                  sz = New ref(Of UInt64)()
                                  tm = New ref(Of UInt64)()
                                  ec = info(sz, tm)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      field(size_field, +sz)
                                      field(time_field, +tm)
                                      Return goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Protected MustOverride Function info(ByVal sz As ref(Of UInt64), ByVal tm As ref(Of UInt64)) As event_comb
End Class
