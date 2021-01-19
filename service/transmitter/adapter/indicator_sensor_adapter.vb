
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure

Public Class indicator_sensor_adapter
    Implements sensor

    Private ReadOnly i As indicator

    Public Sub New(ByVal i As indicator)
        Me.i = i
    End Sub

    Public Sub New(ByVal i As sync_indicator)
        Me.New(New sync_indicator_indicator_adapter(i))
    End Sub

    Public Function sense(ByVal pending As ref(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements sensor.sense
        Dim end_ms As Int64 = 0
        Dim ec As event_comb = Nothing
        Dim pending_counter As pending_io_punishment = Nothing
        Return New event_comb(Function() As Boolean
                                  If i Is Nothing Then
                                      Return False
                                  Else
                                      If timeout_ms < 0 Then
                                          end_ms = max_int64
                                      Else
                                          end_ms = nowadays.milliseconds() + timeout_ms
                                      End If
                                      pending.renew()
                                      pending_counter = New pending_io_punishment()
                                      Return goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  ec = i.indicate(pending)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If nowadays.milliseconds() >= end_ms Then
                                          Return goto_end()
                                      Else
                                          Dim pending_time_ms As Int64 = 0
                                          If pending_counter.record((+pending),
                                                                    pending_time_ms) Then
                                              Return waitfor(pending_time_ms) AndAlso
                                                     goto_prev()
                                          Else
                                              assert(+pending)
                                              Return goto_end()
                                          End If
                                      End If
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function
End Class
