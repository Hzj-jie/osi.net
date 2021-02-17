
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.procedure
Imports osi.root.utt

Friend NotInheritable Class event_comb_case
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
        End If
        If i = 0 Then
            Return timeslice_length_ms
        End If
        Return CLng(Math.Ceiling(CDbl(i) / timeslice_length_ms) * timeslice_length_ms)
    End Function

    Private Function timeout_ms() As Int64
        If strict_time_limited Then
            Return normalize(busy_wait_ms) + normalize(sleep_wait_ms) + 16 * timeslice_length_ms
        End If
        Return max_int64
    End Function

    Public Overrides Function reserved_processors() As Int16
        If strict_time_limited Then
            Return CShort(Environment.ProcessorCount())
        End If
        Return 1
    End Function

    Protected Overrides Function run_case() As Boolean
        Dim this As Int32 = trigger_times()
        assertion.is_true(async_sync(create_event_comb(), timeout_ms()))
        assertion.more(trigger_times(), this)
        Return True
    End Function
End Class
