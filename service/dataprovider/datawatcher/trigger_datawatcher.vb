
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.connector
Imports osi.service.dataprovider.constants.trigger_datawatcher

Public Class trigger_datawatcher
    Implements idatawatcher

    Private ReadOnly interval_ms As Int64
    Private ReadOnly inuse As ref(Of singleentry)

    Public Sub New(Optional ByVal interval_ms As Int64 = default_interval_ms)
        If interval_ms < 0 Then
            interval_ms = default_interval_ms
        End If
        Me.interval_ms = interval_ms
        Me.inuse = New ref(Of singleentry)()
    End Sub

    Public Sub trigger()
        assert(inuse IsNot Nothing)
        inuse.mark_in_use()
    End Sub

    Protected Overridable Function run_watch() As event_comb
        Return event_comb.succeeded()
    End Function

    Public Function watch(ByVal exp As expiration_controller) As event_comb Implements idatawatcher.watch
        Dim ec As event_comb = Nothing
        Return lifetime_event_comb(exp,
                                   Function() As Boolean
                                       ec = run_watch()
                                       Return waitfor(ec) AndAlso
                                              goto_next()
                                   End Function,
                                   Function() As Boolean
                                       If ec.end_result() Then
                                           Return waitfor(inuse, interval_ms) AndAlso
                                                  goto_next()
                                       Else
                                           inuse.mark_not_in_use()
                                           Return False
                                       End If
                                   End Function,
                                   Function() As Boolean
                                       If inuse.in_use() Then
                                           inuse.release()
                                           Return goto_end()
                                       Else
                                           Return goto_begin()
                                       End If
                                   End Function)
    End Function
End Class
