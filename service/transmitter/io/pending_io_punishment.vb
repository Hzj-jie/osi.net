
Imports osi.root.template
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.lock
Imports osi.root.envs
Imports osi.root.utils

Public Class pending_io_punishment
    Inherits pending_io_punishment(Of _false)

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idle_timeout_ms As Int64)
        MyBase.New(idle_timeout_ms)
    End Sub
End Class

Public Class pending_io_punishment(Of _THREAD_SAFE As _boolean)
    Private Shared ReadOnly thread_safe As Boolean
    Private ReadOnly idle_timeout_ms As Int64
    Private lock As slimlock.islimlock
    Private last_active_ms As Int64
    Private pending_counter As UInt32

    Shared Sub New()
        thread_safe = +alloc(Of _THREAD_SAFE)()
    End Sub

    Public Sub New(ByVal idle_timeout_ms As Int64)
        Me.idle_timeout_ms = idle_timeout_ms
        If thread_safe Then
            lock = New slimlock.monitorlock()
        Else
            lock = New broken_lock()
        End If
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
                lock.wait()
                If pending_counter < constants.max_pending_punishment Then
                    pending_counter += 1
                End If
                pending_time_ms = constants.io_pending_wait_ms
                If pending_counter > constants.min_pending_punishment Then
                    pending_time_ms += (pending_counter - constants.min_pending_punishment) * half_timeslice_length_ms
                End If
                lock.release()
            End If
            Return True
        End If
    End Function

    Public Sub reset()
        lock.wait()
        pending_counter = 0
        last_active_ms = nowadays.milliseconds()
        lock.release()
    End Sub
End Class
