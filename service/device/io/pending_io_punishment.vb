
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.utils

Public Class pending_io_punishment
    Private ReadOnly idle_timeout_ms As Int64
    Private last_active_ms As Int64
    Private pending_counter As UInt32

    Public Sub New(ByVal idle_timeout_ms As Int64)
        Me.idle_timeout_ms = idle_timeout_ms
        reset()
    End Sub

    Public Sub New()
        Me.New(npos)
    End Sub

    Default Public ReadOnly Property invoke(ByVal has_data_this_round As Boolean) As event_comb
        Get
            Return New event_comb(Function() As Boolean
                                      Dim pending_time_ms As Int64 = 0
                                      If record(has_data_this_round, pending_time_ms) Then
                                          If idle_timeout_ms >= 0 AndAlso
                                             nowadays.milliseconds() - last_active_ms >= idle_timeout_ms Then
                                              Return False
                                          Else
                                              Return waitfor(pending_time_ms) AndAlso
                                                     goto_end()
                                          End If
                                      Else
                                          Return goto_end()
                                      End If
                                  End Function)
        End Get
    End Property

    Public Function record(ByVal has_data_this_round As Boolean,
                           ByRef pending_time_ms As Int64) As Boolean
        If has_data_this_round Then
            reset()
            Return False
        Else
            If suppress.pending_io_punishment.true_() Then
                pending_time_ms = 0
            Else
                If pending_counter < constants.max_pending_punishment Then
                    pending_counter += 1
                End If
                pending_time_ms = constants.io_pending_wait_ms
                If pending_counter > constants.min_pending_punishment Then
                    pending_time_ms += (pending_counter - constants.min_pending_punishment) * half_timeslice_length_ms
                End If
            End If
            Return True
        End If
    End Function

    Public Sub reset()
        pending_counter = 0
        last_active_ms = nowadays.milliseconds()
    End Sub
End Class
