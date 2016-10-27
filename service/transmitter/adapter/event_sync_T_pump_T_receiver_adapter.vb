
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.event
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

Public NotInheritable Class event_sync_T_pump_T_receiver_adapter
    Public Shared Function [New](Of T)(ByVal p As event_sync_T_pump(Of T),
                                       ByRef r As event_sync_T_pump_T_receiver_adapter(Of T)) As Boolean
        If p Is Nothing Then
            Return False
        Else
            r = New event_sync_T_pump_T_receiver_adapter(Of T)(p)
            Return True
        End If
    End Function

    Public Shared Function [New](Of T)(ByVal p As event_sync_T_pump(Of T)) As event_sync_T_pump_T_receiver_adapter(Of T)
        Dim r As event_sync_T_pump_T_receiver_adapter(Of T) = Nothing
        assert([New](p, r))
        Return r
    End Function

    Private Sub New()
    End Sub
End Class

Public Class event_sync_T_pump_T_receiver_adapter(Of T)
    Implements T_receiver(Of T)

    Private ReadOnly p As event_sync_T_pump(Of T)
    Private ReadOnly arrived_event As signal_event

    Public Sub New(ByVal p As event_sync_T_pump(Of T))
        assert(Not p Is Nothing)
        Me.p = p
        Me.arrived_event = New signal_event(p.pending())
        AddHandler p.data_arrived,
                   Sub()
                       arrived_event.mark()
                   End Sub
    End Sub

    Private Sub unmark_arrived_event()
        ' The following logic expects p.pending() = true is before p.data_arrived()
        If Not p.pending() Then
            arrived_event.unmark()
            If p.pending() Then
                arrived_event.mark()
            End If
        End If
    End Sub

    Public Function sense(ByVal pending As pointer(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements sensor.sense
        Dim end_time As Int64 = 0
        If timeout_ms < 0 Then
            end_time = max_int64
        Else
            end_time = nowadays.milliseconds + timeout_ms
        End If
        Return New event_comb(Function() As Boolean
                                  If p.pending() Then
                                      Return eva(pending, True) AndAlso
                                             goto_end()
                                  Else
                                      unmark_arrived_event()
                                      Return waitfor(arrived_event, end_time - nowadays.milliseconds()) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  If p.pending() Then
                                      Return eva(pending, True) AndAlso
                                             goto_end()
                                  ElseIf nowadays.milliseconds() < end_time Then
                                      Return goto_begin()
                                  Else
                                      Return eva(pending, False) AndAlso
                                             goto_end()
                                  End If
                              End Function)
    End Function

    Public Function receive(ByVal o As pointer(Of T)) As event_comb Implements T_pump(Of T).receive
        Return New event_comb(Function() As Boolean
                                  Dim i As T = Nothing
                                  If p.receive(i) Then
                                      unmark_arrived_event()
                                      Return eva(o, i) AndAlso
                                             goto_end()
                                  Else
                                      Return waitfor(sense())
                                  End If
                              End Function)
    End Function
End Class
