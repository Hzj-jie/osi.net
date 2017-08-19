
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.root.lock
Imports osi.root.connector
Imports osi.root.threadpool
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.delegates

Friend Class event_comb_case
    Inherits count_case

    Private ReadOnly busy_wait_ms As Int32
    Private ReadOnly sleep_wait_ms As Int32
    Private ReadOnly strict_time_limited As Boolean

    Public Sub New(ByVal busy_wait_ms As Int32,
                   ByVal sleep_wait_ms As Int32,
                   ByVal strict_time_limited As Boolean)
        MyBase.New()
        Me.busy_wait_ms = busy_wait_ms
        Me.sleep_wait_ms = sleep_wait_ms
        Me.strict_time_limited = strict_time_limited
    End Sub

    Private Sub wait()
        fake_processor_work(busy_wait_ms)
    End Sub

    Private Function create_event_comb() As event_comb
        Dim i As Int32 = 0
        Return New event_comb(Function() As Boolean
                                  i = rnd_int(0, 2)
                                  If i = 0 Then
                                      wait()
                                  End If
                                  Return waitfor(sleep_wait_ms) AndAlso goto_next()
                              End Function,
                              Function() As Boolean
                                  If i = 1 Then
                                      wait()
                                  End If
                                  trigger()
                                  Return goto_end()
                              End Function)
    End Function

    Private Function normalize(ByVal i As Int64) As Int64
        If i < 0 Then
            Return 0
        ElseIf i = 0 Then
            Return timeslice_length_ms
        Else
            Return Math.Ceiling(CDbl(i) / timeslice_length_ms) * timeslice_length_ms
        End If
    End Function

    Private Function timeout_ms() As Int64
        If strict_time_limited Then
            Return normalize(busy_wait_ms) + normalize(sleep_wait_ms) + sixteen_timeslice_length_ms
        Else
            Return max_int64
        End If
    End Function

    Public Overrides Function reserved_processors() As Int16
        If strict_time_limited Then
            Return Environment.ProcessorCount()
        Else
            Return 1
        End If
    End Function

    Protected Overrides Function run_case() As Boolean
        Dim this As Int32 = 0
        this = trigger_times()
        assert_true(async_sync(create_event_comb(), timeout_ms()))
        assert_more(trigger_times(), this)
        Return True
    End Function
End Class
